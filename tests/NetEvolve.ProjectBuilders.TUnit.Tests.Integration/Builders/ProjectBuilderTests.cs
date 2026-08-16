namespace NetEvolve.ProjectBuilders.TUnit.Tests.Integration.Builders;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders;
using NetEvolve.ProjectBuilders.Builders;
using NetEvolve.ProjectBuilders.Models;

[ClassDataSource<TemporaryDirectory>]
public class ProjectBuilderTests(TemporaryDirectory directory)
{
    [Test]
    [MatrixDataSource]
    public async ValueTask CreateAsync_TargetFrameworkTheory_Expected(
        [Matrix(null, "Microsoft.NET.Sdk", "Microsoft.NET.Sdk.Web")] string? sdk,
        NullableOptions nullable,
        [MatrixInstanceMethod<ProjectBuilderTests>(nameof(GetTargetFrameworkValues))] TargetFramework targetFramework,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        var subdirectory = directory.CreateDirectory(nameof(CreateAsync_TargetFrameworkTheory_Expected));
        await using var builder = new ProjectBuilder(subdirectory, Constants.CSharpProjectFileName);

        await builder
            .WithNullable(nullable)
            .WithTargetFramework(targetFramework)
            .SetProjectSdk(sdk)
            .CreateAsync(cancellationToken: cancellationToken);

        _ = await VerifyFile(builder.FullPath, extension: "xml")
            .UseParameters(nullable, targetFramework, sdk)
            .HashParameters();
    }

    [Test]
    [MatrixDataSource]
    public async ValueTask CreateAsync_TargetFrameworksTheory_Expected(
        [Matrix(null, "Microsoft.NET.Sdk", "Microsoft.NET.Sdk.Web")] string? sdk,
        NullableOptions nullable,
        [MatrixInstanceMethod<ProjectBuilderTests>(nameof(GetTargetFrameworkValues))] TargetFramework targetFramework,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        var subdirectory = directory.CreateDirectory(
            $"{nameof(CreateAsync_TargetFrameworksTheory_Expected)}{nullable}"
        );
        await using var builder = new ProjectBuilder(subdirectory, Constants.CSharpProjectFileName);

        await builder
            .WithNullable(nullable)
            .WithTargetFrameworks(TargetFramework.NetStandard2_0, targetFramework)
            .SetProjectSdk(sdk)
            .CreateAsync(cancellationToken: cancellationToken);

        _ = await VerifyFile(builder.FullPath, extension: "xml")
            .UseParameters(nullable, targetFramework, sdk)
            .HashParameters();
    }

    [SuppressMessage("Design", "CA1024:Use properties where appropriate", Justification = "Required by TUnit.")]
    public static IEnumerable<TargetFramework> GetTargetFrameworkValues() =>
        [TargetFramework.NetStandard2_0, TargetFramework.Net10Windows, TargetFramework.NetFramework4_8_1];

    [Test]
    [MatrixDataSource]
    public async ValueTask AddPackageReference_Newtonsoft_Expected(
        [Matrix("13.0.1", null)] string? version,
        [Matrix("13.0.1", null)] string? versionOverride,
        bool generatePathProperty,
        [Matrix("NJson", "")] string? aliases,
        [Matrix(ReferenceAssets.All, null)] ReferenceAssets? includeAssets,
        [Matrix(ReferenceAssets.None, ReferenceAssets.Runtime)] ReferenceAssets? excludeAssets,
        [Matrix(ReferenceAssets.Build | ReferenceAssets.ContentFiles, null)] ReferenceAssets? privateAssets,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        var subdirectory = directory.CreateDirectory(nameof(AddPackageReference_Newtonsoft_Expected));
        await using var builder = new ProjectBuilder(subdirectory, Constants.CSharpProjectFileName);
        await builder
            .AddPackageReference(
                "Newtonsoft.Json",
                version,
                versionOverride,
                generatePathProperty,
                aliases,
                includeAssets,
                excludeAssets,
                privateAssets
            )
            .CreateAsync(cancellationToken: cancellationToken);

        _ = await VerifyFile(builder.FullPath, extension: "xml")
            .UseParameters(
                version,
                versionOverride,
                generatePathProperty,
                aliases,
                includeAssets,
                excludeAssets,
                privateAssets
            )
            .HashParameters();
    }

    [Test]
    [MatrixDataSource]
    public async ValueTask AddProjectReference_OtherProject_Expected(
        bool generatePathProperty,
        [Matrix("NOther", "")] string? aliases,
        [Matrix(ReferenceAssets.All, null)] ReferenceAssets? includeAssets,
        [Matrix(ReferenceAssets.None, ReferenceAssets.Runtime)] ReferenceAssets? excludeAssets,
        [Matrix(ReferenceAssets.Build | ReferenceAssets.ContentFiles, null)] ReferenceAssets? privateAssets,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        var subdirectory = directory.CreateDirectory(nameof(AddProjectReference_OtherProject_Expected));
        await using var builder = new ProjectBuilder(subdirectory, Constants.CSharpProjectFileName);
        await builder
            .AddProjectReference(
                "../OtherProject/OtherProject.csproj",
                generatePathProperty,
                aliases,
                includeAssets,
                excludeAssets,
                privateAssets
            )
            .CreateAsync(cancellationToken: cancellationToken);

        _ = await VerifyFile(builder.FullPath, extension: "xml")
            .UseParameters(generatePathProperty, aliases, includeAssets, excludeAssets, privateAssets)
            .HashParameters();
    }
}
