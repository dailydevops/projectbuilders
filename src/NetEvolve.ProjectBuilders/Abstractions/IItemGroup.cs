namespace NetEvolve.ProjectBuilders.Abstractions;

using System.Collections.ObjectModel;

/// <summary>
/// Represents an ItemGroup element in an MSBuild project file.
/// </summary>
/// <remarks>
/// <para>
/// The ItemGroup is one of the fundamental building blocks in MSBuild project files. It contains
/// items that represent inputs to the build process, such as package references, project references,
/// framework references, and other file-based items.
/// </para>
/// <para>
/// Each item in the group can have various attributes and properties that influence how the
/// build system processes it. Items can be conditional using the Condition attribute.
/// </para>
/// <para>
/// See <see href="https://learn.microsoft.com/en-us/visualstudio/msbuild/itemgroup-element-msbuild"/> for more information.
/// </para>
/// </remarks>
public interface IItemGroup
{
    /// <summary>
    /// Gets a read-only collection of items in this item group.
    /// </summary>
    /// <value>A read-only collection of <see cref="IItemGroupItem"/> objects.</value>
    ReadOnlyCollection<IItemGroupItem> Items { get; }

    /// <summary>
    /// Adds a new item of the specified type to this item group.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the item to add. Must be a non-abstract class implementing <see cref="IItemGroupItem"/>
    /// with a public parameterless constructor.
    /// </typeparam>
    /// <returns>
    /// The newly created and added item instance.
    /// </returns>
    T Add<T>()
        where T : class, IItemGroupItem, new();
}
