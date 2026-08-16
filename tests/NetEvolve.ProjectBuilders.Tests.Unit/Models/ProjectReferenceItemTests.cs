namespace NetEvolve.ProjectBuilders.Tests.Unit.Models;

using System;
using System.IO;
using System.Linq;
using System.Reflection;
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
        var item = new ProjectReferenceItem();
        SetInclude(item, include);

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
        var item = new ProjectReferenceItem();
        SetInclude(item, "Foo.csproj");

        // Act
        var element = item.GetXElement();

        // Assert
        _ = await Assert.That(element.Name.LocalName).IsEqualTo("ProjectReference");
    }

    [Test]
    public async Task GetXElement_WithAssetFiltering_SerializesAssetElements()
    {
        // Arrange - mirrors real usage: a project reference that shouldn't flow its build
        // output/analyzers to consuming projects, while still generating a path property for it.
        var item = new ProjectReferenceItem
        {
            GeneratePathProperty = true,
            IncludeAssets = ReferenceAssets.Compile | ReferenceAssets.Runtime,
            ExcludeAssets = ReferenceAssets.Analyzers,
            PrivateAssets = ReferenceAssets.All,
        };
        SetInclude(item, "Foo.csproj");

        // Act
        var element = item.GetXElement();

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(element.Attribute("GeneratePathProperty")?.Value).IsEqualTo("true");
            _ = await Assert.That(element.Element("IncludeAssets")?.Value).IsEqualTo("compile;runtime");
            _ = await Assert.That(element.Element("ExcludeAssets")?.Value).IsEqualTo("analyzers");
            _ = await Assert.That(element.Element("PrivateAssets")?.Value).IsEqualTo("all");
        }
    }

    // `Include` is a get-only auto-property (no public setter), so the backing field is set via
    // reflection to exercise the real LookUpPaths/FullPath implementation with a concrete value.
    private static void SetInclude(ProjectReferenceItem item, string include)
    {
        var field = typeof(ProjectReferenceItem).GetField(
            "<Include>k__BackingField",
            BindingFlags.Instance | BindingFlags.NonPublic
        );
        _ = field ?? throw new InvalidOperationException("Backing field for Include not found.");
        field.SetValue(item, include);
    }
}
