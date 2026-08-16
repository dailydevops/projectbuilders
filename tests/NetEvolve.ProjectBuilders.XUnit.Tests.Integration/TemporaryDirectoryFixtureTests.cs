namespace NetEvolve.ProjectBuilders.XUnit.Tests.Integration;

using System.IO;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders;
using Xunit;

public class TemporaryDirectoryFixtureTests
{
    [Fact]
    public async Task CreateDirectory_WithName_CreatesSubdirectory()
    {
        // Arrange
        var fixture = new TemporaryDirectoryFixture();
        await ((IAsyncLifetime)fixture).InitializeAsync();

        // Act
        var subdirectory = fixture.CreateDirectory("sub");

        // Assert
        try
        {
            Assert.True(Directory.Exists(subdirectory.FullPath));
        }
        finally
        {
            await fixture.DisposeAsync();
        }
    }

    [Fact]
    public async Task CreateFile_WithFileName_ReturnsWritableStream()
    {
        // Arrange
        var fixture = new TemporaryDirectoryFixture();
        await ((IAsyncLifetime)fixture).InitializeAsync();

        // Act
        await using var stream = fixture.CreateFile("file.txt");

        // Assert
        try
        {
            Assert.True(stream.CanWrite);
        }
        finally
        {
            await fixture.DisposeAsync();
        }
    }

    [Fact]
    public async Task GetFilePath_WithFileName_ReturnsCombinedPath()
    {
        // Arrange
        var fixture = new TemporaryDirectoryFixture();
        await ((IAsyncLifetime)fixture).InitializeAsync();

        // Act
        var path = fixture.GetFilePath("file.txt");

        // Assert
        try
        {
            Assert.Equal(Path.Combine(fixture.FullPath, "file.txt"), path);
        }
        finally
        {
            await fixture.DisposeAsync();
        }
    }

    [Fact]
    public async Task DisposeAsync_RemovesDirectory()
    {
        // Arrange
        var fixture = new TemporaryDirectoryFixture();
        await ((IAsyncLifetime)fixture).InitializeAsync();
        var path = fixture.FullPath;

        // Act
        await fixture.DisposeAsync();

        // Assert
        Assert.False(Directory.Exists(path));
    }
}
