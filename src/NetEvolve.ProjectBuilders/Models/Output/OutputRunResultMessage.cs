namespace NetEvolve.ProjectBuilders.Models.Output;

using System.Text.Json.Serialization;

/// <summary>
/// Represents the output of a sarif run result message.
/// </summary>
public sealed class OutputRunResultMessage
{
    /// <summary>
    /// The text of the message.
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    /// <inheritdoc/>
    public override string ToString() => string.IsNullOrWhiteSpace(Text) ? string.Empty : Text;
}
