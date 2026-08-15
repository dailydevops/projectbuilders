namespace NetEvolve.ProjectBuilders.Tests.Unit.Abstractions;

using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Abstractions;
using NetEvolve.ProjectBuilders.Models;

public class IReferenceTests
{
    [Test]
    public async Task LookUpPaths_DefaultImplementation_ReturnsEmptyCollection()
    {
        // Arrange
        IReference reference = new FrameworkReferenceItem();

        // Act
        var paths = reference.LookUpPaths;

        // Assert
        _ = await Assert.That(paths).IsEmpty();
    }
}
