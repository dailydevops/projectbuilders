namespace NetEvolve.ProjectBuilders.Abstractions;

using System.Collections.ObjectModel;

/// <summary>
/// Represents a <b>&lt;ItemGroup&gt;</b> in a project file.
/// </summary>
public interface IItemGroup
{
    /// <summary>
    /// The items of the item group.
    /// </summary>
    ReadOnlyCollection<IItemGroupItem> Items { get; }

    /// <summary>
    /// Adds a new item to the item group.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the item to add.
    /// </typeparam>
    /// <returns>
    /// The added item.
    /// </returns>
    T Add<T>()
        where T : class, IItemGroupItem, new();
}
