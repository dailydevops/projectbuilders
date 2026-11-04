namespace NetEvolve.ProjectBuilders.Tests.Unit.Helpers;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Helpers;

public class SharedHttpClientTests
{
    [Test]
    public async Task Instance_GetAsync_WithCanceledToken_ThrowsOperationCanceledException()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act
        async Task Act() => _ = await SharedHttpClient.Instance.GetAsync(new Uri("https://example.com"), cts.Token);

        // Assert
        _ = await Assert.ThrowsAsync<OperationCanceledException>(Act);
    }

    [Test]
    public async Task Instance_IsSingleton()
    {
        // Act
        var first = SharedHttpClient.Instance;
        var second = SharedHttpClient.Instance;

        // Assert
        _ = await Assert.That(first).IsSameReferenceAs(second);
    }

    [Test]
    public async Task SendAsync_ServerErrorThenSuccess_RetriesUntilSuccess(
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();
        // Arrange
        var requestCount = 0;
        using var listener = StartListener(
            context =>
            {
                requestCount++;
                context.Response.StatusCode = requestCount < 3 ? 500 : 200;
            },
            out var prefix
        );

        // Act
        using var response = await SharedHttpClient.Instance.GetAsync(new Uri(prefix), cancellationToken);

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
            _ = await Assert.That(requestCount).IsEqualTo(3);
        }
    }

    [Test]
    public async Task SendAsync_TooManyRequestsWithRetryAfterDelta_RetriesUntilSuccess(
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();
        // Arrange
        var requestCount = 0;
        using var listener = StartListener(
            context =>
            {
                requestCount++;
                if (requestCount == 1)
                {
                    context.Response.StatusCode = 429;
                    context.Response.Headers.Add("Retry-After", "1");
                }
                else
                {
                    context.Response.StatusCode = 200;
                }
            },
            out var prefix
        );

        // Act
        using var response = await SharedHttpClient.Instance.GetAsync(new Uri(prefix), cancellationToken);

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
            _ = await Assert.That(requestCount).IsEqualTo(2);
        }
    }

    [Test]
    public async Task SendAsync_TooManyRequestsWithRetryAfterHttpDate_RetriesUntilSuccess(
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();
        // Arrange
        var requestCount = 0;
        using var listener = StartListener(
            context =>
            {
                requestCount++;
                if (requestCount == 1)
                {
                    context.Response.StatusCode = 429;
                    context.Response.Headers.Add("Retry-After", DateTimeOffset.UtcNow.AddSeconds(1).ToString("R"));
                }
                else
                {
                    context.Response.StatusCode = 200;
                }
            },
            out var prefix
        );

        // Act
        using var response = await SharedHttpClient.Instance.GetAsync(new Uri(prefix), cancellationToken);

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
            _ = await Assert.That(requestCount).IsEqualTo(2);
        }
    }

    [Test]
    public async Task SendAsync_AlwaysServerError_ReturnsLastResponseAfterMaxRetries(
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();
        // Arrange - the handler's last attempt returns whatever it got instead of throwing,
        // so 5 consecutive server errors surface as a final 500 response, not an exception.
        var requestCount = 0;
        using var listener = StartListener(
            context =>
            {
                requestCount++;
                context.Response.StatusCode = 500;
            },
            out var prefix
        );

        // Act
        using var response = await SharedHttpClient.Instance.GetAsync(new Uri(prefix), cancellationToken);

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.InternalServerError);
            _ = await Assert.That(requestCount).IsEqualTo(5);
        }
    }

    [Test]
    public async Task SendAsync_ClientTimeout_RetriesThenThrowsTaskCanceledException(
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();
        // Arrange - every response is delayed well beyond the client's own timeout, exercising the
        // TaskCanceledException-from-HttpClient.Timeout retry/rethrow branch (distinct from caller
        // cancellation, since `cancellationToken` here is never cancelled).
        using var listener = StartListener(
            context =>
            {
                Thread.Sleep(500);
                context.Response.StatusCode = 200;
            },
            out var prefix
        );
        using var innerHandler = new SocketsHttpHandler();
        using var retryHandler = new SharedHttpClient.HttpRetryMessageHandler(innerHandler);
        using var client = new HttpClient(retryHandler) { Timeout = TimeSpan.FromMilliseconds(100) };

        // Act
        async Task Act() => _ = await client.GetAsync(new Uri(prefix), cancellationToken);

        // Assert
        _ = await Assert.ThrowsAsync<TaskCanceledException>(Act);
    }

    [Test]
    public async Task SendAsync_ConnectionRefused_RetriesThenThrowsHttpRequestException()
    {
        // Arrange - nothing is listening on this port, so every attempt fails with HttpRequestException,
        // exercising the exception-retry branch until the final attempt rethrows.
        var port = GetFreePort();
        var uri = new Uri($"http://127.0.0.1:{port}/");

        // Act
        async Task Act() => _ = await SharedHttpClient.Instance.GetAsync(uri);

        // Assert
        _ = await Assert.ThrowsAsync<HttpRequestException>(Act);
    }

    // The handler only retries a TaskCanceledException whose token differs from the caller's token.
    // The tests below therefore always pass a non-default token to the handler and throw a
    // TaskCanceledException carrying the default token from the stub.
    // Lower-bound timing assertions only (never upper bounds) keep them stable on slow runners.

    [Test]
    public async Task SendAsync_PersistentTaskCanceledException_RetriesFiveTimesThenRethrows()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        using var inner = new StubHandler((_, _) => throw new TaskCanceledException());
        using var retryHandler = new SharedHttpClient.HttpRetryMessageHandler(inner);
        using var invoker = new HttpMessageInvoker(retryHandler);
        using var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/");
        var stopwatch = Stopwatch.StartNew();

        // Act
        async Task Act() => _ = await invoker.SendAsync(request, cts.Token);

        // Assert
        _ = await Assert.ThrowsAsync<TaskCanceledException>(Act);
        stopwatch.Stop();
        using (Assert.Multiple())
        {
            _ = await Assert.That(inner.CallCount).IsEqualTo(5);
            // 200 + 400 + 600 + 800 ms backoff
            _ = await Assert.That(stopwatch.Elapsed).IsGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(1800));
        }
    }

    [Test]
    public async Task SendAsync_TaskCanceledExceptionTwiceThenSuccess_ReturnsResponseAfterThreeAttempts()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        using var inner = new StubHandler(
            (attempt, _) => attempt < 3 ? throw new TaskCanceledException() : new HttpResponseMessage(HttpStatusCode.OK)
        );
        using var retryHandler = new SharedHttpClient.HttpRetryMessageHandler(inner);
        using var invoker = new HttpMessageInvoker(retryHandler);
        using var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/");

        // Act
        using var response = await invoker.SendAsync(request, cts.Token);

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
            _ = await Assert.That(inner.CallCount).IsEqualTo(3);
        }
    }

    [Test]
    public async Task SendAsync_TaskCanceledExceptionWithCallerToken_PropagatesWithoutRetry()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        using var inner = new StubHandler((_, _) => throw new TaskCanceledException(null, null, cts.Token));
        using var retryHandler = new SharedHttpClient.HttpRetryMessageHandler(inner);
        using var invoker = new HttpMessageInvoker(retryHandler);
        using var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/");

        // Act
        async Task Act() => _ = await invoker.SendAsync(request, cts.Token);

        // Assert
        _ = await Assert.ThrowsAsync<TaskCanceledException>(Act);
        _ = await Assert.That(inner.CallCount).IsEqualTo(1);
    }

    [Test]
    public async Task SendAsync_PreCanceledToken_ThrowsWithoutCallingInnerHandler()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();
        using var inner = new StubHandler((_, _) => new HttpResponseMessage(HttpStatusCode.OK));
        using var retryHandler = new SharedHttpClient.HttpRetryMessageHandler(inner);
        using var invoker = new HttpMessageInvoker(retryHandler);
        using var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/");

        // Act
        async Task Act() => _ = await invoker.SendAsync(request, cts.Token);

        // Assert
        _ = await Assert.ThrowsAsync<OperationCanceledException>(Act);
        _ = await Assert.That(inner.CallCount).IsEqualTo(0);
    }

    [Test]
    [Arguments(HttpStatusCode.InternalServerError)]
    [Arguments(HttpStatusCode.RequestTimeout)]
    [Arguments(HttpStatusCode.TooManyRequests)]
    public async Task SendAsync_PersistentRetryableStatus_HonorsRetryAfterAndDisposesDiscardedResponses(
        HttpStatusCode statusCode
    )
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var responses = new List<TrackingContent>();
        using var inner = new StubHandler(
            (_, _) =>
            {
                var content = new TrackingContent();
                responses.Add(content);
                var message = new HttpResponseMessage(statusCode) { Content = content };
                message.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromMilliseconds(300));
                return message;
            }
        );
        using var retryHandler = new SharedHttpClient.HttpRetryMessageHandler(inner);
        using var invoker = new HttpMessageInvoker(retryHandler);
        using var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/");
        var stopwatch = Stopwatch.StartNew();

        // Act
        using var response = await invoker.SendAsync(request, cts.Token);
        stopwatch.Stop();

        // Assert - four discarded responses are disposed, the last one is handed to the caller
        using (Assert.Multiple())
        {
            _ = await Assert.That(response.StatusCode).IsEqualTo(statusCode);
            _ = await Assert.That(inner.CallCount).IsEqualTo(5);
            _ = await Assert.That(responses.Take(4).All(x => x.Disposed)).IsTrue();
            _ = await Assert.That(responses[4].Disposed).IsFalse();
            // 4 x 300 ms Retry-After
            _ = await Assert.That(stopwatch.Elapsed).IsGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(1100));
        }
    }

    private static HttpListener StartListener(Action<HttpListenerContext> handler, out string prefix)
    {
        var port = GetFreePort();
        prefix = $"http://127.0.0.1:{port}/";

        var listener = new HttpListener();
        listener.Prefixes.Add(prefix);
        listener.Start();

        _ = Task.Run(async () =>
        {
            while (listener.IsListening)
            {
                try
                {
                    var context = await listener.GetContextAsync().ConfigureAwait(false);
                    handler(context);
                    context.Response.Close();
                }
                catch (HttpListenerException)
                {
                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
            }
        });

        return listener;
    }

    private static int GetFreePort()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }

    private sealed class StubHandler(Func<int, CancellationToken, HttpResponseMessage> responder) : HttpMessageHandler
    {
        private int _callCount;

        public int CallCount => Volatile.Read(ref _callCount);

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken
        ) => Task.FromResult(responder(Interlocked.Increment(ref _callCount), cancellationToken));
    }

    private sealed class TrackingContent : HttpContent
    {
        public bool Disposed { get; private set; }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context) => Task.CompletedTask;

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            Disposed = true;
            base.Dispose(disposing);
        }
    }
}
