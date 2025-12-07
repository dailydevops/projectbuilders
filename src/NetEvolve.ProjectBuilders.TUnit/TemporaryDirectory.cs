namespace NetEvolve.ProjectBuilders.TUnit;

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using global::TUnit.Core.Interfaces;
using NetEvolve.ProjectBuilders.Abstractions;
using NetEvolve.ProjectBuilders.Builders;

public sealed class TemporaryDirectory : ITemporaryDirectoryBuilder, IAsyncInitializer
{
    private readonly TemporaryDirectoryBuilder _directory = new();

    /// <inheritdoc cref="IObjectBuilder.FullPath"/>
    public string FullPath => _directory.FullPath;

    /// <inheritdoc cref="IObjectBuilder.CreateAsync(CancellationToken)"/>
    public ValueTask CreateAsync(CancellationToken cancellationToken = default) =>
        _directory.CreateAsync(cancellationToken);

    /// <inheritdoc cref="ISubdirectoryBuilder.CreateDirectory(string)"/>
    public ISubdirectoryBuilder CreateDirectory(string directoryName) => _directory.CreateDirectory(directoryName);

    /// <inheritdoc cref="ISubdirectoryBuilder.CreateFile(string)"/>
    public Stream CreateFile(string fileName) => _directory.CreateFile(fileName);

    /// <inheritdoc/>
    public async Task DisposeAsync() => await _directory.DisposeAsync().ConfigureAwait(false);

    /// <inheritdoc cref="ISubdirectoryBuilder.GetFilePath(string)"/>
    public string GetFilePath(string fileName) => _directory.GetFilePath(fileName);

    /// <inheritdoc/>
    async Task IAsyncInitializer.InitializeAsync() => await _directory.CreateAsync().ConfigureAwait(false);

    /// <inheritdoc cref="IAsyncDisposable.DisposeAsync" />
    async ValueTask IAsyncDisposable.DisposeAsync() => await DisposeAsync().ConfigureAwait(false);
}
