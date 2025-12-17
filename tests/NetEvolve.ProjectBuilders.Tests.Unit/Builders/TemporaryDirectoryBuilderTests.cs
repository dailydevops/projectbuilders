namespace NetEvolve.ProjectBuilders.Tests.Unit.Builders;

using System;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Builders;

public class TemporaryDirectoryBuilderTests
{
    [Test]
    public async Task CreateFile_WithFileName_ReturnsFile()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        const string fileName = "file.txt";

        // Act
        var file = directory.CreateFile(fileName);

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(file).IsNotNull();
            _ = await Assert.That(file.CanWrite).IsTrue();
        }
    }

    [Test]
    public async Task CreateFile_WithExistingFileName_ThrowsArgumentException()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        const string fileName = "file.txt";
        _ = directory.CreateFile(fileName);

        // Act
        void Act() => directory.CreateFile(fileName);

        // Assert
        _ = Assert.Throws<ArgumentException>(Act);
    }

    [Test]
    public async Task GetFilePath_WithFileName_ReturnsFilePath()
    {
        // Arrange
        await using var tempDirectory = new TemporaryDirectoryBuilder();
        const string fileName = "file.txt";

        // Act
        var filePath = tempDirectory.GetFilePath(fileName);

        // Assert
        _ = await Assert.That(filePath).IsEqualTo(Path.Combine(tempDirectory.FullPath, fileName));
    }

    [Test]
    public async Task GetFilePath_WithNullFileName_ThrowsArgumentNullException()
    {
        // Arrange
        await using var tempDirectory = new TemporaryDirectoryBuilder();
        const string? fileName = null;

        // Act
        void Act() => tempDirectory.GetFilePath(fileName!);

        // Assert
        _ = Assert.Throws<ArgumentNullException>(Act);
    }

    [Test]
    public async Task GetFilePath_WithEmptyFileName_ThrowsArgumentException()
    {
        // Arrange
        await using var tempDirectory = new TemporaryDirectoryBuilder();
        var fileName = string.Empty;

        // Act
        void Act() => tempDirectory.GetFilePath(fileName);

        // Assert
        _ = Assert.Throws<ArgumentException>(Act);
    }
}
