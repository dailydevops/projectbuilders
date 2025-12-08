namespace NetEvolve.ProjectBuilders.Models.Output;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

/// <summary>
/// Represents a single analysis run within a SARIF output file.
/// </summary>
/// <remarks>
/// <para>
/// This class models a single run of an analysis tool, containing all results produced by that
/// invocation. In a typical SARIF file, there is often one run per analysis tool or tool invocation,
/// but SARIF supports multiple runs within a single file to aggregate results from different tools
/// or different invocation parameters.
/// </para>
/// <para>
/// The run contains a list of <see cref="OutputRunResult"/> objects, each representing a diagnostic
/// issue discovered during the analysis. Results include information such as the rule that was violated,
/// the severity level, and descriptive messages.
/// </para>
/// <para>
/// For more information about SARIF run objects, see
/// <see href="https://docs.oasis-open.org/sarif/sarif/v2.1.0/csd02/schemas/invocation-object.json"/>.
/// </para>
/// </remarks>
public sealed class OutputRun
{
    /// <summary>
    /// Gets the list of results produced by this analysis run.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Each result represents a single diagnostic issue found during analysis. Results contain
    /// information such as the rule ID, severity level, location in source code, and descriptive messages.
    /// </para>
    /// <para>
    /// This property may be <see langword="null"/> if the analysis run produced no results.
    /// </para>
    /// </remarks>
    /// <value>
    /// A list of <see cref="OutputRunResult"/> objects, or <see langword="null"/> if no results were produced.
    /// </value>
    [JsonPropertyName("results")]
    [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "As designed.")]
    public List<OutputRunResult>? Results { get; init; }
}
