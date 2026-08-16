namespace NetEvolve.ProjectBuilders.Tests.Unit.Models;

using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Abstractions;
using NetEvolve.ProjectBuilders.Models;

public class FrameworkReferenceItemTests
{
    [Test]
    public async Task Name_ReturnsFrameworkReference()
    {
        // Arrange
        var item = new FrameworkReferenceItem();

        // Act & Assert
        _ = await Assert.That(item.Name).IsEqualTo("FrameworkReference");
    }

    [Test]
    public async Task LookUpPaths_DefaultImplementation_ReturnsEmpty()
    {
        // Arrange
        IReference item = new FrameworkReferenceItem();

        // Act
        var paths = item.LookUpPaths;

        // Assert
        _ = await Assert.That(paths).IsEmpty();
    }

    [Test]
    public async Task GetXElement_ReturnsElementWithName()
    {
        // Arrange
        var item = new FrameworkReferenceItem { GeneratePathProperty = true };

        // Act
        var element = item.GetXElement();

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(element.Name.LocalName).IsEqualTo("FrameworkReference");
            _ = await Assert.That(element.Attribute("GeneratePathProperty")?.Value).IsEqualTo("true");
        }
    }

    [Test]
    public async Task GetXElement_WithAssetFiltering_SerializesAssetElements()
    {
        // Arrange - mirrors real usage: excluding analyzer/build assets from a framework reference
        // while restricting what's forwarded to consuming projects.
        var item = new FrameworkReferenceItem
        {
            IncludeAssets = ReferenceAssets.Compile | ReferenceAssets.Runtime,
            ExcludeAssets = ReferenceAssets.Analyzers,
            PrivateAssets = ReferenceAssets.All,
        };

        // Act
        var element = item.GetXElement();

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(element.Element("IncludeAssets")?.Value).IsEqualTo("compile;runtime");
            _ = await Assert.That(element.Element("ExcludeAssets")?.Value).IsEqualTo("analyzers");
            _ = await Assert.That(element.Element("PrivateAssets")?.Value).IsEqualTo("all");
        }
    }
}
