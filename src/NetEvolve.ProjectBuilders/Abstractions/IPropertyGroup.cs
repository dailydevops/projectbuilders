namespace NetEvolve.ProjectBuilders.Abstractions;

using System.Collections.ObjectModel;

/// <summary>
/// Represents a <b>&lt;PropertyGroup&gt;</b> in a project file.
/// </summary>
public interface IPropertyGroup
{
    /// <summary>
    /// The items of the property group.
    /// </summary>
    ReadOnlyCollection<IPropertyGroupItem> Items { get; }

    /// <summary>
    /// Adds a new item to the property group.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the item to add.
    /// </typeparam>
    /// <returns>
    /// The added item.
    /// </returns>
    T Add<T>()
        where T : class, IPropertyGroupItem, new();
}
