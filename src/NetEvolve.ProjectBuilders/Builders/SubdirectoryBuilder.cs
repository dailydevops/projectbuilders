namespace NetEvolve.ProjectBuilders.Builders;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.Arguments;
using NetEvolve.ProjectBuilders.Abstractions;

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
        Argument.ThrowIfNullOrWhiteSpace(directoryName);

        return new SubdirectoryBuilder(_directory.CreateSubdirectory(directoryName));
    }

    /// <inheritdoc cref="ISubdirectoryBuilder.CreateFile(string)" />
    public Stream CreateFile(string fileName)
    {
        Argument.ThrowIfNullOrWhiteSpace(fileName);

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
