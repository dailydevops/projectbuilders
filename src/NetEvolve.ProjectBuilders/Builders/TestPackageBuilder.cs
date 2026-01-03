namespace NetEvolve.ProjectBuilders.Builders;

using CliWrap;
using NetEvolve.Arguments;
using NetEvolve.ProjectBuilders.Abstractions;
using NetEvolve.ProjectBuilders.Helpers;

/// <summary>
/// Builds NuGet packages from project specifications for testing purposes.
/// </summary>
/// <remarks>
/// <para>
/// This internal class implements <see cref="ITestPackageBuilder"/> to create NuGet packages
/// from project files or .nuspec specifications. It uses the nuget.exe tool (or the nuget CLI)
/// to package referenced projects as test packages with a fixed version (999.999.999).
/// </para>
/// <para>
/// The class features:
/// <list type="bullet">
/// <item><description>Thread-safe package creation with semaphore-based locking</description></item>
/// <item><description>Platform-specific tooling (nuget.exe on Windows, nuget CLI on others)</description></item>
/// <item><description>Automatic downloading of nuget.exe if needed</description></item>
/// <item><description>Deduplication of package paths to avoid redundant processing</description></item>
/// </list>
/// </para>
/// </remarks>
/// <seealso cref="ITestPackageBuilder"/>
/// <inheritdoc cref="ITestPackageBuilder" />
internal sealed class TestPackageBuilder : ITestPackageBuilder
{
    private readonly Guid _identifier = Guid.NewGuid();
    private TemporaryDirectoryBuilder? _nugetFolder;
    private readonly ISubdirectoryBuilder _directoy;
    private bool _isInitialized;
    private static readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
    private readonly HashSet<string> _packagePaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

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
        await _lock.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            if (_isInitialized)
            {
                return;
            }

            await _directoy.CreateAsync(cancellationToken).ConfigureAwait(false);
            var cliWrap = await GetCliWrapAsync(cancellationToken).ConfigureAwait(false);

            foreach (var packagePath in _packagePaths)
            {
                string[] args =
                [
                    "pack",
                    packagePath,
                    "-ForceEnglishOutput",
                    "-Version",
                    "999.999.999", // To prevent version conflicts during testing
                    "-OutputDirectory",
                    _directoy.FullPath,
                ];

                _ = await cliWrap.WithArguments(args).ExecuteAsync(cancellationToken).ConfigureAwait(false);
            }

            _isInitialized = true;
        }
        finally
        {
            _ = _lock.Release();
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

    private async ValueTask<Command> GetCliWrapAsync(CancellationToken cancellationToken)
    {
        if (OperatingSystem.IsWindows())
        {
            var exe = await GetNuGetExeAsync(cancellationToken).ConfigureAwait(false);

            return Cli.Wrap(exe!);
        }

        return Cli.Wrap("nuget");
    }

    private async ValueTask<string> GetNuGetExeAsync(CancellationToken cancellationToken)
    {
        if (_nugetFolder is null)
        {
            _nugetFolder = new TemporaryDirectoryBuilder();
            await _nugetFolder.CreateAsync(cancellationToken).ConfigureAwait(false);
        }

        var fileName = $"nuget-{_identifier:N}.exe";
        var filePath = Path.Combine(_nugetFolder.FullPath, fileName);
        if (!File.Exists(filePath))
        {
#pragma warning disable S1075 // URIs should not be hardcoded
            await DownloadNuGetClientAsync(
                    "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe",
                    fileName,
                    cancellationToken
                )
                .ConfigureAwait(false);
#pragma warning restore S1075 // URIs should not be hardcoded
        }

        return filePath;
    }

    private async Task DownloadNuGetClientAsync(string url, string fileName, CancellationToken cancellationToken)
    {
        using var downloadStream = await SharedHttpClient
            .Instance.GetStreamAsync(new Uri(url, UriKind.Absolute), cancellationToken)
            .ConfigureAwait(false);
        using var fileStream = _nugetFolder!.CreateFile(fileName);

        await downloadStream.CopyToAsync(fileStream, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
    public async ValueTask DisposeAsync()
    {
        await _directoy.DisposeAsync().ConfigureAwait(false);
        if (_nugetFolder is not null)
        {
            await _nugetFolder.DisposeAsync().ConfigureAwait(false);
        }
    }
}
