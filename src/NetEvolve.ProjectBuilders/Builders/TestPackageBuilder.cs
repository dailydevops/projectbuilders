namespace NetEvolve.ProjectBuilders.Builders;

using CliWrap;
using NetEvolve.Arguments;
using NetEvolve.ProjectBuilders.Abstractions;

/// <summary>
/// Builds NuGet packages from project specifications for testing purposes.
/// </summary>
/// <remarks>
/// <para>
/// This internal class implements <see cref="ITestPackageBuilder"/> to create NuGet packages
/// from referenced project files. It uses <c>dotnet pack</c> to package referenced projects as
/// test packages with a fixed version (999.999.999).
/// </para>
/// <para>
/// The class features:
/// <list type="bullet">
/// <item><description>Thread-safe package creation with semaphore-based locking</description></item>
/// <item><description>Deduplication of package paths to avoid redundant processing</description></item>
/// </list>
/// </para>
/// </remarks>
/// <seealso cref="ITestPackageBuilder"/>
/// <inheritdoc cref="ITestPackageBuilder" />
internal sealed class TestPackageBuilder : ITestPackageBuilder
{
    private readonly ISubdirectoryBuilder _directoy;
    internal bool _isInitialized;
    private static readonly SemaphoreSlim Lock = new SemaphoreSlim(1, 1);
    internal readonly HashSet<string> _packagePaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

    /// <inheritdoc cref="IObjectBuilder.FullPath"/>
    public string FullPath => _directoy.FullPath;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestPackageBuilder"/> class.
    /// </summary>
    /// <param name="directory">
    /// The output directory where created NuGet packages will be stored.
    /// </param>
    internal TestPackageBuilder(ISubdirectoryBuilder directory) => _directoy = directory;

    /// <inheritdoc cref="IObjectBuilder.CreateAsync(CancellationToken)"/>
    public async ValueTask CreateAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await Lock.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            if (_isInitialized)
            {
                return;
            }

            await _directoy.CreateAsync(cancellationToken).ConfigureAwait(false);

            foreach (var packagePath in _packagePaths.Where(File.Exists))
            {
                // nuget.exe pack does not support SDK-style projects (NU5049); dotnet pack does,
                // restoring and building the referenced project as part of packing it.
                string[] args =
                [
                    "pack",
                    packagePath,
                    "-c",
                    "Debug",
                    "-o",
                    _directoy.FullPath,
                    "-p:PackageVersion=999.999.999", // To prevent version conflicts during testing
                ];

                _ = await Cli.Wrap("dotnet").WithArguments(args).ExecuteAsync(cancellationToken).ConfigureAwait(false);
            }

            _isInitialized = true;
        }
        finally
        {
            _ = Lock.Release();
        }
    }

    public void SetPackagePaths(string[] packagePaths)
    {
        ArgumentNullException.ThrowIfNull(packagePaths);

        foreach (var packagePath in packagePaths.Where(x => !string.IsNullOrWhiteSpace(x)))
        {
            _ = _packagePaths.Add(packagePath);
        }
    }

    /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
    public ValueTask DisposeAsync() => _directoy.DisposeAsync();
}
