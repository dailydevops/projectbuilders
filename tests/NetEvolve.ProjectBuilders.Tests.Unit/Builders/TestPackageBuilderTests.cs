namespace NetEvolve.ProjectBuilders.Tests.Unit.Builders;

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Builders;

// Note: TestPackageBuilder.CreateAsync's happy path (actually invoking `dotnet pack` to pack a
// project) requires the .NET SDK plus network access for restore and is out of scope for a fast,
// deterministic unit test - that behavior is covered by a real integration test instead.
public class TestPackageBuilderTests
{
    [Test]
    public async Task SetPackagePaths_WithNull_ThrowsArgumentNullException()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await using var builder = new TestPackageBuilder(directory);

        // Act
        void Act() => builder.SetPackagePaths(null!);

        // Assert
        _ = Assert.Throws<ArgumentNullException>(Act);
    }

    [Test]
    public async Task SetPackagePaths_WithWhitespaceEntries_FiltersThemOut()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await using var builder = new TestPackageBuilder(directory);
        string[] paths = ["  ", string.Empty, "\t", "C:\\project\\a.csproj"];

        // Act
        builder.SetPackagePaths(paths);

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(builder._packagePaths.Count).IsEqualTo(1);
            _ = await Assert.That(builder._packagePaths).Contains("C:\\project\\a.csproj");
        }
    }

    [Test]
    public async Task SetPackagePaths_WithDuplicates_Dedupes()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await using var builder = new TestPackageBuilder(directory);
        string[] paths = ["C:\\project\\a.csproj", "C:\\PROJECT\\A.CSPROJ", "C:\\project\\b.csproj"];

        // Act
        builder.SetPackagePaths(paths);

        // Assert
        _ = await Assert.That(builder._packagePaths.Count).IsEqualTo(2);
    }

    [Test]
    public async Task SetPackagePaths_CalledTwice_AccumulatesAcrossCalls()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await using var builder = new TestPackageBuilder(directory);

        // Act
        builder.SetPackagePaths(["C:\\project\\a.csproj"]);
        builder.SetPackagePaths(["C:\\project\\a.csproj", "C:\\project\\b.csproj"]);

        // Assert
        _ = await Assert.That(builder._packagePaths.Count).IsEqualTo(2);
    }

    [Test]
    public async Task CreateAsync_WithCancelledToken_ThrowsOperationCanceledException()
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
    public async Task CreateAsync_WhenAlreadyInitialized_ReturnsEarlyWithoutCreatingDirectory(
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await using var builder = new TestPackageBuilder(directory);
        builder._isInitialized = true;

        // Act
        await builder.CreateAsync(cancellationToken);

        // Assert - the directory was never created because CreateAsync returned early.
        _ = await Assert.That(Directory.Exists(directory.FullPath)).IsFalse();
    }

    [Test]
    [SuppressMessage(
        "Reliability",
        "CA2000:Dispose objects before losing scope",
        Justification = "Ownership of directory is transferred to builder and disposed via builder.DisposeAsync."
    )]
    public async Task DisposeAsync_DisposesDirectory(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        // Arrange
        var directory = new TemporaryDirectoryBuilder();
        await directory.CreateAsync(cancellationToken);
        var builder = new TestPackageBuilder(directory);

        // Act
        await builder.DisposeAsync();

        // Assert
        _ = await Assert.That(Directory.Exists(directory.FullPath)).IsFalse();
    }
}
