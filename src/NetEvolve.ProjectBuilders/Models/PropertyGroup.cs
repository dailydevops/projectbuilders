namespace NetEvolve.ProjectBuilders.Models;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using NetEvolve.Arguments;
using NetEvolve.ProjectBuilders.Abstractions;

/// <summary>
/// Manages a collection of properties within a project file's PropertyGroup element.
/// </summary>
/// <remarks>
/// <para>
/// This internal class implements <see cref="IPropertyGroup"/> to provide a mutable collection of
/// project properties that configure build behavior, project metadata, and other settings.
/// </para>
/// <para>
/// The class supports both typed property items (via the generic Add method) and dynamic properties
/// (via the internal Add method with key-value pairs). Properties are stored in an internal list
/// and exposed as a read-only collection to prevent external modification.
/// </para>
/// </remarks>
/// <seealso cref="IPropertyGroup"/>
/// <seealso cref="IPropertyGroupItem"/>
internal sealed record PropertyGroup : IPropertyGroup
{
    private readonly List<IPropertyGroupItem> _items;

    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyGroup"/> class.
    /// </summary>
    public PropertyGroup() => _items = [];

    /// <inheritdoc cref="IPropertyGroup.Items" />
    public ReadOnlyCollection<IPropertyGroupItem> Items => _items.AsReadOnly();

    /// <summary>
    /// Adds a dynamic property with a key-value pair.
    /// </summary>
    /// <remarks>
    /// This internal method allows adding simple string-valued properties without needing
    /// a dedicated typed class. It's used for straightforward properties that don't require
    /// special handling.
    /// </remarks>
    /// <param name="key">The property name. Must not be <see langword="null"/> or whitespace.</param>
    /// <param name="value">The property value. Can be <see langword="null"/> for empty properties.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="key"/> is <see langword="null"/>, empty, or whitespace.
    /// </exception>
    internal void Add(string key, string? value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        _items.Add(new PropertyGroupItem(key, value));
    }

    /// <inheritdoc cref="IPropertyGroup.Add{T}" />
    public T Add<T>()
        where T : class, IPropertyGroupItem, new()
    {
        var item = new T();
        _items.Add(item);

        return item;
    }
}
