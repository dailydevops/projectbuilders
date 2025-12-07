namespace NetEvolve.ProjectBuilders.Tests.Unit.Builders;

using System;
using System.IO;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Builders;

public class TempDirectoryBuilderTests
{
    [Test]
    public async Task GetFilePath_WithFileName_ReturnsFilePath()
    {
        // Arrange
        await using var tempDirectory = new TempDirectoryBuilder();
        var fileName = "file.txt";

        // Act
        var filePath = tempDirectory.GetFilePath(fileName);

        // Assert
        _ = await Assert.That(filePath).IsEqualTo(Path.Combine(tempDirectory.FullPath, fileName));
    }

    [Test]
    public async Task GetFilePath_WithNullFileName_ThrowsArgumentNullException()
    {
        // Arrange
        await using var tempDirectory = new TempDirectoryBuilder();
        string? fileName = null;

        // Act
        void Act() => tempDirectory.GetFilePath(fileName);

        // Assert
        _ = Assert.Throws<ArgumentNullException>(Act);
    }

    [Test]
    public async Task GetFilePath_WithEmptyFileName_ThrowsArgumentException()
    {
        // Arrange
        await using var tempDirectory = new TempDirectoryBuilder();
        var fileName = string.Empty;

        // Act
        void Act() => tempDirectory.GetFilePath(fileName);

        // Assert
        _ = Assert.Throws<ArgumentException>(Act);
    }
}
