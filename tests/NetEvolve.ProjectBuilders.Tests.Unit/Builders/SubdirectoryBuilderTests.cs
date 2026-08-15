namespace NetEvolve.ProjectBuilders.Tests.Unit.Builders;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Builders;

public class SubdirectoryBuilderTests
{
    [Test]
    public async Task CreateAsync_CompletesImmediately(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        // Arrange
        await using var root = new TemporaryDirectoryBuilder();
        var subdirectory = root.CreateDirectory("sub");

        // Act & Assert
        await subdirectory.CreateAsync(cancellationToken);
    }

    [Test]
    public async Task FullPath_ReturnsDirectoryPath()
    {
        // Arrange
        await using var root = new TemporaryDirectoryBuilder();

        // Act
        var subdirectory = root.CreateDirectory("sub");

        // Assert
        _ = await Assert.That(Directory.Exists(subdirectory.FullPath)).IsTrue();
    }

    [Test]
    public async Task CreateDirectory_WithNestedName_CreatesNestedDirectory()
    {
        // Arrange
        await using var root = new TemporaryDirectoryBuilder();
        var subdirectory = root.CreateDirectory("sub");

        // Act
        var nested = subdirectory.CreateDirectory("nested");

        // Assert
        _ = await Assert.That(Directory.Exists(nested.FullPath)).IsTrue();
    }

    [Test]
    public async Task CreateDirectory_WithNullName_ThrowsArgumentException()
    {
        // Arrange
        await using var root = new TemporaryDirectoryBuilder();
        var subdirectory = root.CreateDirectory("sub");

        // Act
        void Act() => subdirectory.CreateDirectory(null!);

        // Assert
        _ = Assert.Throws<ArgumentException>(Act);
    }

    [Test]
    public async Task CreateFile_WithFileName_ReturnsWritableStream()
    {
        // Arrange
        await using var root = new TemporaryDirectoryBuilder();
        var subdirectory = root.CreateDirectory("sub");

        // Act
        await using var stream = subdirectory.CreateFile("file.txt");

        // Assert
        _ = await Assert.That(stream.CanWrite).IsTrue();
    }

    [Test]
    public async Task CreateFile_WithExistingFileName_ThrowsArgumentException()
    {
        // Arrange
        await using var root = new TemporaryDirectoryBuilder();
        var subdirectory = root.CreateDirectory("sub");
        await using (var stream = subdirectory.CreateFile("file.txt"))
        {
            // ensure the stream is disposed before recreating
        }

        // Act
        void Act() => _ = subdirectory.CreateFile("file.txt");

        // Assert
        _ = Assert.Throws<ArgumentException>(Act);
    }

    [Test]
    public async Task CreateFile_WithNullFileName_ThrowsArgumentException()
    {
        // Arrange
        await using var root = new TemporaryDirectoryBuilder();
        var subdirectory = root.CreateDirectory("sub");

        // Act
        void Act() => subdirectory.CreateFile(null!);

        // Assert
        _ = Assert.Throws<ArgumentException>(Act);
    }

    [Test]
    public async Task GetFilePath_WithFileName_ReturnsCombinedPath()
    {
        // Arrange
        await using var root = new TemporaryDirectoryBuilder();
        var subdirectory = root.CreateDirectory("sub");

        // Act
        var path = subdirectory.GetFilePath("file.txt");

        // Assert
        _ = await Assert.That(path).IsEqualTo(Path.Combine(subdirectory.FullPath, "file.txt"));
    }

    [Test]
    public async Task DisposeAsync_CompletesImmediately()
    {
        // Arrange
        await using var root = new TemporaryDirectoryBuilder();
        var subdirectory = root.CreateDirectory("sub");

        // Act & Assert
        await subdirectory.DisposeAsync();
    }
}
