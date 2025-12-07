namespace NetEvolve.ProjectBuilders.Models.Output;

using System.Text.Json.Serialization;

/// <summary>
/// Represents the message content of a SARIF diagnostic result.
/// </summary>
/// <remarks>
/// <para>
/// This class wraps the text of a diagnostic message within a SARIF result. In full SARIF documents,
/// messages can be complex objects with formatting, location information, and references to markdown-formatted
/// strings. However, this implementation focuses on the plain text message content.
/// </para>
/// <para>
/// The message provides human-readable information about the diagnostic issue, explaining what was
/// detected and often providing context or suggestions for fixing the issue.
/// </para>
/// <para>
/// Example messages:
/// <code>
/// - "The type initializer threw an exception."
/// - "Using directive is unnecessary."
/// - "Variable 'x' is assigned but its value is never used"
/// </code>
/// </para>
/// </remarks>
public sealed class OutputRunResultMessage
{
    /// <summary>
    /// Gets the plain text content of the diagnostic message.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This contains the textual description of the diagnostic issue in a human-readable format.
    /// The message may include context about where the issue was detected and suggestions for remediation.
    /// </para>
    /// <para>
    /// The text may be empty or <see langword="null"/> if the result has no textual message content.
    /// </para>
    /// </remarks>
    /// <value>
    /// The message text as a string, or <see langword="null"/> if the message content is not available.
    /// </value>
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    /// <summary>
    /// Returns the text content of this message.
    /// </summary>
    /// <remarks>
    /// Returns an empty string if the text is <see langword="null"/> or whitespace to ensure clean output.
    /// </remarks>
    /// <returns>The message text, or an empty string if the text is <see langword="null"/> or whitespace.</returns>
    public override string ToString() => string.IsNullOrWhiteSpace(Text) ? string.Empty : Text;
}
