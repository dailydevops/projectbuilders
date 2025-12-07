namespace NetEvolve.ProjectBuilders.Tests.Integration.Builders;

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Builders;
using NetEvolve.ProjectBuilders.Models;
using NetEvolve.ProjectBuilders.TUnit;

[ClassDataSource<TemporaryDirectory>]
public class ProjectBuilderTests(TemporaryDirectory directory)
{
    [Test]
    [MatrixDataSource]
    public async ValueTask CreateAsync_TargetFrameworkTheory_Expected(
        [Matrix(null, "Microsoft.NET.Sdk", "Microsoft.NET.Sdk.Web")] string? sdk,
        NullableOptions nullable,
        [MatrixInstanceMethod<ProjectBuilderTests>(nameof(GetTargetFrameworkValues))] TargetFramework targetFramework
    )
    {
        var subdirectory = directory.CreateDirectory($"{nameof(CreateAsync_TargetFrameworkTheory_Expected)}");
        await using var builder = new ProjectBuilder(subdirectory, Constants.CSharpProjectFileName);

        await builder.WithNullable(nullable).WithTargetFramework(targetFramework).SetProjectSdk(sdk).CreateAsync();

        _ = await VerifyFile(builder.FullPath, extension: "xml")
            .UseParameters(nullable, targetFramework, sdk)
            .HashParameters();
    }

    [Test]
    [MatrixDataSource]
    public async ValueTask CreateAsync_TargetFrameworksTheory_Expected(
        [Matrix(null, "Microsoft.NET.Sdk", "Microsoft.NET.Sdk.Web")] string? sdk,
        NullableOptions nullable,
        [MatrixInstanceMethod<ProjectBuilderTests>(nameof(GetTargetFrameworkValues))] TargetFramework targetFramework
    )
    {
        var subdirectory = directory.CreateDirectory(
            $"{nameof(CreateAsync_TargetFrameworksTheory_Expected)}{nullable}"
        );
        await using var builder = new ProjectBuilder(subdirectory, Constants.CSharpProjectFileName);

        await builder
            .WithNullable(nullable)
            .WithTargetFrameworks(TargetFramework.NetStandard2_0, targetFramework)
            .SetProjectSdk(sdk)
            .CreateAsync();

        _ = await VerifyFile(builder.FullPath, extension: "xml")
            .UseParameters(nullable, targetFramework, sdk)
            .HashParameters();
    }

    [SuppressMessage("Design", "CA1024:Use properties where appropriate", Justification = "Required by TUnit.")]
    public static IEnumerable<TargetFramework> GetTargetFrameworkValues() =>
        [TargetFramework.NetStandard2_0, TargetFramework.Net10Windows, TargetFramework.NetFramework4_8_1];
}
