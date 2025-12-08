namespace NetEvolve.ProjectBuilders.Tests.Unit.Models;

using System;
using System.Linq;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Models;

public class PropertyGroupTests
{
    [Test]
    public async Task Constructor_Initializes()
    {
        // Arrange & Act
        var propertyGroup = new PropertyGroup();

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(propertyGroup).IsNotNull();
            _ = await Assert.That(propertyGroup.Items).IsNotNull();
            _ = await Assert.That(propertyGroup.Items.Count).IsZero();
        }
    }

    [Test]
    public async Task Add_WithValidType_AddsItemToCollection()
    {
        // Arrange
        var propertyGroup = new PropertyGroup();

        // Act
        var item = propertyGroup.Add<NullableItem>();

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(item).IsNotNull();
            _ = await Assert.That(propertyGroup.Items.Count).IsEqualTo(1);
        }
    }

    [Test]
    public async Task Add_WithMultipleItems_AddsAllToCollection()
    {
        // Arrange
        var propertyGroup = new PropertyGroup();

        // Act
        var item1 = propertyGroup.Add<NullableItem>();
        var item2 = propertyGroup.Add<NullableItem>();
        var item3 = propertyGroup.Add<NullableItem>();

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(propertyGroup.Items.Count).IsEqualTo(3);
            _ = await Assert.That(propertyGroup.Items.Contains(item1)).IsTrue();
            _ = await Assert.That(propertyGroup.Items.Contains(item2)).IsTrue();
            _ = await Assert.That(propertyGroup.Items.Contains(item3)).IsTrue();
        }
    }

    [Test]
    public async Task Add_WithNullableItem_CreatesNewInstance()
    {
        // Arrange
        var propertyGroup = new PropertyGroup();

        // Act
        var item = propertyGroup.Add<NullableItem>();

        // Assert
        _ = await Assert.That(item).IsTypeOf<NullableItem>();
    }

    [Test]
    public async Task Add_SameTypeMultipleTimes_CreatesMultipleInstances()
    {
        // Arrange
        var propertyGroup = new PropertyGroup();

        // Act
        _ = propertyGroup.Add<NullableItem>();
        _ = propertyGroup.Add<NullableItem>();
        _ = propertyGroup.Add<NullableItem>();

        // Assert
        _ = await Assert.That(propertyGroup.Items.Count).IsEqualTo(3);
    }

    [Test]
    public async Task Items_CanBeEnumerated()
    {
        // Arrange
        var propertyGroup = new PropertyGroup();
        var item1 = propertyGroup.Add<NullableItem>();
        var item2 = propertyGroup.Add<NullableItem>();

        // Act
        var items = propertyGroup.Items.ToList();

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(items.Count).IsEqualTo(2);
            _ = await Assert.That(items).Contains(item1).And.Contains(item2);
        }
    }
}
