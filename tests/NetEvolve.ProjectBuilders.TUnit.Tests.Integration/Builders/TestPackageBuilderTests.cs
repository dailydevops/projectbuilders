namespace NetEvolve.ProjectBuilders.TUnit.Tests.Integration.Builders;

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders;
using NetEvolve.ProjectBuilders.Builders;
using NetEvolve.ProjectBuilders.Models;

[ClassDataSource<TemporaryDirectory>]
public class TestPackageBuilderTests(TemporaryDirectory directory)
{
    [Test]
    public async Task CreateAsync_WithRealBuiltProject_PacksNuGetPackage(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // Arrange - `dotnet pack` of an SDK-style project requires it to already be restored and
        // built (NU5026 otherwise), mirroring the real scenario this feature targets: a sibling
        // project that's already been built earlier in the same solution.
        var otherProjectDirectory = directory.CreateDirectory(
            $"{nameof(CreateAsync_WithRealBuiltProject_PacksNuGetPackage)}Other"
        );
        await using var otherFactory = ProjectFactory.Create(directory: otherProjectDirectory);
        var otherBuildResult = await otherFactory
            .AddCSharpProject(builder => builder.WithTargetFramework(TargetFramework.Net8))
            .AddGlobalJson(configure: builder => builder.WithDefaults())
            .BuildAsync(cancellationToken: cancellationToken);
        _ = await Assert.That(otherBuildResult.HasNoErrorsOrWarnings()).IsTrue();

        var otherProjectPath = Path.Combine(otherFactory.DirectoryBuilder.FullPath, Constants.CSharpProjectFileName);

        var outputDirectory = directory.CreateDirectory(
            $"{nameof(CreateAsync_WithRealBuiltProject_PacksNuGetPackage)}Output"
        );
        await using var packageBuilder = new TestPackageBuilder(outputDirectory);
        packageBuilder.SetPackagePaths([otherProjectPath]);

        // Act
        await packageBuilder.CreateAsync(cancellationToken);

        // Assert - the fixed test version (999.999.999) prevents version conflicts between runs.
        var packages = Directory.GetFiles(outputDirectory.FullPath, "*.nupkg");
        using (Assert.Multiple())
        {
            _ = await Assert.That(packages).HasSingleItem();
            _ = await Assert.That(packages[0]).Contains("999.999.999");
        }
    }
}
