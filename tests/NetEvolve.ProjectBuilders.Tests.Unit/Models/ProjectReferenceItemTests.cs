namespace NetEvolve.ProjectBuilders.Tests.Unit.Models;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Models;

public class ProjectReferenceItemTests
{
    [Test]
    public async Task Name_ReturnsProjectReference()
    {
        // Arrange
        var item = new ProjectReferenceItem();

        // Act & Assert
        _ = await Assert.That(item.Name).IsEqualTo("ProjectReference");
    }

    [Test]
    public void LookUpPaths_WithoutInclude_ThrowsArgumentNullException()
    {
        // Arrange
        var item = new ProjectReferenceItem();

        // Act
        void Act() => _ = item.LookUpPaths.ToList();

        // Assert
        _ = Assert.Throws<ArgumentNullException>(Act);
    }

    [Test]
    public async Task LookUpPaths_WithInclude_ReturnsNuspecAndProjectPath()
    {
        // Arrange
        const string include = "../OtherProject/OtherProject.csproj";
        var item = new ProjectReferenceItem { Include = include };

        // Act
        var paths = item.LookUpPaths.ToList();

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(paths.Count).IsEqualTo(2);
            _ = await Assert.That(paths[0]).IsEqualTo(Path.ChangeExtension(Path.GetFullPath(include), ".nuspec"));
            _ = await Assert.That(paths[1]).IsEqualTo(Path.GetFullPath(include));
        }
    }

    [Test]
    public async Task GetXElement_ReturnsElementWithName()
    {
        // Arrange
        var item = new ProjectReferenceItem { Include = "Foo.csproj" };

        // Act
        var element = item.GetXElement();

        // Assert
        _ = await Assert.That(element.Name.LocalName).IsEqualTo("ProjectReference");
    }

    [Test]
    public async Task GetXElement_WithAssetFiltering_SerializesAssetElements()
    {
        // Arrange - mirrors real usage: a project reference that shouldn't flow its build
        // output/analyzers to consuming projects, while still generating a path property for it,
        // and referencing it under an alias to avoid a type-name collision.
        var item = new ProjectReferenceItem
        {
            Include = "Foo.csproj",
            GeneratePathProperty = true,
            Aliases = "FooLib",
            IncludeAssets = ReferenceAssets.Compile | ReferenceAssets.Runtime,
            ExcludeAssets = ReferenceAssets.Analyzers,
            PrivateAssets = ReferenceAssets.All,
        };

        // Act
        var element = item.GetXElement();

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(element.Attribute("GeneratePathProperty")?.Value).IsEqualTo("true");
            _ = await Assert.That(element.Attribute("Aliases")?.Value).IsEqualTo("FooLib");
            _ = await Assert.That(element.Element("IncludeAssets")?.Value).IsEqualTo("compile;runtime");
            _ = await Assert.That(element.Element("ExcludeAssets")?.Value).IsEqualTo("analyzers");
            _ = await Assert.That(element.Element("PrivateAssets")?.Value).IsEqualTo("all");
        }
    }
}
