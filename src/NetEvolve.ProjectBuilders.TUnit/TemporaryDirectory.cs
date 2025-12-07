namespace NetEvolve.ProjectBuilders.TUnit;

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using global::TUnit.Core.Interfaces;
using NetEvolve.ProjectBuilders.Abstractions;
using NetEvolve.ProjectBuilders.Builders;

/// <summary>
/// Represents a temporary directory that is automatically created and cleaned up for TUnit tests.
/// This class integrates with TUnit's <see cref="IAsyncInitializer"/> to automatically initialize
/// the temporary directory before test execution and implements <see cref="IAsyncDisposable"/>
/// to ensure proper cleanup after test completion.
/// </summary>
/// <remarks>
/// <para>
/// This class serves as a TUnit-specific wrapper around <see cref="TemporaryDirectoryBuilder"/>
/// to provide seamless integration with TUnit's test lifecycle management.
/// </para>
/// <para>
/// The temporary directory is created in a unique location to avoid conflicts between parallel
/// test executions and is automatically deleted when disposed, ensuring clean test isolation.
/// </para>
/// <para>
/// Usage in TUnit tests:
/// <code>
/// [ClassDataSource&lt;TemporaryDirectory&gt;]
/// public class MyTests(TemporaryDirectory directory)
/// {
///     [Test]
///     public async Task MyTest()
///     {
///         var filePath = directory.GetFilePath("test.txt");
///         using var stream = directory.CreateFile("test.txt");
///         // ... test code ...
///     }
/// }
/// </code>
/// </para>
/// </remarks>
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
