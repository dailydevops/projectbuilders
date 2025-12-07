namespace NetEvolve.ProjectBuilders.Tests.Unit.Models;

using System;
using System.Linq;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Models;

public class ItemGroupTests
{
    [Test]
    public async Task Constructor_Initializes()
    {
        // Arrange & Act
        var itemGroup = new ItemGroup();

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(itemGroup).IsNotNull();
            _ = await Assert.That(itemGroup.Items).IsNotNull();
            _ = await Assert.That(itemGroup.Items.Count).IsZero();
        }
    }

    [Test]
    public async Task Add_WithValidType_AddsItemToCollection()
    {
        // Arrange
        var itemGroup = new ItemGroup();

        // Act
        var item = itemGroup.Add<PackageReferenceItem>();

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(item).IsNotNull();
            _ = await Assert.That(itemGroup.Items.Count).IsEqualTo(1);
        }
    }

    [Test]
    public async Task Add_WithMultipleItems_AddsAllToCollection()
    {
        // Arrange
        var itemGroup = new ItemGroup();

        // Act
        var item1 = itemGroup.Add<PackageReferenceItem>();
        var item2 = itemGroup.Add<ProjectReferenceItem>();
        var item3 = itemGroup.Add<FrameworkReferenceItem>();

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(itemGroup.Items.Count).IsEqualTo(3);
            _ = await Assert.That(itemGroup.Items.Contains(item1)).IsTrue();
            _ = await Assert.That(itemGroup.Items.Contains(item2)).IsTrue();
            _ = await Assert.That(itemGroup.Items.Contains(item3)).IsTrue();
        }
    }

    [Test]
    public async Task Add_WithPackageReferenceItem_CreatesNewInstance()
    {
        // Arrange
        var itemGroup = new ItemGroup();

        // Act
        var item = itemGroup.Add<PackageReferenceItem>();

        // Assert
        _ = await Assert.That(item).IsTypeOf<PackageReferenceItem>();
    }

    [Test]
    public async Task Add_WithProjectReferenceItem_CreatesNewInstance()
    {
        // Arrange
        var itemGroup = new ItemGroup();

        // Act
        var item = itemGroup.Add<ProjectReferenceItem>();

        // Assert
        _ = await Assert.That(item).IsTypeOf<ProjectReferenceItem>();
    }

    [Test]
    public async Task Add_WithFrameworkReferenceItem_CreatesNewInstance()
    {
        // Arrange
        var itemGroup = new ItemGroup();

        // Act
        var item = itemGroup.Add<FrameworkReferenceItem>();

        // Assert
        _ = await Assert.That(item).IsTypeOf<FrameworkReferenceItem>();
    }

    [Test]
    public async Task Add_DifferentTypes_CreatesDistinctInstances()
    {
        // Arrange
        var itemGroup = new ItemGroup();

        // Act
        var packageItem = itemGroup.Add<PackageReferenceItem>();
        var projectItem = itemGroup.Add<ProjectReferenceItem>();
        var frameworkItem = itemGroup.Add<FrameworkReferenceItem>();

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(packageItem).IsTypeOf<PackageReferenceItem>();
            _ = await Assert.That(projectItem).IsTypeOf<ProjectReferenceItem>();
            _ = await Assert.That(frameworkItem).IsTypeOf<FrameworkReferenceItem>();
            _ = await Assert.That(itemGroup.Items.Count).IsEqualTo(3);
        }
    }

    [Test]
    public async Task Items_CanBeEnumerated()
    {
        // Arrange
        var itemGroup = new ItemGroup();
        var item1 = itemGroup.Add<PackageReferenceItem>();
        var item2 = itemGroup.Add<ProjectReferenceItem>();

        // Act
        var items = itemGroup.Items.ToList();

        // Assert
        using (Assert.Multiple())
        {
            _ = await Assert.That(items.Count).IsEqualTo(2);
            _ = await Assert.That(items[0] == item1).IsTrue();
            _ = await Assert.That(items[1] == item2).IsTrue();
        }
    }

    [Test]
    public async Task Add_SameTypeMultipleTimes_CreatesMultipleInstances()
    {
        // Arrange
        var itemGroup = new ItemGroup();

        // Act
        var item1 = itemGroup.Add<PackageReferenceItem>();
        var item2 = itemGroup.Add<PackageReferenceItem>();
        var item3 = itemGroup.Add<PackageReferenceItem>();

        // Assert
        _ = await Assert.That(itemGroup.Items.Count).IsEqualTo(3);
    }

    [Test]
    public async Task ItemGroup_ContainsAllAddedItems()
    {
        // Arrange
        var itemGroup = new ItemGroup();
        var items = new PackageReferenceItem[5];

        // Act
        for (var i = 0; i < 5; i++)
        {
            items[i] = itemGroup.Add<PackageReferenceItem>();
        }

        // Assert
        foreach (var item in items)
        {
            _ = await Assert.That(itemGroup.Items.Contains(item)).IsTrue();
        }
    }
}
