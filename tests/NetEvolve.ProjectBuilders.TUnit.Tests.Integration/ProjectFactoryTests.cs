namespace NetEvolve.ProjectBuilders.Tests.Integration;

using System.Threading.Tasks;
using NetEvolve.Extensions.TUnit.Logging;
using NetEvolve.ProjectBuilders.Builders;
using NetEvolve.ProjectBuilders.Models;
using NetEvolve.ProjectBuilders.TUnit;

[ClassDataSource<TemporaryDirectory>]
public class ProjectFactoryTests(TemporaryDirectory directory)
{
    [Test]
    public async Task BuildAsync_CSharpProject_Expected()
    {
        var logger = TestContext.Current!.GetDefaultLogger();
        var subdirectory = directory.CreateDirectory(nameof(BuildAsync_CSharpProject_Expected));
        await using var factory = ProjectFactory.Create(
            directory: subdirectory,
            logger: logger.ConvertTo<ProjectFactory>()
        );

        var result = await factory
            .AddCSharpProject(projectBuilder => projectBuilder.WithTargetFramework(TargetFramework.Net8))
            .AddGlobalJson(
                Constants.RuntimeSdkDefault,
                jsonBuilder => jsonBuilder.SetRollForward(RollForward.LatestMinor)
            )
            .BuildAsync();

        _ = await Assert.That(result.HasNoErrorsOrWarnings()).IsTrue();
    }

    [Test]
    public async Task BuildAsync_VBProject_Expected()
    {
        var logger = TestContext.Current!.GetDefaultLogger();
        var subdirectory = directory.CreateDirectory(nameof(BuildAsync_VBProject_Expected));
        await using var factory = ProjectFactory.Create(
            directory: subdirectory,
            logger: logger.ConvertTo<ProjectFactory>()
        );

        var result = await factory
            .AddVBProject(projectBuilder => projectBuilder.WithTargetFramework(TargetFramework.Net8))
            .AddGlobalJson(
                Constants.RuntimeSdkDefault,
                jsonBuilder => jsonBuilder.SetRollForward(RollForward.LatestMinor)
            )
            .BuildAsync();

        _ = await Assert.That(result.HasNoErrorsOrWarnings()).IsTrue();
    }
}
