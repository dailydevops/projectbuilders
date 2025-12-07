namespace NetEvolve.ProjectBuilders.Abstractions;

/// <summary>
/// Represents a <b>&lt;ItemGroup&gt;</b> item in a project file.
/// </summary>
public interface IItemGroupItem
{
    /// <summary>
    /// The name of the item group item.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The condition of the item group item.
    /// </summary>
    string? Condition => null;

    /// <summary>
    /// The label of the item group item.
    /// </summary>
    string? Label => null;

    /// <summary>
    /// The include of the item group item.
    /// </summary>
    string Include { get; }
}
