namespace NetEvolve.ProjectBuilders.Abstractions;

using System.IO;

/// <summary>
/// Represents a subdirectory builder, which can be configured to build a subdirectory in a temporary directory.
/// </summary>
public interface ISubdirectoryBuilder : IObjectBuilder
{
    /// <summary>
    /// Creates a subdirectory in the temporary directory.
    /// </summary>
    /// <param name="directoryName">
    /// The name of the subdirectory to create.
    /// </param>
    /// <returns>
    /// A builder for the created subdirectory.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">
    /// Thrown when <paramref name="directoryName"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="System.ArgumentException">
    /// Thrown when <paramref name="directoryName"/> is empty or contains only whitespace characters.
    /// </exception>
    ISubdirectoryBuilder CreateDirectory(string directoryName);

    /// <summary>
    /// Creates a file in the temporary directory.
    /// </summary>
    /// <param name="fileName">The name of the file to create.</param>
    /// <returns>
    /// A writeable <see cref="Stream"/> to the file.
    /// </returns>
    Stream CreateFile(string fileName);

    /// <summary>
    /// Gets the full path of a file in the temporary directory.
    /// </summary>
    /// <param name="fileName">The name of the file to get the full path for.</param>
    /// <returns>The absolute path to the file.</returns>
    string GetFilePath(string fileName);
}
