namespace NetEvolve.ProjectBuilders.Models.Output;

using System.Text.Json.Serialization;

/// <summary>
/// Represents the output of a sarif run result.
/// </summary>
public sealed class OutputRunResult
{
    /// <summary>
    /// The rule id.
    /// </summary>
    [JsonPropertyName("ruleId")]
    public string? RuleId { get; set; }

    /// <summary>
    /// The level of the message.
    /// </summary>
    [JsonPropertyName("level")]
    public string? Level { get; set; }

    /// <summary>
    /// The message.
    /// </summary>
    [JsonPropertyName("message")]
    public OutputRunResultMessage? Message { get; set; }

    /// <inheritdoc/>
    public override string ToString() => $"    {Level}:{RuleId} {Message}";
}
