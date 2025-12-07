namespace NetEvolve.ProjectBuilders.Models;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using NetEvolve.ProjectBuilders.Abstractions;

/// <summary>
/// Manages a collection of items within a project file's ItemGroup element.
/// </summary>
/// <remarks>
/// <para>
/// This internal class implements <see cref="IItemGroup"/> to provide a mutable collection of
/// project items such as package references, project references, and framework references.
/// </para>
/// <para>
/// The class stores items in an internal list and exposes them as a read-only collection to
/// prevent external modification of the collection structure while building the project file.
/// </para>
/// </remarks>
/// <seealso cref="IItemGroup"/>
/// <seealso cref="IItemGroupItem"/>
internal sealed record ItemGroup : IItemGroup
{
    private readonly List<IItemGroupItem> _items;

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemGroup"/> class.
    /// </summary>
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
