namespace NetEvolve.ProjectBuilders.Abstractions;

using System.IO;

/// <summary>
/// Represents a builder for creating and managing subdirectories and files within a directory hierarchy.
/// </summary>
/// <remarks>
/// <para>
/// This interface extends <see cref="IObjectBuilder"/> to provide methods for creating subdirectories
/// and files within a parent directory. It manages a portion of the file system hierarchy used by
/// the project building process.
/// </para>
/// <para>
/// Implementations support nested directory creation, file creation with stream access, and path queries
/// to help manage the directory structure during test project building.
/// </para>
/// </remarks>
/// <seealso cref="IObjectBuilder"/>
/// <seealso cref="ITemporaryDirectoryBuilder"/>
public interface ISubdirectoryBuilder : IObjectBuilder
{
    /// <summary>
    /// Creates a subdirectory within this directory.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method creates a new subdirectory hierarchy under the current directory and returns
    /// a builder for that subdirectory, allowing for nested directory creation and manipulation.
    /// </para>
    /// <para>
    /// The method supports fluent chaining to create multiple nested levels or perform operations
    /// on the created subdirectory.
    /// </para>
    /// </remarks>
    /// <param name="directoryName">
    /// The name of the subdirectory to create. Must not be <see langword="null"/>, empty, or contain only whitespace.
    /// Can be a simple name (e.g., "subdir") or a relative path (e.g., "parent/child").
    /// </param>
    /// <returns>
    /// An <see cref="ISubdirectoryBuilder"/> for the newly created subdirectory, supporting fluent chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="directoryName"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="directoryName"/> is empty, contains only whitespace characters,
    /// or the subdirectory already exists.
    /// </exception>
    ISubdirectoryBuilder CreateDirectory(string directoryName);

    /// <summary>
    /// Creates a file in this directory.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method creates a new file in the directory and returns a writeable stream for writing content.
    /// The stream must be disposed after writing to ensure proper file closure and flushing.
    /// </para>
    /// <para>
    /// If a file with the same name already exists, an exception is thrown to prevent accidental overwriting.
    /// </para>
    /// </remarks>
    /// <param name="fileName">The name of the file to create. Must not be <see langword="null"/> or empty.</param>
    /// <returns>
    /// A writeable <see cref="Stream"/> to the newly created file. The caller is responsible for
    /// disposing this stream after writing.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when a file with the specified name already exists in this directory.
    /// </exception>
    Stream CreateFile(string fileName);

    /// <summary>
    /// Gets the full path of a file in this directory.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method constructs the absolute path to a file without creating it. It's useful for
    /// reading or referencing files after they've been created.
    /// </para>
    /// <para>
    /// The returned path is absolute and suitable for use with standard File I/O operations.
    /// </para>
    /// </remarks>
    /// <param name="fileName">The name of the file to get the path for. Must not be <see langword="null"/> or empty.</param>
    /// <returns>The absolute path to the file.</returns>
    string GetFilePath(string fileName);
}
