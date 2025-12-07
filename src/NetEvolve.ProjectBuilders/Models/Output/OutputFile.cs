namespace NetEvolve.ProjectBuilders.Models.Output;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;

/// <summary>
/// Represents the output of a sarif file.
/// </summary>
public sealed class OutputFile
{
    /// <summary>
    /// List of runs.
    /// </summary>
    [JsonPropertyName("runs")]
    [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "As designed.")]
    public List<OutputRun> Runs { get; init; } = default!;

    private List<OutputRunResult>? _results;

    /// <summary>
    /// Combined list of results from all runs.
    /// </summary>
    [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "As designed.")]
    public List<OutputRunResult> Results
    {
        get
        {
            if (_results is null)
            {
                _results =
                    Runs?.SelectMany(r =>
                        {
                            if (r.Results is null)
                            {
                                return [];
                            }
                            return r.Results;
                        })
                        .ToList() ?? [];
            }

            return _results;
        }
    }

    /// <summary>
    /// Checks if there are any errors in the results.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if there are errors, <see langword="false"/> otherwise.
    /// </returns>
    public bool HasErrors() => Results.Exists(r => r.Level == "error");

    /// <summary>
    /// Checks if there are specific errors in the results.
    /// </summary>
    /// <param name="ruleId">
    /// The rule id to check for.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if there are errors, <see langword="false"/> otherwise.
    /// </returns>
    public bool HasError(string ruleId) => Results.Exists(r => r.Level == "error" && r.RuleId == ruleId);

    /// <summary>
    /// Checks if there are no errors or warnings in the results.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if there are no errors or warnings, <see langword="false"/> otherwise.
    /// </returns>
    public bool HasNoErrorsOrWarnings() => Results.TrueForAll(r => r.Level is not "error" and not "warning");

    /// <summary>
    /// Checks if there are warnrings in the results.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if there are warnings, <see langword="false"/> otherwise.
    /// </returns>
    public bool HasWarnings() => Results.Exists(r => r.Level == "warning");

    /// <summary>
    /// Checks if there are warnrings in the results.
    /// </summary>
    /// <param name="ruleId">
    /// The rule id to check for.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if there are warnings, <see langword="false"/> otherwise.
    /// </returns>
    public bool HasWarning(string ruleId) => Results.Exists(r => r.Level == "warning" && r.RuleId == ruleId);
}
