namespace NetEvolve.ProjectBuilders.Tests.Unit.Models.Output;

using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Models.Output;

public class OutputFileTests
{
    [Test]
    public async Task Results_WithNullRuns_ReturnsEmpty()
    {
        // Arrange
        var file = new OutputFile { Runs = null! };

        // Act
        var results = file.Results;

        // Assert
        _ = await Assert.That(results).IsEmpty();
    }

    [Test]
    public async Task Results_WithRunHavingNullResults_SkipsRun()
    {
        // Arrange
        var file = new OutputFile { Runs = [new OutputRun { Results = null }] };

        // Act
        var results = file.Results;

        // Assert
        _ = await Assert.That(results).IsEmpty();
    }

    [Test]
    public async Task Results_IsCachedAfterFirstAccess()
    {
        // Arrange
        var file = new OutputFile { Runs = [new OutputRun { Results = [new OutputRunResult { Level = "error" }] }] };

        // Act
        var first = file.Results;
        var second = file.Results;

        // Assert
        _ = await Assert.That(ReferenceEquals(first, second)).IsTrue();
    }

    private static OutputFile CreateFile(params string[] levels)
    {
        var results = new System.Collections.Generic.List<OutputRunResult>();
        foreach (var level in levels)
        {
            results.Add(new OutputRunResult { Level = level, RuleId = "RULE1" });
        }

        return new OutputFile { Runs = [new OutputRun { Results = results }] };
    }

    [Test]
    public async Task HasErrors_WithErrorResult_ReturnsTrue()
    {
        // Arrange
        var file = CreateFile("error", "note");

        // Act & Assert
        _ = await Assert.That(file.HasErrors()).IsTrue();
    }

    [Test]
    public async Task HasErrors_WithoutErrorResult_ReturnsFalse()
    {
        // Arrange
        var file = CreateFile("warning", "note");

        // Act & Assert
        _ = await Assert.That(file.HasErrors()).IsFalse();
    }

    [Test]
    public async Task HasError_WithMatchingRuleId_ReturnsTrue()
    {
        // Arrange
        var file = CreateFile("error");

        // Act & Assert
        _ = await Assert.That(file.HasError("RULE1")).IsTrue();
    }

    [Test]
    public async Task HasError_WithNonMatchingRuleId_ReturnsFalse()
    {
        // Arrange
        var file = CreateFile("error");

        // Act & Assert
        _ = await Assert.That(file.HasError("OTHER")).IsFalse();
    }

    [Test]
    public async Task HasWarnings_WithWarningResult_ReturnsTrue()
    {
        // Arrange
        var file = CreateFile("warning");

        // Act & Assert
        _ = await Assert.That(file.HasWarnings()).IsTrue();
    }

    [Test]
    public async Task HasWarnings_WithoutWarningResult_ReturnsFalse()
    {
        // Arrange
        var file = CreateFile("note");

        // Act & Assert
        _ = await Assert.That(file.HasWarnings()).IsFalse();
    }

    [Test]
    public async Task HasWarning_WithMatchingRuleId_ReturnsTrue()
    {
        // Arrange
        var file = CreateFile("warning");

        // Act & Assert
        _ = await Assert.That(file.HasWarning("RULE1")).IsTrue();
    }

    [Test]
    public async Task HasWarning_WithNonMatchingRuleId_ReturnsFalse()
    {
        // Arrange
        var file = CreateFile("warning");

        // Act & Assert
        _ = await Assert.That(file.HasWarning("OTHER")).IsFalse();
    }

    [Test]
    public async Task HasNoErrorsOrWarnings_WithOnlyNotes_ReturnsTrue()
    {
        // Arrange
        var file = CreateFile("note", "none");

        // Act & Assert
        _ = await Assert.That(file.HasNoErrorsOrWarnings()).IsTrue();
    }

    [Test]
    public async Task HasNoErrorsOrWarnings_WithError_ReturnsFalse()
    {
        // Arrange
        var file = CreateFile("error");

        // Act & Assert
        _ = await Assert.That(file.HasNoErrorsOrWarnings()).IsFalse();
    }

    [Test]
    public async Task HasNoErrorsOrWarnings_WithNoResults_ReturnsTrue()
    {
        // Arrange
        var file = new OutputFile { Runs = [] };

        // Act & Assert
        _ = await Assert.That(file.HasNoErrorsOrWarnings()).IsTrue();
    }
}
