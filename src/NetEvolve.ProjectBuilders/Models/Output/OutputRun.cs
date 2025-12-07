namespace NetEvolve.ProjectBuilders.Models.Output;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

/// <summary>
/// Represents the output of a sarif run.
/// </summary>
public sealed class OutputRun
{
    /// <summary>
    /// List of results.
    /// </summary>
    [JsonPropertyName("results")]
    [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "As designed.")]
    public List<OutputRunResult>? Results { get; init; }
}
