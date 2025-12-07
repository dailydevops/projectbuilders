namespace NetEvolve.ProjectBuilders.Helpers;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

internal static class SharedHttpClient
{
    private static readonly Lazy<HttpClient> _instance = new(
        CreateHttpClient,
        LazyThreadSafetyMode.ExecutionAndPublication
    );

    public static HttpClient Instance { get; } = _instance.Value;

    [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "As designed.")]
    private static HttpClient CreateHttpClient()
    {
        var socketHandler = new SocketsHttpHandler()
        {
            PooledConnectionIdleTimeout = TimeSpan.FromMinutes(1),
            PooledConnectionLifetime = TimeSpan.FromMinutes(1),
        };

        return new HttpClient(new HttpRetryMessageHandler(socketHandler), disposeHandler: true);
    }

    private sealed class HttpRetryMessageHandler(HttpMessageHandler handler) : DelegatingHandler(handler)
    {
        private const int MaxRetries = 5;

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken
        )
        {
            for (var i = 1; i <= MaxRetries; i++)
            {
                var delay = TimeSpan.FromMilliseconds(i * 200);
                HttpResponseMessage? result = null;

                try
                {
                    result = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
                    if (
                        !ValidAttemp(i)
                        && (
                            result.StatusCode >= HttpStatusCode.InternalServerError
                            || result.StatusCode is HttpStatusCode.RequestTimeout or HttpStatusCode.TooManyRequests
                        )
                    )
                    {
                        if (result.Headers.RetryAfter is { Date: { } date })
                        {
                            delay = date - DateTimeOffset.UtcNow;
                        }
                        else if (result.Headers.RetryAfter is { Delta: { } delta })
                        {
                            delay = delta;
                        }

                        result.Dispose();
                    }
                    else
                    {
                        return result;
                    }
                }
                catch (HttpRequestException)
                {
                    result?.Dispose();
                    if (ValidAttemp(i))
                    {
                        throw;
                    }
                }
                catch (TaskCanceledException ex) when (ex.CancellationToken != cancellationToken) // catch "The request was canceled due to the configured HttpClient.Timeout of 100 seconds elapsing"
                {
                    result?.Dispose();
                    if (ValidAttemp(i))
                    {
                        throw;
                    }
                }

                await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
            }

            throw new InvalidOperationException("The code should not reach this point.");
        }

        private static bool ValidAttemp(int i) => i >= MaxRetries;
    }
}
