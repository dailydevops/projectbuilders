namespace NetEvolve.ProjectBuilders.Builders;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.Arguments;
using NetEvolve.ProjectBuilders.Abstractions;

/// <inheritdoc cref="ITempDirectoryBuilder" />
internal sealed class TempDirectoryBuilder : ITempDirectoryBuilder
{
    private readonly DirectoryInfo _directory;
    private bool _disposedValue;

    /// <inheritdoc cref="IObjectBuilder.FullPath" />
    public string FullPath => _directory.FullName;

    /// <inheritdoc cref="ISubdirectoryBuilder.CreateDirectory(string)" />
    public ISubdirectoryBuilder CreateDirectory(string directoryName)
    {
        Argument.ThrowIfNullOrWhiteSpace(directoryName);

        return new SubdirectoryBuilder(_directory.CreateSubdirectory(directoryName));
    }

    /// <inheritdoc cref="ISubdirectoryBuilder.CreateFile(string)" />
    public Stream CreateFile(string fileName)
    {
        Argument.ThrowIfNullOrWhiteSpace(fileName);

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
        Argument.ThrowIfNullOrWhiteSpace(fileName);
        return Path.Combine(FullPath, fileName);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TempDirectoryBuilder"/> class.
    /// </summary>
    public TempDirectoryBuilder()
    {
        var directoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        _directory = new DirectoryInfo(directoryPath);
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
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
        catch (Exception)
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
