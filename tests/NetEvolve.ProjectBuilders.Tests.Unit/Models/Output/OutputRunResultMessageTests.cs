namespace NetEvolve.ProjectBuilders.Tests.Unit.Models.Output;

using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Models.Output;

public class OutputRunResultMessageTests
{
    [Test]
    public async Task ToString_WithText_ReturnsText()
    {
        // Arrange
        var message = new OutputRunResultMessage { Text = "Something went wrong." };

        // Act & Assert
        _ = await Assert.That(message.ToString()).IsEqualTo("Something went wrong.");
    }

    [Test]
    public async Task ToString_WithNullText_ReturnsEmptyString()
    {
        // Arrange
        var message = new OutputRunResultMessage { Text = null };

        // Act & Assert
        _ = await Assert.That(message.ToString()).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task ToString_WithWhitespaceText_ReturnsEmptyString()
    {
        // Arrange
        var message = new OutputRunResultMessage { Text = "   " };

        // Act & Assert
        _ = await Assert.That(message.ToString()).IsEqualTo(string.Empty);
    }
}
