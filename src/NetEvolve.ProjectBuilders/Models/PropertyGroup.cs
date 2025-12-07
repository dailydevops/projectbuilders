namespace NetEvolve.ProjectBuilders.Models;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using NetEvolve.Arguments;
using NetEvolve.ProjectBuilders.Abstractions;

/// <inheritdoc cref="IPropertyGroup" />
internal sealed record PropertyGroup : IPropertyGroup
{
    private readonly List<IPropertyGroupItem> _items;

    public PropertyGroup() => _items = [];

    /// <inheritdoc cref="IPropertyGroup.Items" />
    public ReadOnlyCollection<IPropertyGroupItem> Items => _items.AsReadOnly();

    internal void Add(string key, string? value)
    {
        Argument.ThrowIfNullOrWhiteSpace(key);

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
