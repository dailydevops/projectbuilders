namespace NetEvolve.ProjectBuilders.Tests.Unit.Models;

using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Abstractions;
using NetEvolve.ProjectBuilders.Models;

public class PropertyGroupItemTests
{
    [Test]
    public async Task Constructor_SetsNameAndValues()
    {
        // Arrange & Act
        var item = new PropertyGroupItem("OutputPath", "bin/Debug");

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(item.Name).IsEqualTo("OutputPath");
            _ = await Assert.That(item.Values.ToString()).IsEqualTo("bin/Debug");
        }
    }

    [Test]
    public async Task Constructor_WithNullValue_SetsEmptyValues()
    {
        // Arrange & Act
        var item = new PropertyGroupItem("Configuration", null);

        // Assert
        _ = await Assert.That(((IPropertyGroupItem)item).IsNullOrEmpty).IsTrue();
    }
}
