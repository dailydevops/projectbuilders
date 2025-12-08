namespace NetEvolve.ProjectBuilders.Tests.Unit;

using System;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders;
using NetEvolve.ProjectBuilders.Builders;

public class ProjectFactoryTests
{
    [Test]
    public async Task BuildAsync_NoFileBuilder_ThrowsArgumentException()
    {
        var ex = await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await using var builder = ProjectFactory.Create();

            _ = await builder.BuildAsync();
        });

        using (Assert.Multiple())
        {
            _ = await Assert.That(ex).IsNotNull();
            _ = await Assert.That(ex.Message).IsNotNullOrWhiteSpace().And.EqualTo("No file builders were added.");
        }
    }

    [Test]
    public async Task BuildAsync_NoProjectBuilder_ThrowsArgumentException()
    {
        var ex = await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await using var builder = ProjectFactory.Create();

            _ = await builder.AddGlobalJson("8.0.204").BuildAsync();
        });

        using (Assert.Multiple())
        {
            _ = await Assert.That(ex).IsNotNull();
            _ = await Assert.That(ex.Message).IsNotNullOrWhiteSpace().And.EqualTo("No project builder were added.");
        }
    }
}
