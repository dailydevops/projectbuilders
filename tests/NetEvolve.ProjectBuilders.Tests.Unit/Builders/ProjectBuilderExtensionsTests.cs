namespace NetEvolve.ProjectBuilders.Tests.Unit.Builders;

using System;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Abstractions;
using NetEvolve.ProjectBuilders.Builders;

public class ProjectBuilderExtensionsTests
{
    [Test]
    public async Task AddCSharpFile_WithValidParameters_CreatesFile()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await directory.CreateAsync();
        await using var builder = new ProjectBuilder(directory, Constants.CSharpProjectFileName);
        const string fileName = "Program";
        const string content = "public class Program { }";

        // Act
        builder.AddCSharpFile(fileName, content);

        // Assert
        var filePath = Path.Combine(directory.FullPath, "Program.cs");
        _ = await Assert.That(File.Exists(filePath)).IsTrue();
    }

    [Test]
    public async Task AddCSharpFile_WithFileExtension_RemovesAndReAddsIt()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await directory.CreateAsync();
        await using var builder = new ProjectBuilder(directory, Constants.CSharpProjectFileName);
        const string fileName = "Program.cs";
        const string content = "public class Program { }";

        // Act
        builder.AddCSharpFile(fileName, content);

        // Assert
        var filePath = Path.Combine(directory.FullPath, "Program.cs");
        _ = await Assert.That(File.Exists(filePath)).IsTrue();
    }

    [Test]
    public async Task AddCSharpFile_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        IProjectBuilder builder = null!;
        const string fileName = "Program";
        const string content = "public class Program { }";

        // Act
        void Act() => builder.AddCSharpFile(fileName, content);

        // Assert
        _ = Assert.Throws<ArgumentNullException>(Act);
    }

    [Test]
    public async Task AddCSharpFile_WithNullFileName_ThrowsArgumentNullException()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await using var builder = new ProjectBuilder(directory, Constants.CSharpProjectFileName);
        const string? fileName = null;
        const string content = "public class Program { }";

        // Act
        void Act() => builder.AddCSharpFile(fileName!, content);

        // Assert
        _ = Assert.Throws<ArgumentNullException>(Act);
    }

    [Test]
    public async Task AddCSharpFile_WithEmptyFileName_ThrowsArgumentException()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await using var builder = new ProjectBuilder(directory, Constants.CSharpProjectFileName);
        var fileName = string.Empty;
        const string content = "public class Program { }";

        // Act
        void Act() => builder.AddCSharpFile(fileName, content);

        // Assert
        _ = Assert.Throws<ArgumentException>(Act);
    }

    [Test]
    public async Task AddCSharpFile_WithNullContent_ThrowsArgumentNullException()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await using var builder = new ProjectBuilder(directory, Constants.CSharpProjectFileName);
        const string fileName = "Program";
        const string? content = null;

        // Act
        void Act() => builder.AddCSharpFile(fileName, content!);

        // Assert
        _ = Assert.Throws<ArgumentNullException>(Act);
    }

    [Test]
    public async Task AddCSharpFile_WithEmptyContent_ThrowsArgumentException()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await using var builder = new ProjectBuilder(directory, Constants.CSharpProjectFileName);
        const string fileName = "Program";
        var content = string.Empty;

        // Act
        void Act() => builder.AddCSharpFile(fileName, content);

        // Assert
        _ = Assert.Throws<ArgumentException>(Act);
    }

    [Test]
    public async Task AddVBFile_WithValidParameters_CreatesFile()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await directory.CreateAsync();
        await using var builder = new ProjectBuilder(directory, Constants.VBNetProjectFileName);
        const string fileName = "Program";
        const string content = "Public Class Program\nEnd Class";

        // Act
        builder.AddVBFile(fileName, content);

        // Assert
        var filePath = Path.Combine(directory.FullPath, "Program.vb");
        _ = await Assert.That(File.Exists(filePath)).IsTrue();
    }

    [Test]
    public async Task AddVBFile_WithFileExtension_RemovesAndReAddsIt()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await directory.CreateAsync();
        await using var builder = new ProjectBuilder(directory, Constants.VBNetProjectFileName);
        const string fileName = "Program.vb";
        const string content = "Public Class Program\nEnd Class";

        // Act
        builder.AddVBFile(fileName, content);

        // Assert
        var filePath = Path.Combine(directory.FullPath, "Program.vb");
        _ = await Assert.That(File.Exists(filePath)).IsTrue();
    }

    [Test]
    public async Task AddVBFile_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        IProjectBuilder builder = null!;
        const string fileName = "Program";
        const string content = "Public Class Program\nEnd Class";

        // Act
        void Act() => builder.AddVBFile(fileName, content);

        // Assert
        _ = Assert.Throws<ArgumentNullException>(Act);
    }

    [Test]
    public async Task AddCSharpFile_WritesContentToFile()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await directory.CreateAsync();
        await using var builder = new ProjectBuilder(directory, Constants.CSharpProjectFileName);
        const string fileName = "Program";
        const string content = "namespace Test { public class Program { } }";

        // Act
        builder.AddCSharpFile(fileName, content);
        var filePath = Path.Combine(directory.FullPath, "Program.cs");
        var fileContent = await File.ReadAllTextAsync(filePath);

        // Assert
        _ = await Assert.That(fileContent).IsEqualTo(content);
    }

    [Test]
    public async Task AddVBFile_WritesContentToFile()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await directory.CreateAsync();
        await using var builder = new ProjectBuilder(directory, Constants.VBNetProjectFileName);
        const string fileName = "Program";
        const string content = "Namespace Test\nPublic Class Program\nEnd Class\nEnd Namespace";

        // Act
        builder.AddVBFile(fileName, content);
        var filePath = Path.Combine(directory.FullPath, "Program.vb");
        var fileContent = await File.ReadAllTextAsync(filePath);

        // Assert
        _ = await Assert.That(fileContent).IsEqualTo(content);
    }
}
