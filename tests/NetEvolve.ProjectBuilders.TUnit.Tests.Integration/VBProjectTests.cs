namespace NetEvolve.ProjectBuilders.TUnit.Tests.Integration;

using System;
using System.Threading.Tasks;
using NetEvolve.Extensions.TUnit.Logging;
using NetEvolve.ProjectBuilders;
using NetEvolve.ProjectBuilders.Builders;

[ClassDataSource<TemporaryDirectory>]
public class VBProjectTests(TemporaryDirectory directory)
{
    [Test]
    [MethodDataSource(nameof(AddVBFileData))]
    public async Task BuildAsync_VB_Theory(bool expectedErrors, bool expectedWarnings, string content)
    {
        var logger = TestContext.Current!.GetDefaultLogger();
        var projectDirectory = directory.CreateDirectory($"{nameof(BuildAsync_VB_Theory)}{Guid.NewGuid()}");
        var nugetDirectory = directory.CreateDirectory($"{nameof(BuildAsync_VB_Theory)}{Guid.NewGuid()}");

        await using var nugetFactory = new TestPackageBuilder(nugetDirectory);
        await using var factory = ProjectFactory.Create(
            nugetFactory,
            projectDirectory,
            logger.ConvertTo<ProjectFactory>()
        );

        var result = await factory
            .AddVBProject(builder => builder.WithDefaults().AddVBFile("main.vb", content))
            .AddGlobalJson(configure: projectBuilder => projectBuilder.WithDefaults())
            .BuildAsync();

        using (Assert.Multiple())
        {
            _ = await Assert.That(result.HasErrors()).IsEqualTo(expectedErrors);
            _ = await Assert.That(result.HasWarnings()).IsEqualTo(expectedWarnings);
        }
    }

    [Test]
    [MethodDataSource(nameof(AddVBFileData))]
    public async Task BuildAsync_VB_VerifyDirectory(bool expectedErrors, bool expectedWarnings, string content)
    {
        var logger = TestContext.Current!.GetDefaultLogger();
        var projectDirectory = directory.CreateDirectory($"{nameof(BuildAsync_VB_Theory)}{Guid.NewGuid()}");
        var nugetDirectory = directory.CreateDirectory($"{nameof(BuildAsync_VB_Theory)}{Guid.NewGuid()}");

        await using var nugetFactory = new TestPackageBuilder(nugetDirectory);
        await using var factory = ProjectFactory.Create(
            nugetFactory,
            projectDirectory,
            logger.ConvertTo<ProjectFactory>()
        );

        _ = await factory
            .AddVBProject(builder => builder.WithDefaults().AddVBFile("main.vb", content))
            .AddGlobalJson(configure: projectBuilder => projectBuilder.WithDefaults())
            .BuildAsync();

        _ = await VerifyDirectory(projectDirectory.FullPath, include: ProjectHelpers.DirectoryFilter)
            .UseParameters(expectedErrors, expectedWarnings, content)
            .HashParameters();
    }

    public static IEnumerable<(bool, bool, string)> AddVBFileData =>
        [
            (
                false,
                false,
                """
                Module Program
                    Sub Main()
                        System.Console.WriteLine("Hello, World!")
                    End Sub
                End Module
                """
            ),
            (
                true,
                false,
                """
                Module Program
                    Sub Main()
                        WriteLine(""Hello, World!"")
                    End Sub
                End Module
                """
            ),
            (
                false,
                true,
                """
                Module Program
                    Async Sub Main()
                        System.Console.WriteLine("Hello, World!")
                    End Sub
                End Module
                """
            ),
        ];
}
