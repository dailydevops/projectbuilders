namespace NetEvolve.ProjectBuilders.Tests.Unit.Builders;

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Builders;

// Note: TestPackageBuilder.CreateAsync's happy path (actually invoking nuget.exe / the nuget CLI
// to pack a project) requires a real nuget executable plus network access to download it and is
// out of scope for a fast, deterministic unit test - that behavior is integration-test territory.
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
        Justification = "Ownership of nugetFolder is transferred to builder and disposed via builder.DisposeAsync/GetNuGetExeAsync."
    )]
    public async Task GetNuGetExeAsync_WhenExeAlreadyExists_SkipsDownloadAndReturnsPath(
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await using var builder = new TestPackageBuilder(directory);

        var nugetFolder = new TemporaryDirectoryBuilder();
        await nugetFolder.CreateAsync(cancellationToken);
        builder._nugetFolder = nugetFolder;

        var expectedFileName = $"nuget-{builder._identifier:N}.exe";
        var expectedFilePath = Path.Combine(nugetFolder.FullPath, expectedFileName);
        await File.WriteAllTextAsync(expectedFilePath, "fake-exe-content", cancellationToken);

        // Act
        var result = await builder.GetNuGetExeAsync(cancellationToken);

        // Assert - no download happened (would fail here without network); the pre-existing file is returned.
        using (Assert.Multiple())
        {
            _ = await Assert.That(result).IsEqualTo(expectedFilePath);
            _ = await Assert
                .That(await File.ReadAllTextAsync(expectedFilePath, cancellationToken))
                .IsEqualTo("fake-exe-content");
        }
    }

    [Test]
    [SuppressMessage(
        "Reliability",
        "CA2000:Dispose objects before losing scope",
        Justification = "Ownership of directory is transferred to builder and disposed via builder.DisposeAsync."
    )]
    public async Task DisposeAsync_WithoutNugetFolder_DisposesDirectoryOnly(
        CancellationToken cancellationToken = default
    )
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

    [Test]
    [SuppressMessage(
        "Reliability",
        "CA2000:Dispose objects before losing scope",
        Justification = "Ownership of directory/nugetFolder is transferred to builder and disposed via builder.DisposeAsync."
    )]
    public async Task DisposeAsync_WithNugetFolder_DisposesBothDirectories(
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();
        // Arrange
        var directory = new TemporaryDirectoryBuilder();
        await directory.CreateAsync(cancellationToken);
        var builder = new TestPackageBuilder(directory);

        var nugetFolder = new TemporaryDirectoryBuilder();
        await nugetFolder.CreateAsync(cancellationToken);
        builder._nugetFolder = nugetFolder;

        // Act
        await builder.DisposeAsync();

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(Directory.Exists(directory.FullPath)).IsFalse();
            _ = await Assert.That(Directory.Exists(nugetFolder.FullPath)).IsFalse();
        }
    }
}
