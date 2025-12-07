namespace NetEvolve.ProjectBuilders.Abstractions;

using System.Linq;
using Microsoft.Extensions.Primitives;

/// <summary>
/// Represents a <b>&lt;PropertyGroup&gt;</b> item in a project file.
/// </summary>
public interface IPropertyGroupItem
{
    /// <summary>
    /// The name of the property group item.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The values of the property group item.
    /// </summary>
    StringValues Values { get; }

    /// <summary>
    /// The condition of the property group item.
    /// </summary>
    string? Condition => null;

    /// <summary>
    /// The label of the property group item.
    /// </summary>
    string? Label => null;

    /// <summary>
    /// Determines whether the property group item is null or empty.
    /// </summary>
    bool IsNullOrEmpty => StringValues.IsNullOrEmpty(Values);

    /// <summary>
    /// Gets the value of the property group item, based on specific rules.
    /// </summary>
    string GetValue() => string.Join(';', Values.Distinct());
}
