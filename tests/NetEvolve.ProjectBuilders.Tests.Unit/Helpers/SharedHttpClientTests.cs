namespace NetEvolve.ProjectBuilders.Tests.Unit.Helpers;

using System;
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
}
