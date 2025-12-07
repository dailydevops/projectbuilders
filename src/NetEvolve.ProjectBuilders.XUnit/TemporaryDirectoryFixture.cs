namespace NetEvolve.ProjectBuilders.XUnit;

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Abstractions;
using NetEvolve.ProjectBuilders.Builders;
using Xunit;

/// <summary>
/// Represents a temporary directory fixture that is automatically created and cleaned up for xUnit tests.
/// </summary>
/// <remarks>
/// <para>
/// This class serves as an xUnit-specific adapter around <see cref="TemporaryDirectoryBuilder"/>
/// to provide seamless integration with xUnit's test lifecycle management through the fixture pattern.
/// It implements <see cref="IAsyncLifetime"/> for automatic initialization and cleanup.
/// </para>
/// <para>
/// The temporary directory is automatically:
/// <list type="bullet">
/// <item><description>Created before test execution via <see cref="IAsyncLifetime.InitializeAsync"/></description></item>
/// <item><description>Cleaned up and deleted after test completion via <see cref="IAsyncDisposable.DisposeAsync"/></description></item>
/// <item><description>Placed in a unique location to avoid conflicts between parallel test executions</description></item>
/// </list>
/// </para>
/// <para>
/// Provides directory and file management methods through <see cref="ITemporaryDirectoryBuilder"/>,
/// including directory creation, file creation, and path resolution.
/// </para>
/// <para>
/// Works with xUnit class fixtures and collection fixtures for flexible test lifecycle management.
/// </para>
/// <para>
/// Class fixture usage example:
/// <code>
/// public class MyTests : IClassFixture&lt;TemporaryDirectoryFixture&gt;
/// {
///     private readonly TemporaryDirectoryFixture _directory;
///
///     public MyTests(TemporaryDirectoryFixture directory)
///     {
///         _directory = directory;
///     }
///
///     [Fact]
///     public void TestCreateFile()
///     {
///         // Directory is automatically initialized before this test runs
///         using var stream = _directory.CreateFile("test.txt");
///         var fullPath = _directory.GetFilePath("test.txt");
///         Assert.True(File.Exists(fullPath));
///         // Directory is automatically cleaned up after this test completes
///     }
/// }
/// </code>
/// </para>
/// <para>
/// Collection fixture usage example:
/// <code>
/// [CollectionDefinition("Temporary Directory Collection")]
/// public class TemporaryDirectoryCollection : ICollectionFixture&lt;TemporaryDirectoryFixture&gt;
/// {
/// }
///
/// [Collection("Temporary Directory Collection")]
/// public class MyTests
/// {
///     private readonly TemporaryDirectoryFixture _directory;
///
///     public MyTests(TemporaryDirectoryFixture directory)
///     {
///         _directory = directory;
///     }
///
///     [Fact]
///     public void TestOne()
///     {
///         // Directory is shared across all tests in the collection
///         using var stream = _directory.CreateFile("test1.txt");
///     }
///
///     [Fact]
///     public void TestTwo()
///     {
///         // Same directory instance from TestOne
///         using var stream = _directory.CreateFile("test2.txt");
///     }
/// }
/// </code>
/// </para>
/// </remarks>
/// <seealso cref="IClassFixture{TFixture}"/>
/// <seealso cref="ICollectionFixture{TFixture}"/>
/// <seealso cref="IAsyncLifetime"/>
/// <seealso cref="TemporaryDirectoryBuilder"/>
public sealed class TemporaryDirectoryFixture : ITemporaryDirectoryBuilder, IAsyncLifetime
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
    async ValueTask IAsyncLifetime.InitializeAsync() => await _directory.CreateAsync().ConfigureAwait(false);

    /// <inheritdoc cref="IAsyncDisposable.DisposeAsync" />
    async ValueTask IAsyncDisposable.DisposeAsync() => await DisposeAsync().ConfigureAwait(false);
}
