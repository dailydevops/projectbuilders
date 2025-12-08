namespace NetEvolve.ProjectBuilders.Models.Output;

using System.Text.Json.Serialization;

/// <summary>
/// Represents a single diagnostic result from a SARIF analysis run.
/// </summary>
/// <remarks>
/// <para>
/// This class models a SARIF result object, which contains information about a single issue discovered
/// during static analysis. Each result represents one diagnostic violation, warning, or informational
/// message produced by the analysis tool.
/// </para>
/// <para>
/// Results include three primary pieces of information:
/// <list type="bullet">
/// <item><description><see cref="RuleId"/>: The identifier of the rule that was violated</description></item>
/// <item><description><see cref="Level"/>: The severity level ("error", "warning", "note", etc.)</description></item>
/// <item><description><see cref="Message"/>: Descriptive information about the issue</description></item>
/// </list>
/// </para>
/// <para>
/// Typical rule IDs include C# compiler error codes (e.g., "CS1002"), code analysis rules (e.g., "CA5000"),
/// or analyzer-specific diagnostic IDs. The level determines how the issue should be treated during the build.
/// </para>
/// </remarks>
public sealed class OutputRunResult
{
    /// <summary>
    /// Gets the identifier of the rule that was violated or the diagnostic code.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This identifies which rule, warning code, or diagnostic the result pertains to. Common formats include:
    /// <list type="bullet">
    /// <item><description>C# Compiler: "CS1234" (e.g., "CS1002" for missing semicolon)</description></item>
    /// <item><description>Code Analysis: "CA5000" (e.g., "CA1050" for design rules)</description></item>
    /// <item><description>Analyzers: Custom identifiers (e.g., "NETANALYZER001")</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <value>The rule identifier string, or <see langword="null"/> if not available.</value>
    [JsonPropertyName("ruleId")]
    public string? RuleId { get; set; }

    /// <summary>
    /// Gets the severity level of this result.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Specifies the importance and actionability of the result. Standard SARIF severity levels include:
    /// <list type="bullet">
    /// <item><description>"error": The most severe level, indicating a problem that must be addressed</description></item>
    /// <item><description>"warning": An issue that should be addressed but may not prevent the build</description></item>
    /// <item><description>"note": Informational message, usually non-blocking</description></item>
    /// <item><description>"none": No severity classification</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// The interpretation of error vs. warning severity is context-specific and may be influenced by
    /// project settings (e.g., "treat warnings as errors").
    /// </para>
    /// </remarks>
    /// <value>The severity level string ("error", "warning", "note", "none", etc.), or <see langword="null"/> if not specified.</value>
    [JsonPropertyName("level")]
    public string? Level { get; set; }

    /// <summary>
    /// Gets the message containing details about the result.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The message provides human-readable details about the diagnostic issue, including the nature
    /// of the problem, why it matters, and often suggestions for remediation.
    /// </para>
    /// </remarks>
    /// <value>An <see cref="OutputRunResultMessage"/> object containing the message text, or <see langword="null"/> if no message is available.</value>
    [JsonPropertyName("message")]
    public OutputRunResultMessage? Message { get; set; }

    /// <summary>
    /// Returns a formatted string representation of this result.
    /// </summary>
    /// <remarks>
    /// The format is "{Level}:{RuleId} {Message}", making it suitable for display in build output or logs.
    /// </remarks>
    /// <returns>A formatted string in the pattern "error:CS1002 Missing semicolon".</returns>
    public override string ToString() => $"    {Level}:{RuleId} {Message}";
}
