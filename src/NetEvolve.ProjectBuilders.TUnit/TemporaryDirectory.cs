namespace NetEvolve.ProjectBuilders;

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Abstractions;
using NetEvolve.ProjectBuilders.Builders;
using TUnit.Core.Interfaces;

/// <summary>
/// Represents a temporary directory that is automatically created and cleaned up for TUnit tests.
/// </summary>
/// <remarks>
/// <para>
/// This class serves as a TUnit-specific adapter around <see cref="TemporaryDirectoryBuilder"/>
/// to provide seamless integration with TUnit's test lifecycle management. It implements both
/// <see cref="IAsyncInitializer"/> for automatic initialization and <see cref="IAsyncDisposable"/>
/// for automatic cleanup.
/// </para>
/// <para>
/// The temporary directory is automatically:
/// <list type="bullet">
/// <item><description>Created before test execution via <see cref="IAsyncInitializer.InitializeAsync"/></description></item>
/// <item><description>Cleaned up and deleted after test completion via <see cref="DisposeAsync"/></description></item>
/// <item><description>Placed in a unique location to avoid conflicts between parallel test executions</description></item>
/// </list>
/// </para>
/// <para>
/// Provides directory and file management methods through <see cref="ITemporaryDirectoryBuilder"/>,
/// including directory creation, file creation, and path resolution.
/// </para>
/// <para>
/// Usage example in TUnit tests:
/// <code>
/// [ClassDataSource&lt;TemporaryDirectory&gt;]
/// public class MyTests(TemporaryDirectory directory)
/// {
///     [Test]
///     public async Task TestCreateFile()
///     {
///         using var stream = directory.CreateFile("test.txt");
///         stream.WriteAsync(new byte[] { 1, 2, 3 }, 0, 3);
///
///         string fullPath = directory.GetFilePath("test.txt");
///         Assert.That(File.Exists(fullPath));
///     }
///
///     [Test]
///     public void TestCreateDirectory()
///     {
///         var subDir = directory.CreateDirectory("subdirectory");
///         Assert.That(Directory.Exists(Path.Combine(directory.FullPath, "subdirectory")));
///     }
/// }
/// </code>
/// </para>
/// </remarks>
/// <seealso cref="IAsyncInitializer"/>
/// <seealso cref="IAsyncDisposable"/>
/// <seealso cref="TemporaryDirectoryBuilder"/>
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
