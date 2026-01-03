namespace NetEvolve.ProjectBuilders.XUnit.Tests.Integration;

using System;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders;
using NetEvolve.ProjectBuilders.Builders;

public class CSharpProjectTests(TemporaryDirectoryFixture directory) : IClassFixture<TemporaryDirectoryFixture>
{
    [Theory]
    [MemberData(nameof(AddCSharpFileData))]
    public async Task BuildAsync_CSharp_Theory(bool expectedErrors, bool expectedWarnings, string content)
    {
        var projectDirectory = directory.CreateDirectory($"{nameof(BuildAsync_CSharp_Theory)}{Guid.NewGuid()}");
        var nugetDirectory = directory.CreateDirectory($"{nameof(BuildAsync_CSharp_Theory)}{Guid.NewGuid()}");

        await using var nugetFactory = new TestPackageBuilder(nugetDirectory);
        await using var factory = ProjectFactory.Create(nugetFactory, projectDirectory);

        var result = await factory
            .AddCSharpProject(builder => builder.WithDefaults().AddCSharpFile("main.cs", content))
            .AddGlobalJson(configure: projectBuilder => projectBuilder.WithDefaults())
            .BuildAsync(cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(result.HasErrors(), expectedErrors);
        Assert.Equal(result.HasWarnings(), expectedWarnings);
    }

    [Theory]
    [MemberData(nameof(AddCSharpFileData))]
    public async Task BuildAsync_CSharp_VerifyDirectory(bool expectedErrors, bool expectedWarnings, string content)
    {
        var projectDirectory = directory.CreateDirectory(
            $"{nameof(BuildAsync_CSharp_VerifyDirectory)}{Guid.NewGuid()}"
        );
        var nugetDirectory = directory.CreateDirectory($"{nameof(BuildAsync_CSharp_VerifyDirectory)}{Guid.NewGuid()}");

        await using var nugetFactory = new TestPackageBuilder(nugetDirectory);
        await using var factory = ProjectFactory.Create(nugetFactory, projectDirectory);

        _ = await factory
            .AddCSharpProject(builder => builder.WithDefaults().AddCSharpFile("main.cs", content))
            .AddGlobalJson(configure: projectBuilder => projectBuilder.WithDefaults())
            .BuildAsync(cancellationToken: TestContext.Current.CancellationToken);

        _ = await VerifyDirectory(projectDirectory.FullPath, include: ProjectHelpers.DirectoryFilter)
            .UseParameters(expectedErrors, expectedWarnings, content)
            .HashParameters();
    }

    public static TheoryData<bool, bool, string> AddCSharpFileData =>
        new TheoryData<bool, bool, string>
        {
            { false, false, "class Program { static void Main() { System.Console.WriteLine(\"Hello, World!\"); } }" },
            { true, false, "class Program { static void Main() { WriteLine(\"Hello, World!\"); } }" },
            {
                false,
                true,
                "class Program { static async void Main() { System.Console.WriteLine(\"Hello, World!\"); } }"
            },
        };
}
