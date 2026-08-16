namespace NetEvolve.ProjectBuilders.Tests.Unit.Models;

using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Models;

public class PackageReferenceItemTests
{
    [Test]
    public async Task Name_ReturnsPackageReference()
    {
        // Arrange
        var item = new PackageReferenceItem();

        // Act & Assert
        _ = await Assert.That(item.Name).IsEqualTo("PackageReference");
    }

    [Test]
    public async Task GetXElement_WithVersion_SetsVersionAttribute()
    {
        // Arrange
        var item = new PackageReferenceItem
        {
            Include = "Some.Package",
            Version = "1.2.3",
            Aliases = "SomePackageAlias",
        };

        // Act
        var element = item.GetXElement();

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(element.Name.LocalName).IsEqualTo("PackageReference");
            _ = await Assert.That(element.Attribute("Include")?.Value).IsEqualTo("Some.Package");
            _ = await Assert.That(element.Attribute("Version")?.Value).IsEqualTo("1.2.3");
            _ = await Assert.That(element.Attribute("VersionOverride")).IsNull();
            _ = await Assert.That(element.Attribute("Aliases")?.Value).IsEqualTo("SomePackageAlias");
        }
    }

    [Test]
    public async Task VersionOverride_WhenSet_ClearsVersion()
    {
        // Arrange & Act
        var item = new PackageReferenceItem { Version = "1.0.0", VersionOverride = "2.0.0" };

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(item.Version).IsNull();
            _ = await Assert.That(item.VersionOverride).IsEqualTo("2.0.0");
        }
    }

    [Test]
    public async Task VersionOverride_SetToNull_DoesNotTouchVersion()
    {
        // Arrange & Act
        var item = new PackageReferenceItem { Version = "1.0.0", VersionOverride = null };

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(item.Version).IsEqualTo("1.0.0");
            _ = await Assert.That(item.VersionOverride).IsNull();
        }
    }

    [Test]
    public async Task GetXElement_WithVersionOverride_SetsVersionOverrideAttribute()
    {
        // Arrange
        var item = new PackageReferenceItem { Include = "Some.Package", VersionOverride = "2.0.0" };

        // Act
        var element = item.GetXElement();

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(element.Attribute("Version")).IsNull();
            _ = await Assert.That(element.Attribute("VersionOverride")?.Value).IsEqualTo("2.0.0");
        }
    }
}
