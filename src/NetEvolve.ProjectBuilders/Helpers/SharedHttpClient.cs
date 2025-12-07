namespace NetEvolve.ProjectBuilders.Helpers;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Provides a shared, reusable HttpClient instance with automatic retry logic for resilient HTTP operations.
/// </summary>
/// <remarks>
/// <para>
/// This static utility class manages a singleton <see cref="HttpClient"/> instance that's optimized for
/// use throughout the ProjectBuilders library. It implements exponential backoff retry logic to handle
/// transient failures gracefully.
/// </para>
/// <para>
/// The client is configured with:
/// <list type="bullet">
/// <item><description>Connection pooling with 1-minute idle timeout</description></item>
/// <item><description>1-minute connection lifetime limit</description></item>
/// <item><description>Up to 5 automatic retries for transient failures</description></item>
/// <item><description>Exponential backoff delays (200ms, 400ms, 600ms, 800ms, 1000ms)</description></item>
/// <item><description>Respect for Retry-After headers</description></item>
/// </list>
/// </para>
/// <para>
/// This pattern ensures:
/// <list type="bullet">
/// <item><description>Efficient connection reuse</description></item>
/// <item><description>Resilience against transient network failures</description></item>
/// <item><description>No unnecessary DNS lookups</description></item>
/// <item><description>Proper handling of rate limiting and server errors</description></item>
/// </list>
/// </para>
/// </remarks>
internal static class SharedHttpClient
{
    private static readonly Lazy<HttpClient> _instance = new(
        CreateHttpClient,
        LazyThreadSafetyMode.ExecutionAndPublication
    );

    /// <summary>
    /// Gets the shared singleton HttpClient instance.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This instance is lazily initialized on first access and reused throughout the application lifetime.
    /// It should never be disposed by callers.
    /// </para>
    /// </remarks>
    /// <value>The shared HttpClient instance configured with retry logic and connection pooling.</value>
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

    /// <summary>
    /// Provides automatic retry logic with exponential backoff for transient HTTP failures.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This delegating handler intercepts HTTP requests and automatically retries them up to 5 times
    /// when transient failures occur. It respects the Retry-After header from the server if present.
    /// </para>
    /// <para>
    /// Retryable conditions:
    /// <list type="bullet">
    /// <item><description>500+ status codes (server errors)</description></item>
    /// <item><description>408 Request Timeout</description></item>
    /// <item><description>429 Too Many Requests</description></item>
    /// <item><description>HttpRequestException (network failures)</description></item>
    /// <item><description>TaskCanceledException (timeouts, not explicit cancellation)</description></item>
    /// </list>
    /// </para>
    /// </remarks>
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
