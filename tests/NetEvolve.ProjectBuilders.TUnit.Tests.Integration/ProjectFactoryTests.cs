namespace NetEvolve.ProjectBuilders.TUnit.Tests.Integration;

using System.Threading;
using System.Threading.Tasks;
using NetEvolve.Extensions.TUnit.Logging;
using NetEvolve.ProjectBuilders;
using NetEvolve.ProjectBuilders.Builders;
using NetEvolve.ProjectBuilders.Models;

[ClassDataSource<TemporaryDirectory>]
public class ProjectFactoryTests(TemporaryDirectory directory)
{
    [Test]
    public async Task BuildAsync_CSharpProject_Expected(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

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
            .BuildAsync(cancellationToken: cancellationToken);

        _ = await Assert.That(result.HasNoErrorsOrWarnings()).IsTrue();
    }

    [Test]
    public async Task BuildAsync_WithoutEnvironmentVariables_StillBuildsSuccessfully(
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        // Arrange - clears the environment variables the factory seeds by default (CI,
        // DOTNET_CLI_TELEMETRY_OPTOUT, ...), exercising the "nothing to forward to the dotnet
        // process" branch of ExecuteDotNetCommandAsync during a real build.
        var logger = TestContext.Current!.GetDefaultLogger();
        var subdirectory = directory.CreateDirectory(
            nameof(BuildAsync_WithoutEnvironmentVariables_StillBuildsSuccessfully)
        );
        await using var factory = ProjectFactory.Create(
            directory: subdirectory,
            logger: logger.ConvertTo<ProjectFactory>()
        );
        ((ProjectFactory)factory).EnvironmentVariables.Clear();

        // Act
        var result = await factory
            .AddCSharpProject(projectBuilder => projectBuilder.WithTargetFramework(TargetFramework.Net8))
            .AddGlobalJson(
                Constants.RuntimeSdkDefault,
                jsonBuilder => jsonBuilder.SetRollForward(RollForward.LatestMinor)
            )
            .BuildAsync(cancellationToken: cancellationToken);

        // Assert
        _ = await Assert.That(result.HasNoErrorsOrWarnings()).IsTrue();
    }

    [Test]
    public async Task BuildAsync_VBProject_Expected(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

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
            .BuildAsync(cancellationToken: cancellationToken);

        _ = await Assert.That(result.HasNoErrorsOrWarnings()).IsTrue();
    }
}
