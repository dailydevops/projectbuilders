namespace NetEvolve.ProjectBuilders.Models;

using Microsoft.Extensions.Primitives;
using NetEvolve.ProjectBuilders.Abstractions;

/// <summary>
/// Represents a simple property element in a project file's PropertyGroup.
/// </summary>
/// <remarks>
/// <para>
/// This internal class implements <see cref="IPropertyGroupItem"/> to provide storage for
/// a simple named property with one or more values. It's used for straightforward properties
/// that don't require special type conversion or handling.
/// </para>
/// <para>
/// This class is typically instantiated by the <see cref="PropertyGroup.Add(string, string?)"/> method
/// when adding dynamic properties to a property group.
/// </para>
/// </remarks>
/// <seealso cref="IPropertyGroupItem"/>
/// <seealso cref="PropertyGroup"/>
internal sealed record PropertyGroupItem : IPropertyGroupItem
{
    /// <summary>
    /// Gets the name of the property.
    /// </summary>
    /// <remarks>
    /// This is the property name as it appears in the project file XML element.
    /// </remarks>
    /// <value>The property name, typically capitalized per MSBuild conventions (e.g., "OutputPath", "AssemblyName").</value>
    public string Name { get; }

    /// <summary>
    /// Gets the values of the property.
    /// </summary>
    /// <remarks>
    /// Although most properties have a single value, this field can contain multiple values
    /// if needed, stored as a StringValues collection.
    /// </remarks>
    /// <value>A StringValues object representing the property value(s).</value>
    public StringValues Values { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyGroupItem"/> class.
    /// </summary>
    /// <param name="key">The property name.</param>
    /// <param name="value">The property value.</param>
    public PropertyGroupItem(string key, string? value)
    {
        Name = key;
        Values = value;
    }
}
