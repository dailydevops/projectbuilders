namespace NetEvolve.ProjectBuilders.XUnit;

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Abstractions;
using NetEvolve.ProjectBuilders.Builders;
using Xunit;

/// <summary>
/// Represents a temporary directory fixture that is automatically created and cleaned up for xUnit tests.
/// This class integrates with xUnit's <see cref="IAsyncLifetime"/> to automatically initialize
/// the temporary directory before test execution and implements <see cref="IAsyncDisposable"/>
/// to ensure proper cleanup after test completion.
/// </summary>
/// <remarks>
/// <para>
/// This class serves as an xUnit-specific wrapper around <see cref="TemporaryDirectoryBuilder"/>
/// to provide seamless integration with xUnit's test lifecycle management through the fixture pattern.
/// </para>
/// <para>
/// The temporary directory is created in a unique location to avoid conflicts between parallel
/// test executions and is automatically deleted when disposed, ensuring clean test isolation.
/// </para>
/// <para>
/// Usage in xUnit tests with class fixtures:
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
///     public void MyTest()
///     {
///         // Directory is automatically initialized via IAsyncLifetime
///         var filePath = _directory.GetFilePath("test.txt");
///         using var stream = _directory.CreateFile("test.txt");
///         // ... test code ...
///
///         // Directory is automatically cleaned up after all tests in the class complete
///     }
/// }
/// </code>
/// </para>
/// <para>
/// Usage in xUnit tests with collection fixtures:
/// <code>
/// [CollectionDefinition("Temporary Directory")]
/// public class TemporaryDirectoryCollection : ICollectionFixture&lt;TemporaryDirectoryFixture&gt;
/// {
/// }
///
/// [Collection("Temporary Directory")]
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
///     public void MyTest()
///     {
///         // Directory is shared across all tests in the collection
///         var filePath = _directory.GetFilePath("test.txt");
///         // ... test code ...
///     }
/// }
/// </code>
/// </para>
/// </remarks>
/// <seealso cref="IClassFixture{TFixture}"/>
/// <seealso cref="ICollectionFixture{TFixture}"/>
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
