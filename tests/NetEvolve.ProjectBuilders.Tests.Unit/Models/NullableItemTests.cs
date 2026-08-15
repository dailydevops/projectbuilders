namespace NetEvolve.ProjectBuilders.Tests.Unit.Models;

using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Abstractions;
using NetEvolve.ProjectBuilders.Models;

public class NullableItemTests
{
    [Test]
    public async Task Name_ReturnsNullable()
    {
        // Arrange
        var item = new NullableItem();

        // Act & Assert
        _ = await Assert.That(item.Name).IsEqualTo("Nullable");
    }

    [Test]
    [Arguments(NullableOptions.Enable, "enable")]
    [Arguments(NullableOptions.Disable, "disable")]
    [Arguments(NullableOptions.Warnings, "warnings")]
    [Arguments(NullableOptions.Annotations, "annotations")]
    public async Task SetValue_WithKnownOption_SetsExpectedValue(NullableOptions option, string expected)
    {
        // Arrange
        var item = new NullableItem();

        // Act
        item.SetValue(option);

        // Assert
        _ = await Assert.That(item.Values.ToString()).IsEqualTo(expected);
    }

    [Test]
    public async Task SetValue_WithUnknownOption_SetsNullValue()
    {
        // Arrange
        var item = new NullableItem();

        // Act
        item.SetValue((NullableOptions)999);

        // Assert
        _ = await Assert.That(((IPropertyGroupItem)item).IsNullOrEmpty).IsTrue();
    }

    [Test]
    public async Task SetValues_DoesNotThrow()
    {
        // Arrange
        var item = new NullableItem();

        // Act
        item.SetValues([NullableOptions.Enable]);

        // Assert
        _ = await Assert.That(((IPropertyGroupItem)item).IsNullOrEmpty).IsTrue();
    }
}
