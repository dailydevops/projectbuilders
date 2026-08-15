namespace NetEvolve.ProjectBuilders.Tests.Unit.Builders;

using System;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Builders;

public class TestPackageBuilderTests
{
    [Test]
    public async Task CreateAsync_WithCanceledToken_ThrowsOperationCanceledException()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await using var builder = new TestPackageBuilder(directory);
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act
        async Task Act() => await builder.CreateAsync(cts.Token);

        // Assert
        _ = await Assert.ThrowsAsync<OperationCanceledException>(Act);
    }

    [Test]
    public async Task GetCliWrapAsync_WithCanceledToken_ThrowsOperationCanceledException()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await using var builder = new TestPackageBuilder(directory);
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act
        async Task Act() => _ = await builder.GetCliWrapAsync(cts.Token);

        // Assert
        _ = await Assert.ThrowsAsync<OperationCanceledException>(Act);
    }

    [Test]
    public async Task GetNuGetExeAsync_WithCanceledToken_ThrowsOperationCanceledException()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await using var builder = new TestPackageBuilder(directory);
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act
        async Task Act() => _ = await builder.GetNuGetExeAsync(cts.Token);

        // Assert
        _ = await Assert.ThrowsAsync<OperationCanceledException>(Act);
    }

    [Test]
    public async Task DownloadNuGetClientAsync_WithCanceledToken_ThrowsOperationCanceledException()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await using var builder = new TestPackageBuilder(directory);
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act
        async Task Act() => await builder.DownloadNuGetClientAsync("https://example.com/file", "file.exe", cts.Token);

        // Assert
        _ = await Assert.ThrowsAsync<OperationCanceledException>(Act);
    }
}
