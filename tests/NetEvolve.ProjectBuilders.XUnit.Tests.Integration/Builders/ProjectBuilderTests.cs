namespace NetEvolve.ProjectBuilders.XUnit.Tests.Integration.Builders;

using System.Linq;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders;
using NetEvolve.ProjectBuilders.Builders;
using NetEvolve.ProjectBuilders.Models;

public class ProjectBuilderTests(TemporaryDirectoryFixture directory) : IClassFixture<TemporaryDirectoryFixture>
{
    [Theory]
    [MemberData(nameof(GetTargetFrameworkValues))]
    public async ValueTask CreateAsync_TargetFrameworkTheory_Expected(
        string? sdk,
        NullableOptions nullable,
        TargetFramework targetFramework
    )
    {
        var subdirectory = directory.CreateDirectory(
            $"{nameof(CreateAsync_TargetFrameworkTheory_Expected)}{Guid.NewGuid()}"
        );
        await using var builder = new ProjectBuilder(subdirectory, Constants.CSharpProjectFileName);

        await builder
            .WithNullable(nullable)
            .WithTargetFramework(targetFramework)
            .SetProjectSdk(sdk)
            .CreateAsync(TestContext.Current.CancellationToken);

        _ = await VerifyFile(builder.FullPath, extension: "xml")
            .UseParameters(nullable, targetFramework, sdk)
            .HashParameters();
    }

    [Theory]
    [MemberData(nameof(GetTargetFrameworkValues))]
    public async ValueTask CreateAsync_TargetFrameworksTheory_Expected(
        string? sdk,
        NullableOptions nullable,
        TargetFramework targetFramework
    )
    {
        var subdirectory = directory.CreateDirectory(
            $"{nameof(CreateAsync_TargetFrameworksTheory_Expected)}{Guid.NewGuid()}"
        );
        await using var builder = new ProjectBuilder(subdirectory, Constants.CSharpProjectFileName);

        await builder
            .WithNullable(nullable)
            .WithTargetFrameworks(TargetFramework.NetStandard2_0, targetFramework)
            .SetProjectSdk(sdk)
            .CreateAsync(TestContext.Current.CancellationToken);

        _ = await VerifyFile(builder.FullPath, extension: "xml")
            .UseParameters(nullable, targetFramework, sdk)
            .HashParameters();
    }

    public static TheoryData<string?, NullableOptions, TargetFramework> GetTargetFrameworkValues()
    {
        var data = new TheoryData<string?, NullableOptions, TargetFramework>();
        var sdks = new string?[] { null, "Microsoft.NET.Sdk", "Microsoft.NET.Sdk.Web" };
        var nullableOptions = Enum.GetValues<NullableOptions>();
        var targetFrameworks = new TargetFramework[]
        {
            TargetFramework.NetStandard2_0,
            TargetFramework.Net10Windows,
            TargetFramework.NetFramework4_8_1,
        };
        foreach (
            var c in from sdk in sdks
            from nullable in nullableOptions
            from targetFramework in targetFrameworks
            select (sdk, nullable, targetFramework)
        )
        {
            data.Add(c.sdk, c.nullable, c.targetFramework);
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(AddPackageReference_Newtonsoft_Expected_Data))]
    public async ValueTask AddPackageReference_Newtonsoft_Expected(
        string? version,
        string? versionOverride,
        bool generatePathProperty,
        string? aliases,
        ReferenceAssets? includeAssets,
        ReferenceAssets? excludeAssets,
        ReferenceAssets? privateAssets
    )
    {
        var subdirectory = directory.CreateDirectory(
            $"{nameof(AddPackageReference_Newtonsoft_Expected)}{Guid.NewGuid()}"
        );
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
            .CreateAsync(TestContext.Current.CancellationToken);

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

    public static TheoryData<
        string?,
        string?,
        bool,
        string?,
        ReferenceAssets?,
        ReferenceAssets?,
        ReferenceAssets?
    > AddPackageReference_Newtonsoft_Expected_Data()
    {
        var data =
            new TheoryData<string?, string?, bool, string?, ReferenceAssets?, ReferenceAssets?, ReferenceAssets?>();
        var versions = new string?[] { "13.0.1", null };
        var versionOverrides = new string?[] { "13.0.1", null };
        var aliasesOptions = new string?[] { "NJson", "" };
        ReferenceAssets?[] includeAssetsOptions = [ReferenceAssets.All, null];
        ReferenceAssets?[] exlcudeAssetsOptions = [ReferenceAssets.None, ReferenceAssets.Runtime];
        ReferenceAssets?[] privateAssetsOptions = [ReferenceAssets.Build | ReferenceAssets.ContentFiles, null];

        foreach (
            var c in from version in versions
            from versionOverride in versionOverrides
            from aliases in aliasesOptions
            from includeAssets in includeAssetsOptions
            from excludeAssets in exlcudeAssetsOptions
            from privateAssets in privateAssetsOptions
            select (version, versionOverride, aliases, includeAssets, excludeAssets, privateAssets)
        )
        {
            data.Add(c.version, c.versionOverride, true, c.aliases, c.includeAssets, c.excludeAssets, c.privateAssets);
            data.Add(c.version, c.versionOverride, false, c.aliases, c.includeAssets, c.excludeAssets, c.privateAssets);
        }

        return data;
    }
}
