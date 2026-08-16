namespace NetEvolve.ProjectBuilders.TUnit.Tests.Integration;

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using global::TUnit.Core.Interfaces;
using NetEvolve.ProjectBuilders;
using NetEvolve.ProjectBuilders.Abstractions;

public class TemporaryDirectoryTests
{
    [Test]
    public async Task CreateAsync_ViaObjectBuilderContract_CreatesUnderlyingDirectory(
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        // Arrange - exercises the IObjectBuilder.CreateAsync passthrough directly, as a caller
        // holding only the interface (not the concrete type) would.
        IObjectBuilder directory = new TemporaryDirectory();

        // Act
        await directory.CreateAsync(cancellationToken);

        // Assert
        try
        {
            _ = await Assert.That(Directory.Exists(directory.FullPath)).IsTrue();
        }
        finally
        {
            await directory.DisposeAsync();
        }
    }

    [Test]
    public async Task CreateDirectory_WithName_CreatesSubdirectory()
    {
        // Arrange
        var directory = new TemporaryDirectory();
        await ((IAsyncInitializer)directory).InitializeAsync();

        // Act
        var subdirectory = directory.CreateDirectory("sub");

        // Assert
        try
        {
            _ = await Assert.That(Directory.Exists(subdirectory.FullPath)).IsTrue();
        }
        finally
        {
            await directory.DisposeAsync();
        }
    }

    [Test]
    public async Task CreateFile_WithFileName_ReturnsWritableStream()
    {
        // Arrange
        var directory = new TemporaryDirectory();
        await ((IAsyncInitializer)directory).InitializeAsync();

        // Act
        await using var stream = directory.CreateFile("file.txt");

        // Assert
        try
        {
            _ = await Assert.That(stream.CanWrite).IsTrue();
        }
        finally
        {
            await directory.DisposeAsync();
        }
    }

    [Test]
    public async Task GetFilePath_WithFileName_ReturnsCombinedPath()
    {
        // Arrange
        var directory = new TemporaryDirectory();
        await ((IAsyncInitializer)directory).InitializeAsync();

        // Act
        var path = directory.GetFilePath("file.txt");

        // Assert
        try
        {
            _ = await Assert.That(path).IsEqualTo(Path.Combine(directory.FullPath, "file.txt"));
        }
        finally
        {
            await directory.DisposeAsync();
        }
    }

    [Test]
    public async Task DisposeAsync_RemovesDirectory()
    {
        // Arrange
        var directory = new TemporaryDirectory();
        await ((IAsyncInitializer)directory).InitializeAsync();
        var path = directory.FullPath;

        // Act
        await directory.DisposeAsync();

        // Assert
        _ = await Assert.That(Directory.Exists(path)).IsFalse();
    }
}
