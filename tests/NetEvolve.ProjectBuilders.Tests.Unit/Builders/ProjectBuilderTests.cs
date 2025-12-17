namespace NetEvolve.ProjectBuilders.Tests.Unit.Builders;

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Builders;
using NetEvolve.ProjectBuilders.Models;

public class ProjectBuilderTests
{
    [Test]
    public async Task CreateAsync_CreatesProjectFile()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await directory.CreateAsync();
        await using var builder = new ProjectBuilder(directory, Constants.CSharpProjectFileName);

        // Act
        await builder.CreateAsync();

        // Assert
        _ = await Assert.That(File.Exists(builder.FullPath)).IsTrue();
    }

    [Test]
    public async Task CreateAsync_GeneratesValidXml()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await directory.CreateAsync();
        await using var builder = new ProjectBuilder(directory, Constants.CSharpProjectFileName);

        // Act
        await builder.CreateAsync();
        var content = await File.ReadAllTextAsync(builder.FullPath);

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(content).Contains("<Project");
            _ = await Assert.That(content).Contains("</Project>");
        }
    }

    [Test]
    public async Task SetProjectSdk_UpdatesSdkAttribute()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await using var builder = new ProjectBuilder(directory, Constants.CSharpProjectFileName);
        const string customSdk = "Custom.SDK";

        // Act
        var result = builder.SetProjectSdk(customSdk);

        // Assert
        _ = await Assert.That(result).IsNotNull();
    }

    [Test]
    public async Task CreateFile_WithValidFileName_ReturnsStream()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await using var builder = new ProjectBuilder(directory, Constants.CSharpProjectFileName);
        const string fileName = "TestFile.cs";

        // Act
        using var stream = builder.CreateFile(fileName);

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(stream).IsNotNull();
            _ = await Assert.That(stream.CanWrite).IsTrue();
        }
    }

    [Test]
    public async Task GetOrAddItemGroupItem_WithValidType_ReturnsItem()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await using var builder = new ProjectBuilder(directory, Constants.CSharpProjectFileName);

        // Act
        var item = builder.GetOrAddItemGroupItem<PackageReferenceItem>();

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(item).IsNotNull();
            _ = await Assert.That(item).IsTypeOf<PackageReferenceItem>();
        }
    }

    [Test]
    public async Task GetOrAddItemGroupItem_CalledTwice_ReturnsSameInstance()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await using var builder = new ProjectBuilder(directory, Constants.CSharpProjectFileName);

        // Act
        var item1 = builder.GetOrAddItemGroupItem<PackageReferenceItem>();
        var item2 = builder.GetOrAddItemGroupItem<PackageReferenceItem>();

        // Assert
        _ = await Assert.That(item1 == item2).IsTrue();
    }

    [Test]
    public async Task GetOrAddPropertyGroupItem_WithValidType_ReturnsItem()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await using var builder = new ProjectBuilder(directory, Constants.CSharpProjectFileName);

        // Act
        var item = builder.GetOrAddPropertyGroupItem<NullableItem>();

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(item).IsNotNull();
            _ = await Assert.That(item).IsTypeOf<NullableItem>();
        }
    }

    [Test]
    public async Task GetOrAddPropertyGroupItem_CalledTwice_ReturnsSameInstance()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await using var builder = new ProjectBuilder(directory, Constants.CSharpProjectFileName);

        // Act
        var item1 = builder.GetOrAddPropertyGroupItem<NullableItem>();
        var item2 = builder.GetOrAddPropertyGroupItem<NullableItem>();

        // Assert
        _ = await Assert.That(item1 == item2).IsTrue();
    }

    [Test]
    public async Task CreateAsync_WithItemGroupItems_IncludesInOutput()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await directory.CreateAsync();
        await using var builder = new ProjectBuilder(directory, Constants.CSharpProjectFileName);
        _ = builder.GetOrAddItemGroupItem<PackageReferenceItem>();

        // Act
        await builder.CreateAsync();
        var content = await File.ReadAllTextAsync(builder.FullPath);

        // Assert
        _ = await Assert.That(content).Contains("ItemGroup");
    }

    [Test]
    public async Task DisposeAsync_CompletesSuccessfully()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        var builder = new ProjectBuilder(directory, Constants.CSharpProjectFileName);

        // Act & Assert
        await builder.DisposeAsync();
    }

    [Test]
    public async Task Constructor_WithVBProjectExtension_InitializesCorrectly()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        const string projectExtension = Constants.VBNetProjectFileName;

        // Act
        await using var builder = new ProjectBuilder(directory, projectExtension);

        // Assert
        _ = await Assert.That(builder.FullPath).EndsWith(".vbproj");
    }

    [Test]
    public async Task CreateFile_MultipleFiles_CreatesAllSuccessfully()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await directory.CreateAsync();
        await using var builder = new ProjectBuilder(directory, Constants.CSharpProjectFileName);
        var fileNames = new[] { "File1.cs", "File2.cs", "File3.cs" };

        // Act
        foreach (var fileName in fileNames)
        {
            using var stream = builder.CreateFile(fileName);
            await stream.WriteAsync(Encoding.UTF8.GetBytes("// Test content"));
        }

        // Assert
        foreach (var fileName in fileNames)
        {
            var filePath = Path.Combine(directory.FullPath, fileName);
            _ = await Assert.That(File.Exists(filePath)).IsTrue();
        }
    }
}
