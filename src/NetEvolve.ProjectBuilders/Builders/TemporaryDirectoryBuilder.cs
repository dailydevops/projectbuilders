namespace NetEvolve.ProjectBuilders.Builders;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.Arguments;
using NetEvolve.ProjectBuilders.Abstractions;

/// <summary>
/// Creates and manages temporary directories with automatic cleanup.
/// </summary>
/// <remarks>
/// <para>
/// This internal class implements <see cref="ITemporaryDirectoryBuilder"/> to provide isolated,
/// auto-cleaning temporary directories for project building and testing. Each instance creates
/// a unique directory in the system's temporary folder.
/// </para>
/// <para>
/// The builder:
/// <list type="bullet">
/// <item><description>Creates a unique directory using <see cref="Guid"/> for isolation</description></item>
/// <item><description>Provides file and subdirectory creation capabilities</description></item>
/// <item><description>Automatically deletes the entire directory hierarchy when disposed</description></item>
/// <item><description>Handles exceptions during cleanup gracefully</description></item>
/// </list>
/// </para>
/// <para>
/// The automatic cleanup ensures that test artifacts don't pollute the file system, even if exceptions occur
/// during the build process. This is essential for maintaining clean test environments.
/// </para>
/// </remarks>
/// <seealso cref="ITemporaryDirectoryBuilder"/>
/// <seealso cref="SubdirectoryBuilder"/>
/// <inheritdoc cref="ITemporaryDirectoryBuilder" />
internal sealed class TemporaryDirectoryBuilder : ITemporaryDirectoryBuilder
{
    private readonly DirectoryInfo _directory;
    private bool _disposedValue;

    /// <inheritdoc cref="IObjectBuilder.FullPath" />
    public string FullPath => _directory.FullName;

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

        if (!_directory.Exists)
        {
            _directory.Create();
        }

        var fileInfo = new FileInfo(Path.Combine(_directory.FullName, fileName));

        if (fileInfo.Exists)
        {
            throw new ArgumentException($"File with name `{fileName}` already exists.", nameof(fileName));
        }

        return fileInfo.Create();
    }

    /// <inheritdoc cref="ISubdirectoryBuilder.GetFilePath(string)" />
    public string GetFilePath(string fileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        return Path.Combine(FullPath, fileName);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TemporaryDirectoryBuilder"/> class.
    /// </summary>
    public TemporaryDirectoryBuilder()
    {
        var directoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        _directory = new DirectoryInfo(directoryPath);
    }

    /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
    public ValueTask DisposeAsync() => DisposeAsync(true);

    private async ValueTask DisposeAsync(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                await DeleteDiretoryAsync(_directory).ConfigureAwait(false);
            }

            _disposedValue = true;
        }
    }

    private static async ValueTask DeleteDiretoryAsync(DirectoryInfo directory)
    {
        try
        {
            await Parallel
                .ForEachAsync(
                    directory.EnumerateDirectories("*", SearchOption.TopDirectoryOnly),
                    async (dir, _) => await DeleteDiretoryAsync(dir).ConfigureAwait(false)
                )
                .ConfigureAwait(false);

            directory.Delete(true);
        }
        catch
        {
            // Ignore, because we are using this while testing.
        }
    }

    /// <inheritdoc cref="IObjectBuilder.CreateAsync(CancellationToken)"/>
    public ValueTask CreateAsync(CancellationToken cancellationToken = default)
    {
        if (!_directory.Exists)
        {
            _directory.Create();
        }

        return ValueTask.CompletedTask;
    }
}
