namespace NetEvolve.ProjectBuilders.Builders;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.Arguments;
using NetEvolve.ProjectBuilders.Abstractions;

/// <summary>
/// Manages subdirectories and file creation within a directory hierarchy.
/// </summary>
/// <remarks>
/// <para>
/// This internal class implements <see cref="ISubdirectoryBuilder"/> to provide file system
/// operations for creating and managing subdirectories and files during project building.
/// </para>
/// <para>
/// Each instance wraps a <see cref="DirectoryInfo"/> and provides methods to:
/// <list type="bullet">
/// <item><description>Create nested subdirectories</description></item>
/// <item><description>Create new files with stream access</description></item>
/// <item><description>Query file paths</description></item>
/// </list>
/// </para>
/// <para>
/// Unlike <see cref="TemporaryDirectoryBuilder"/>, this class does not provide automatic cleanup.
/// It is typically used to represent subdirectories within a temporary directory that will be
/// cleaned up by its parent.
/// </para>
/// </remarks>
/// <seealso cref="ISubdirectoryBuilder"/>
/// <seealso cref="TemporaryDirectoryBuilder"/>
/// <inheritdoc cref="ISubdirectoryBuilder" />
internal sealed class SubdirectoryBuilder : ISubdirectoryBuilder
{
    private readonly DirectoryInfo _directory;

    /// <inheritdoc cref="IObjectBuilder.FullPath" />
    public string FullPath => _directory.FullName;

    internal SubdirectoryBuilder(DirectoryInfo directory) => _directory = directory;

    /// <inheritdoc cref="IObjectBuilder.CreateAsync(CancellationToken)"/>
    public ValueTask CreateAsync(CancellationToken cancellationToken = default) => ValueTask.CompletedTask;

    /// <inheritdoc cref="ISubdirectoryBuilder.CreateDirectory(string)" />
    public ISubdirectoryBuilder CreateDirectory(string directoryName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(directoryName);

        return new SubdirectoryBuilder(_directory.CreateSubdirectory(directoryName));
    }

    /// <inheritdoc cref="ISubdirectoryBuilder.CreateFile(string)" />
    public Stream CreateFile(string fileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        var fileInfo = new FileInfo(Path.Combine(_directory.FullName, fileName));

        if (fileInfo.Exists)
        {
            throw new ArgumentException($"File with name `{fileName}` already exists.", nameof(fileName));
        }

        return fileInfo.Create();
    }

    /// <inheritdoc cref="IAsyncDisposable.DisposeAsync" />
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    /// <inheritdoc cref="ISubdirectoryBuilder.GetFilePath(string)" />
    public string GetFilePath(string fileName) => Path.Combine(FullPath, fileName);
}
