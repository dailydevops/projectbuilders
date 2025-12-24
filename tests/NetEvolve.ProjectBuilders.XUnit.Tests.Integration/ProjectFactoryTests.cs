namespace NetEvolve.ProjectBuilders.XUnit.Tests.Integration;

using System.Threading.Tasks;
using NetEvolve.ProjectBuilders;
using NetEvolve.ProjectBuilders.Builders;
using NetEvolve.ProjectBuilders.Models;

public class ProjectFactoryTests(TemporaryDirectoryFixture directory) : IClassFixture<TemporaryDirectoryFixture>
{
    [Fact]
    public async Task BuildAsync_CSharpProject_Expected()
    {
        var subdirectory = directory.CreateDirectory(nameof(BuildAsync_CSharpProject_Expected));
        await using var factory = ProjectFactory.Create(directory: subdirectory);

        var result = await factory
            .AddCSharpProject(projectBuilder => projectBuilder.WithTargetFramework(TargetFramework.Net8))
            .AddGlobalJson(
                Constants.RuntimeSdkDefault,
                jsonBuilder => jsonBuilder.SetRollForward(RollForward.LatestMinor)
            )
            .BuildAsync(cancellationToken: TestContext.Current.CancellationToken);

        Assert.True(result.HasNoErrorsOrWarnings());
    }

    [Fact]
    public async Task BuildAsync_VBProject_Expected()
    {
        var subdirectory = directory.CreateDirectory(nameof(BuildAsync_VBProject_Expected));
        await using var factory = ProjectFactory.Create(directory: subdirectory);

        var result = await factory
            .AddVBProject(projectBuilder => projectBuilder.WithTargetFramework(TargetFramework.Net8))
            .AddGlobalJson(
                Constants.RuntimeSdkDefault,
                jsonBuilder => jsonBuilder.SetRollForward(RollForward.LatestMinor)
            )
            .BuildAsync(cancellationToken: TestContext.Current.CancellationToken);

        Assert.True(result.HasNoErrorsOrWarnings());
    }
}
