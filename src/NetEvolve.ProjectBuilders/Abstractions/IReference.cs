namespace NetEvolve.ProjectBuilders.Abstractions;

using System.Collections.Generic;

/// <summary>
/// Represents an item that provides a collection of directory paths for lookup operations.
/// </summary>
public interface IReference : IItemGroupItem
{
    /// <summary>
    /// Gets the collection of directory paths used for lookup operations.
    /// </summary>
    IEnumerable<string> LookUpPaths => [];
}
