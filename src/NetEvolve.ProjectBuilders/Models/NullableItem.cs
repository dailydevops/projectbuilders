namespace NetEvolve.ProjectBuilders.Models;

using Microsoft.Extensions.Primitives;
using NetEvolve.ProjectBuilders.Abstractions;

/// <summary>
/// Represents the &lt;Nullable&gt; property in a project file.
/// </summary>
/// <remarks>
/// <para>
/// This class implements <see cref="IPropertyGroupItem{T}"/> to provide type-safe configuration
/// of the Nullable property in .NET projects. The Nullable property controls how the C# compiler
/// handles nullable reference types.
/// </para>
/// <para>
/// Valid values are defined by the <see cref="NullableOptions"/> enum:
/// <list type="bullet">
/// <item><description>Enable: Full nullable reference type checking</description></item>
/// <item><description>Disable: Disables nullable reference type checking</description></item>
/// <item><description>Warnings: Only warnings for potential null reference issues</description></item>
/// <item><description>Annotations: Only tracks null dereference locations</description></item>
/// </list>
/// </para>
/// <para>
/// See <see href="https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references#nullable-contexts"/>
/// for detailed information on nullable contexts.
/// </para>
/// </remarks>
/// <seealso cref="IPropertyGroupItem{T}"/>
/// <seealso cref="NullableOptions"/>
internal sealed record NullableItem : IPropertyGroupItem<NullableOptions>
{
    /// <inheritdoc/>
    public string Name => "Nullable";

    /// <inheritdoc/>
    public StringValues Values { get; internal set; }

    /// <inheritdoc/>
    public void SetValue(NullableOptions value) => Values = ConvertValue(value);

    /// <inheritdoc/>
    public void SetValues(NullableOptions[] values) { }

    private static StringValues ConvertValue(NullableOptions value) =>
        value switch
        {
            NullableOptions.Enable => "enable",
            NullableOptions.Disable => "disable",
            NullableOptions.Warnings => "warnings",
            NullableOptions.Annotations => "annotations",
            _ => null,
        };
}
