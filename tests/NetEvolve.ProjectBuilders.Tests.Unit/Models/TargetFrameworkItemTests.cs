namespace NetEvolve.ProjectBuilders.Tests.Unit.Models;

using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Models;

public class TargetFrameworkItemTests
{
    [Test]
    public async Task Name_WithSingleValue_ReturnsTargetFramework()
    {
        // Arrange
        var item = new TargetFrameworkItem();
        item.SetValue(TargetFramework.Net8);

        // Act & Assert
        _ = await Assert.That(item.Name).IsEqualTo("TargetFramework");
    }

    [Test]
    public async Task Name_WithMultipleValues_ReturnsTargetFrameworks()
    {
        // Arrange
        var item = new TargetFrameworkItem();
        item.SetValues([TargetFramework.Net8, TargetFramework.Net9]);

        // Act & Assert
        _ = await Assert.That(item.Name).IsEqualTo("TargetFrameworks");
    }

    [Test]
    public async Task SetValue_ReplacesExistingValues()
    {
        // Arrange
        var item = new TargetFrameworkItem();
        item.SetValues([TargetFramework.Net8, TargetFramework.Net9]);

        // Act
        item.SetValue(TargetFramework.Net10);

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(item.Values.Count).IsEqualTo(1);
            _ = await Assert.That(item.Values.ToString()).IsEqualTo(TargetFramework.Net10.Value);
        }
    }

    [Test]
    public async Task SetValues_AppendsToExistingValues()
    {
        // Arrange
        var item = new TargetFrameworkItem();
        item.SetValue(TargetFramework.Net8);

        // Act
        item.SetValues([TargetFramework.Net9]);

        // Assert
        _ = await Assert.That(item.Values.Count).IsEqualTo(2);
    }
}
