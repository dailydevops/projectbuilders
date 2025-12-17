namespace NetEvolve.ProjectBuilders.Models.Output;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;

/// <summary>
/// Represents the root object of a SARIF (Static Analysis Results Format) output file.
/// </summary>
/// <remarks>
/// <para>
/// This class models the top-level structure of a SARIF v2.1 compliant JSON file that contains
/// diagnostic results from static analysis tools like the .NET compiler and analyzers. SARIF is
/// a standard format for reporting analysis results across different tools.
/// </para>
/// <para>
/// The OutputFile aggregates multiple <see cref="OutputRun"/> instances and provides convenient
/// methods to query the complete set of results across all runs, enabling unified analysis result
/// processing regardless of how the results are logically organized.
/// </para>
/// <para>
/// For more information about SARIF format, see
/// <see href="https://docs.oasis-open.org/sarif/sarif/v2.1.0/sarif-v2.1.0.html"/>.
/// </para>
/// </remarks>
public sealed class OutputFile
{
    /// <summary>
    /// Gets the list of analysis runs included in this SARIF output file.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Each run typically represents a single invocation of an analyzer tool. Multiple runs may be
    /// present if results from different tools or tool invocations are aggregated.
    /// </para>
    /// <para>
    /// Use <see cref="Results"/> to retrieve all results across all runs in a single collection.
    /// </para>
    /// </remarks>
    /// <value>A list of <see cref="OutputRun"/> objects, each containing results from a single analysis run.</value>
    [JsonPropertyName("runs")]
    [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "As designed.")]
    public List<OutputRun> Runs { get; init; } = default!;

    private List<OutputRunResult>? _results;

    /// <summary>
    /// Gets a combined list of all results from all runs in the file.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property flattens the result hierarchy, combining all <see cref="OutputRunResult"/> objects
    /// from all <see cref="OutputRun"/> instances into a single collection. This is convenient for
    /// unified result processing without manually iterating through runs.
    /// </para>
    /// <para>
    /// The result list is cached after first access for performance.
    /// </para>
    /// </remarks>
    /// <value>
    /// A list containing all results from all runs. Returns an empty list if no runs contain results.
    /// </value>
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
                        .ToList()
                    ?? [];
            }

            return _results;
        }
    }

    /// <summary>
    /// Determines whether any results in the file have an error severity level.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Errors represent the most severe type of diagnostic issue, typically indicating a problem
    /// that must be fixed before the build succeeds.
    /// </para>
    /// <para>
    /// This method examines all results across all runs. Use <see cref="HasError(string)"/> to
    /// check for specific rule violations.
    /// </para>
    /// </remarks>
    /// <returns>
    /// <see langword="true"/> if any result has level "error"; otherwise, <see langword="false"/>.
    /// </returns>
    public bool HasErrors() => Results.Exists(r => r.Level == "error");

    /// <summary>
    /// Determines whether any error results match the specified rule ID.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Use this method to check for specific rule violations. Rules are identified by rule IDs
    /// that correspond to analyzer rules, compiler warnings, or code style violations.
    /// </para>
    /// <para>
    /// Example: Checking for "CS1002" for C# compiler errors or "CA1050" for design guidelines.
    /// </para>
    /// </remarks>
    /// <param name="ruleId">
    /// The rule identifier to search for. Typically follows patterns like "CS1234" for compiler errors
    /// or "CA5000" for code analysis rules.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if any result has level "error" and matches the specified rule ID;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool HasError(string ruleId) => Results.Exists(r => r.Level == "error" && r.RuleId == ruleId);

    /// <summary>
    /// Determines whether the results contain no errors or warnings.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Returns <see langword="true"/> if all results have severity levels other than "error" or "warning"
    /// (typically "note" or "none"), or if there are no results at all.
    /// </para>
    /// <para>
    /// This is useful for determining if a build has any actionable issues.
    /// </para>
    /// </remarks>
    /// <returns>
    /// <see langword="true"/> if no results have "error" or "warning" severity;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool HasNoErrorsOrWarnings() => Results.TrueForAll(r => r.Level is not "error" and not "warning");

    /// <summary>
    /// Determines whether any results in the file have a warning severity level.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Warnings represent issues that should be addressed but do not necessarily prevent the build
    /// from succeeding. The treatment of warnings depends on project settings.
    /// </para>
    /// <para>
    /// This method examines all results across all runs. Use <see cref="HasWarning(string)"/> to
    /// check for specific warning rules.
    /// </para>
    /// </remarks>
    /// <returns>
    /// <see langword="true"/> if any result has level "warning"; otherwise, <see langword="false"/>.
    /// </returns>
    public bool HasWarnings() => Results.Exists(r => r.Level == "warning");

    /// <summary>
    /// Determines whether any warning results match the specified rule ID.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Use this method to check for specific warning rules. This is useful for suppressing or
    /// validating particular categories of warnings.
    /// </para>
    /// <para>
    /// Example: Checking for "CS0618" for obsolete member usage warnings or "CA1014" for
    /// assembly-level warnings.
    /// </para>
    /// </remarks>
    /// <param name="ruleId">
    /// The rule identifier to search for.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if any result has level "warning" and matches the specified rule ID;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool HasWarning(string ruleId) => Results.Exists(r => r.Level == "warning" && r.RuleId == ruleId);
}
