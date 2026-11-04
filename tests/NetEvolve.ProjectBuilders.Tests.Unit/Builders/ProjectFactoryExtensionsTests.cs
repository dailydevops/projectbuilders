namespace NetEvolve.ProjectBuilders.Tests.Unit.Builders;

using System;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Abstractions;
using NetEvolve.ProjectBuilders.Builders;

public class ProjectFactoryExtensionsTests
{
    [Test]
    [Arguments("")]
    [Arguments(" ")]
    public async Task AddGlobalJson_WithEmptyOrWhitespaceRuntimeVersion_ThrowsArgumentException(string runtimeVersion)
    {
        // Arrange
        await using var factory = ProjectFactory.Create();

        // Act
        void Act() => factory.AddGlobalJson(runtimeVersion);

        // Assert
        _ = Assert.Throws<ArgumentException>(Act);
    }

    [Test]
    public async Task AddGlobalJson_WithConfigure_InvokesCallbackWithGlobalJsonBuilder()
    {
        // Arrange
        await using var factory = ProjectFactory.Create();
        IGlobalJsonBuilder? received = null;
        var calls = 0;

        // Act
        _ = factory.AddGlobalJson(configure: gb =>
        {
            calls++;
            received = gb;
        });

        // Assert
        _ = await Assert.That(calls).IsEqualTo(1);
        _ = await Assert.That(received).IsNotNull().And.IsTypeOf<GlobalJsonBuilder>();
    }
}
