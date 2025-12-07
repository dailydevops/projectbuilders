namespace NetEvolve.ProjectBuilders.Models;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using NetEvolve.ProjectBuilders.Abstractions;

/// <inheritdoc cref="IItemGroup" />
internal sealed record ItemGroup : IItemGroup
{
    private readonly List<IItemGroupItem> _items;

    public ItemGroup() => _items = [];

    /// <inheritdoc cref="IItemGroup.Items" />
    public ReadOnlyCollection<IItemGroupItem> Items => _items.AsReadOnly();

    /// <inheritdoc cref="IItemGroup.Add{T}" />
    public T Add<T>()
        where T : class, IItemGroupItem, new()
    {
        var item = new T();
        _items.Add(item);

        return item;
    }
}
