namespace NetEvolve.ProjectBuilders.Abstractions;

using System;
using System.IO;

/// <summary>
/// Represents a general project builder.
/// </summary>
public interface IProjectBuilder : IFileBuilder
{
    /// <summary>
    /// Creates a file in the temporary directory.
    /// </summary>
    /// <param name="fileName">The name of the file to create.</param>
    /// <returns>
    /// A writeable <see cref="Stream"/> to the file.
    /// </returns>
    Stream CreateFile(string fileName);

    /// <summary>
    /// Gets or adds an item of the specified type to the project.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the item to get or add.
    /// </typeparam>
    /// <returns>
    /// The item.
    /// </returns>
    T GetOrAddItemGroupItem<T>()
        where T : class, IItemGroupItem, new();

    /// <summary>
    /// Gets or adds an item of the specified type to the project.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the item to get or add.
    /// </typeparam>
    /// <returns>
    /// The item.
    /// </returns>
    T GetOrAddPropertyGroupItem<T>()
        where T : class, IPropertyGroupItem, new();

    /// <summary>
    /// Set the project SDK.
    /// </summary>
    /// <param name="sdk"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="sdk"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="sdk"/> is empty or whitespace.
    /// </exception>
    IProjectBuilder SetProjectSdk(string sdk);
}
