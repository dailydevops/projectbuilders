namespace NetEvolve.ProjectBuilders.XUnit.Tests.Integration;

using System;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders;
using NetEvolve.ProjectBuilders.Builders;

public class VBProjectTests(TemporaryDirectoryFixture directory) : IClassFixture<TemporaryDirectoryFixture>
{
    [Theory]
    [MemberData(nameof(AddVBFileData))]
    public async Task BuildAsync_VB_Theory(bool expectedErrors, bool expectedWarnings, string content)
    {
        var projectDirectory = directory.CreateDirectory($"{nameof(BuildAsync_VB_Theory)}{Guid.NewGuid()}");
        var nugetDirectory = directory.CreateDirectory($"{nameof(BuildAsync_VB_Theory)}{Guid.NewGuid()}");

        await using var nugetFactory = new TestPackageBuilder(nugetDirectory);
        await using var factory = ProjectFactory.Create(nugetFactory, projectDirectory);

        var result = await factory
            .AddVBProject(builder => builder.WithDefaults().AddVBFile("main.vb", content))
            .AddGlobalJson(configure: projectBuilder => projectBuilder.WithDefaults())
            .BuildAsync(cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(result.HasErrors(), expectedErrors);
        Assert.Equal(result.HasWarnings(), expectedWarnings);
    }

    [Theory]
    [MemberData(nameof(AddVBFileData))]
    public async Task BuildAsync_VB_VerifyDirectory(bool expectedErrors, bool expectedWarnings, string content)
    {
        var projectDirectory = directory.CreateDirectory($"{nameof(BuildAsync_VB_Theory)}{Guid.NewGuid()}");
        var nugetDirectory = directory.CreateDirectory($"{nameof(BuildAsync_VB_Theory)}{Guid.NewGuid()}");

        await using var nugetFactory = new TestPackageBuilder(nugetDirectory);
        await using var factory = ProjectFactory.Create(nugetFactory, projectDirectory);

        _ = await factory
            .AddVBProject(builder => builder.WithDefaults().AddVBFile("main.vb", content))
            .AddGlobalJson(configure: projectBuilder => projectBuilder.WithDefaults())
            .BuildAsync(cancellationToken: TestContext.Current.CancellationToken);

        _ = await VerifyDirectory(projectDirectory.FullPath, include: ProjectHelpers.DirectoryFilter)
            .UseParameters(expectedErrors, expectedWarnings, content)
            .HashParameters();
    }

    public static TheoryData<bool, bool, string> AddVBFileData =>
        new TheoryData<bool, bool, string>
        {
            {
                false,
                false,
                """
                    Module Program
                        Sub Main()
                            System.Console.WriteLine("Hello, World!")
                        End Sub
                    End Module
                    """
            },
            {
                true,
                false,
                """
                    Module Program
                        Sub Main()
                            WriteLine(""Hello, World!"")
                        End Sub
                    End Module
                    """
            },
            {
                false,
                true,
                """
                    Module Program
                        Async Sub Main()
                            System.Console.WriteLine("Hello, World!")
                        End Sub
                    End Module
                    """
            },
        };
}
