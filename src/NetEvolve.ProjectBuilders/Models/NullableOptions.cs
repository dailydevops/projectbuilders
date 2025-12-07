namespace NetEvolve.ProjectBuilders.Models;

/// <summary>
/// Specifies the nullable context options for a SDK-based project.
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references#nullable-contexts"/>
public enum NullableOptions
{
    /// <summary>
    /// This value will be ignored.
    /// </summary>
    None = 0,

    /// <summary>
    /// The compiler enables all null reference analysis and all language features.
    /// </summary>
    Enable,

    /// <summary>
    /// The code is <i>nullable-oblivious</i>. Disable matches the behavior before nullable reference types were enabled, except the new syntax produces warnings instead of errors.
    /// </summary>
    Disable,

    /// <summary>
    /// The compiler performs all null analysis and emits warnings when code might dereference <see langword="null"/>.
    /// </summary>
    Warnings,

    /// <summary>
    /// The compiler doesn't emit warnings when code might dereference <see langword="null"/>, or when you assign a maybe-null expression to a non-nullable variable.
    /// </summary>
    Annotations,
}
