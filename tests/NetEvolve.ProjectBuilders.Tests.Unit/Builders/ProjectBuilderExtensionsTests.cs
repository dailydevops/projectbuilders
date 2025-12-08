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
        var fileName = "Program";
        var content = "public class Program { }";

        // Act
        builder.AddCSharpFile(fileName, content);

        // Assert
        var filePath = System.IO.Path.Combine(directory.FullPath, "Program.cs");
        _ = await Assert.That(System.IO.File.Exists(filePath)).IsTrue();
    }

    [Test]
    public async Task AddCSharpFile_WithFileExtension_RemovesAndReAddsIt()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await directory.CreateAsync();
        await using var builder = new ProjectBuilder(directory, Constants.CSharpProjectFileName);
        var fileName = "Program.cs";
        var content = "public class Program { }";

        // Act
        builder.AddCSharpFile(fileName, content);

        // Assert
        var filePath = System.IO.Path.Combine(directory.FullPath, "Program.cs");
        _ = await Assert.That(System.IO.File.Exists(filePath)).IsTrue();
    }

    [Test]
    public async Task AddCSharpFile_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        IProjectBuilder? builder = null!;
        var fileName = "Program";
        var content = "public class Program { }";

        // Act
        void Act() => ProjectBuilderExtensions.AddCSharpFile(builder, fileName, content);

        // Assert
        _ = Assert.Throws<ArgumentNullException>(Act);
    }

    [Test]
    public async Task AddCSharpFile_WithNullFileName_ThrowsArgumentNullException()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await using var builder = new ProjectBuilder(directory, Constants.CSharpProjectFileName);
        string? fileName = null;
        var content = "public class Program { }";

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
        var content = "public class Program { }";

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
        var fileName = "Program";
        string? content = null;

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
        var fileName = "Program";
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
        var fileName = "Program";
        var content = "Public Class Program\nEnd Class";

        // Act
        builder.AddVBFile(fileName, content);

        // Assert
        var filePath = System.IO.Path.Combine(directory.FullPath, "Program.vb");
        _ = await Assert.That(System.IO.File.Exists(filePath)).IsTrue();
    }

    [Test]
    public async Task AddVBFile_WithFileExtension_RemovesAndReAddsIt()
    {
        // Arrange
        await using var directory = new TemporaryDirectoryBuilder();
        await directory.CreateAsync();
        await using var builder = new ProjectBuilder(directory, Constants.VBNetProjectFileName);
        var fileName = "Program.vb";
        var content = "Public Class Program\nEnd Class";

        // Act
        builder.AddVBFile(fileName, content);

        // Assert
        var filePath = System.IO.Path.Combine(directory.FullPath, "Program.vb");
        _ = await Assert.That(System.IO.File.Exists(filePath)).IsTrue();
    }

    [Test]
    public async Task AddVBFile_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        IProjectBuilder? builder = null!;
        var fileName = "Program";
        var content = "Public Class Program\nEnd Class";

        // Act
        void Act() => ProjectBuilderExtensions.AddVBFile(builder, fileName, content);

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
        var fileName = "Program";
        var content = "namespace Test { public class Program { } }";

        // Act
        builder.AddCSharpFile(fileName, content);
        var filePath = System.IO.Path.Combine(directory.FullPath, "Program.cs");
        var fileContent = await System.IO.File.ReadAllTextAsync(filePath);

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
        var fileName = "Program";
        var content = "Namespace Test\nPublic Class Program\nEnd Class\nEnd Namespace";

        // Act
        builder.AddVBFile(fileName, content);
        var filePath = System.IO.Path.Combine(directory.FullPath, "Program.vb");
        var fileContent = await System.IO.File.ReadAllTextAsync(filePath);

        // Assert
        _ = await Assert.That(fileContent).IsEqualTo(content);
    }
}
