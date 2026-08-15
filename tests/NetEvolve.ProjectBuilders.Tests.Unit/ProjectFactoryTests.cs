namespace NetEvolve.ProjectBuilders.Tests.Unit;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders;
using NetEvolve.ProjectBuilders.Abstractions;
using NetEvolve.ProjectBuilders.Builders;

public class ProjectFactoryTests
{
    [Test]
    public async Task BuildAsync_NoFileBuilder_ThrowsArgumentException()
    {
        var ex = await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await using var builder = ProjectFactory.Create();

            _ = await builder.BuildAsync();
        });

        using (Assert.Multiple())
        {
            _ = await Assert.That(ex).IsNotNull();
            _ = await Assert.That(ex.Message).IsNotNullOrWhiteSpace().And.EqualTo("No file builders were added.");
        }
    }

    [Test]
    public async Task BuildAsync_NoProjectBuilder_ThrowsArgumentException()
    {
        var ex = await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await using var builder = ProjectFactory.Create();

            _ = await builder.AddGlobalJson("8.0.204").BuildAsync();
        });

        using (Assert.Multiple())
        {
            _ = await Assert.That(ex).IsNotNull();
            _ = await Assert.That(ex.Message).IsNotNullOrWhiteSpace().And.EqualTo("No project builder were added.");
        }
    }

    [Test]
    public async Task AddEnvironmentVariable_WithNullName_ThrowsArgumentException()
    {
        await using var factory = ProjectFactory.Create();

        void Act() => factory.AddEnvironmentVariable(null!, "value");

        _ = Assert.Throws<ArgumentException>(Act);
    }

    [Test]
    public async Task AddEnvironmentVariable_WithNullValue_ThrowsArgumentException()
    {
        await using var factory = ProjectFactory.Create();

        void Act() => factory.AddEnvironmentVariable("NAME", null);

        _ = Assert.Throws<ArgumentException>(Act);
    }

    [Test]
    public async Task AddEnvironmentVariable_NewName_AddsVariable()
    {
        await using var factory = ProjectFactory.Create();

        _ = factory.AddEnvironmentVariable("MY_VAR", "1");

        _ = await Assert.That(((ProjectFactory)factory).EnvironmentVariables["MY_VAR"]).IsEqualTo("1");
    }

    [Test]
    public async Task AddEnvironmentVariable_ExistingName_OverwritesValue()
    {
        await using var factory = ProjectFactory.Create();

        _ = factory.AddEnvironmentVariable("MY_VAR", "1");
        _ = factory.AddEnvironmentVariable("MY_VAR", "2");

        _ = await Assert.That(((ProjectFactory)factory).EnvironmentVariables["MY_VAR"]).IsEqualTo("2");
    }

    [Test]
    public async Task AddEnvironmentVariables_WithNullArray_ThrowsArgumentNullException()
    {
        await using var factory = ProjectFactory.Create();

        void Act() => factory.AddEnvironmentVariables(null!);

        _ = Assert.Throws<ArgumentNullException>(Act);
    }

    [Test]
    public async Task AddEnvironmentVariables_WithMultipleValues_AddsAll()
    {
        await using var factory = ProjectFactory.Create();

        _ = factory.AddEnvironmentVariables(
            new KeyValuePair<string, string?>("VAR_A", "a"),
            new KeyValuePair<string, string?>("VAR_B", "b")
        );

        using (Assert.Multiple())
        {
            _ = await Assert.That(((ProjectFactory)factory).EnvironmentVariables["VAR_A"]).IsEqualTo("a");
            _ = await Assert.That(((ProjectFactory)factory).EnvironmentVariables["VAR_B"]).IsEqualTo("b");
        }
    }

    [Test]
    public async Task AddFileBuilder_WithNullBuilder_ThrowsArgumentNullException()
    {
        await using var factory = ProjectFactory.Create();

        void Act() => factory.AddFileBuilder<IProjectBuilder, ProjectBuilder>(null!);

        _ = Assert.Throws<ArgumentNullException>(Act);
    }

    [Test]
    public async Task AddFileBuilder_WithDuplicateInstance_ThrowsArgumentException()
    {
        await using var factory = ProjectFactory.Create();
        var directory = ((ProjectFactory)factory).DirectoryBuilder;
        await using var projectBuilder = new ProjectBuilder(directory, Constants.CSharpProjectFileName);
        _ = factory.AddFileBuilder<IProjectBuilder, ProjectBuilder>(projectBuilder);

        void Act() => factory.AddFileBuilder<IProjectBuilder, ProjectBuilder>(projectBuilder);

        var ex = Assert.Throws<ArgumentException>(Act);
        _ = await Assert.That(ex.Message).Contains("already registered");
    }

    [Test]
    public async Task DisposeAsync_CalledTwice_DoesNotThrow()
    {
        var factory = ProjectFactory.Create();
        var directory = ((ProjectFactory)factory).DirectoryBuilder;
        await using var projectBuilder = new ProjectBuilder(directory, Constants.CSharpProjectFileName);
        _ = factory.AddFileBuilder<IProjectBuilder, ProjectBuilder>(projectBuilder);

        await factory.DisposeAsync();
        await factory.DisposeAsync();
    }

    [Test]
    public async Task RulesFilter_MatchesNuGetRuleId()
    {
        var match = ProjectFactory.RulesFilter().Match("warning NU1605: some message");

        using (Assert.Multiple())
        {
            _ = await Assert.That(match.Success).IsTrue();
            _ = await Assert.That(match.Value).IsEqualTo("NU1605");
        }
    }

    [Test]
    public async Task RulesFilter_MatchesNetEvolveRuleId()
    {
        var match = ProjectFactory.RulesFilter().Match("error NEP1234: some other message");

        using (Assert.Multiple())
        {
            _ = await Assert.That(match.Success).IsTrue();
            _ = await Assert.That(match.Value).IsEqualTo("NEP1234");
        }
    }

    [Test]
    public async Task RulesFilter_NoMatch_ReturnsUnsuccessful()
    {
        var match = ProjectFactory.RulesFilter().Match("just a plain line without a rule id");

        _ = await Assert.That(match.Success).IsFalse();
    }
}
