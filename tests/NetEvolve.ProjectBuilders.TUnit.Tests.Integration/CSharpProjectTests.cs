namespace NetEvolve.ProjectBuilders.Tests.Integration;

using System;
using System.Threading.Tasks;
using NetEvolve.Extensions.TUnit.Logging;
using NetEvolve.ProjectBuilders.Abstractions;
using NetEvolve.ProjectBuilders.Builders;
using NetEvolve.ProjectBuilders.Models;
using NetEvolve.ProjectBuilders.TUnit;

[ClassDataSource<TemporaryDirectory>]
public class CSharpProjectTests(TemporaryDirectory directory)
{
    [Test]
    [MethodDataSource(nameof(AddCSharpFileData))]
    public async Task BuildAsync_CSharp_Theory(bool expectedErrors, bool expectedWarnings, string content)
    {
        var logger = TestContext.Current!.GetDefaultLogger();
        var projectDirectory = directory.CreateDirectory($"{nameof(BuildAsync_CSharp_Theory)}{Guid.NewGuid()}");
        var nugetDirectory = directory.CreateDirectory($"{nameof(BuildAsync_CSharp_Theory)}{Guid.NewGuid()}");

        await using var nugetFactory = new TestPackageBuilder(nugetDirectory);
        await using var factory = ProjectFactory.Create(
            nugetFactory,
            projectDirectory,
            logger.ConvertTo<ProjectFactory>()
        );

        var result = await factory
            .AddCSharpProject(builder =>
            {
                builder.WithDefaults().AddCSharpFile("main.cs", content);
            })
            .AddGlobalJson(configure: projectBuilder => projectBuilder.WithDefaults())
            .BuildAsync();

        using (Assert.Multiple())
        {
            _ = await Assert.That(result.HasErrors()).IsEqualTo(expectedErrors);
            _ = await Assert.That(result.HasWarnings()).IsEqualTo(expectedWarnings);
        }
    }

    public static IEnumerable<(bool, bool, string)> AddCSharpFileData =>
        [
            (false, false, "class Program { static void Main() { System.Console.WriteLine(\"Hello, World!\"); } }"),
            (true, false, "class Program { static void Main() { WriteLine(\"Hello, World!\"); }"),
            (
                false,
                true,
                "class Program { static async void Main() { System.Console.WriteLine(\"Hello, World!\"); } }"
            ),
        ];
}
