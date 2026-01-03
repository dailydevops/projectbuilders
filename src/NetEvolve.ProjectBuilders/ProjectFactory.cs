namespace NetEvolve.ProjectBuilders;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CliWrap;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NetEvolve.Arguments;
using NetEvolve.ProjectBuilders.Abstractions;
using NetEvolve.ProjectBuilders.Builders;
using NetEvolve.ProjectBuilders.Models.Output;

/// <summary>
/// Provides fluent API to create, configure, and build .NET projects for testing purposes.
/// </summary>
/// <remarks>
/// <para>
/// This class is the primary entry point for the ProjectBuilders library. It implements the
/// fluent API pattern to allow convenient creation of test projects with various configurations,
/// such as C#, VB.NET, and global.json files.
/// </para>
/// <para>
/// The factory manages the complete lifecycle of test project creation, including:
/// <list type="bullet">
/// <item><description>Creating temporary directories for isolated testing</description></item>
/// <item><description>Building project files with custom properties and item groups</description></item>
/// <item><description>Executing dotnet build and restore commands</description></item>
/// <item><description>Capturing and analyzing SARIF diagnostic output</description></item>
/// <item><description>Managing environment variables for build execution</description></item>
/// </list>
/// </para>
/// <para>
/// Usage example:
/// <code>
/// using var factory = ProjectFactory.Create();
/// factory
///     .AddCSharpProject(pb => pb.WithDefaults())
///     .AddGlobalJson(configure: gb => gb.WithDefaults());
/// var result = await factory.BuildAsync();
/// </code>
/// </para>
/// </remarks>
/// <seealso cref="IProjectFactory"/>
/// <inheritdoc cref="IProjectFactory" />
public sealed partial class ProjectFactory : IProjectFactory
{
    /* language=regex */
    private const string RulesFilterPattern = @"(NU|NEP)\d{4}";

    private readonly ILogger<ProjectFactory> _logger;
    private readonly ITestPackageBuilder? _testPackageBuilder;
    private readonly ISubdirectoryBuilder _tempDirectory;
    private bool _disposedValue;
    private readonly List<string> _output;

    internal Dictionary<string, string?> EnvironmentVariables { get; init; }
    internal HashSet<IObjectBuilder> ObjectBuilders { get; init; }

    /// <summary>
    /// Gets the directory builder for the temporary directory used by this factory.
    /// </summary>
    /// <remarks>
    /// This directory serves as the root for all project files, subdirectories, and outputs
    /// created during the test project building process.
    /// </remarks>
    /// <inheritdoc cref="IProjectFactory.DirectoryBuilder" />
    public ISubdirectoryBuilder DirectoryBuilder => _tempDirectory;

    private static readonly string[] _newLineSeparator = ["\r\n", "\r", "\n"];

    internal ProjectFactory(
        ITestPackageBuilder? testPackageBuilder,
        ISubdirectoryBuilder? directory,
        ILogger<ProjectFactory>? logger
    )
    {
        _logger = logger ?? NullLogger<ProjectFactory>.Instance;
        _testPackageBuilder = testPackageBuilder;
        _tempDirectory = directory ?? new TemporaryDirectoryBuilder();
        _output = [];

        EnvironmentVariables = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase)
        {
            { "CI", null },
            { "DOTNET_CLI_TELEMETRY_OPTOUT", "true" },
            { "DOTNET_NOLOGO", "true" },
            { "DOTNET_SKIP_FIRST_TIME_EXPERIENCE", "true" },
            { "GITHUB_ACTIONS", null },
        };
        ObjectBuilders = [];
    }

    /// <summary>
    /// Creates a new instance of <see cref="IProjectFactory"/>. Use the fluent API to build a project.
    /// </summary>
    /// <param name="testPackageBuilder">
    /// The test package builder to use.
    /// </param>
    /// <param name="directory">
    /// The directory builder to use.
    /// </param>
    /// <param name="logger">
    /// The logger to use. If not provided, a <see cref="NullLogger{T}"/> will be used.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="IProjectFactory"/>.
    /// </returns>
    public static IProjectFactory Create(
        ITestPackageBuilder? testPackageBuilder = null,
        ISubdirectoryBuilder? directory = null,
        ILogger<ProjectFactory>? logger = null
    ) => new ProjectFactory(testPackageBuilder, directory, logger);

    /// <inheritdoc cref="IProjectFactory.AddEnvironmentVariable"/>
    public IProjectFactory AddEnvironmentVariable(string name, string? value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        if (!EnvironmentVariables.TryAdd(name, value))
        {
            EnvironmentVariables[name] = value;
        }

        return this;
    }

    /// <inheritdoc cref="IProjectFactory.AddEnvironmentVariables"/>
    public IProjectFactory AddEnvironmentVariables(params KeyValuePair<string, string?>[] variables)
    {
        ArgumentNullException.ThrowIfNull(variables);

        foreach (var variable in variables)
        {
            _ = AddEnvironmentVariable(variable.Key, variable.Value);
        }

        return this;
    }

    /// <inheritdoc cref="IProjectFactory.AddFileBuilder{TInterface, TImplementation}(TImplementation)"/>
    public TInterface AddFileBuilder<TInterface, TImplementation>(TImplementation builder)
        where TInterface : IFileBuilder
        where TImplementation : class, TInterface
    {
        ArgumentNullException.ThrowIfNull(builder);

        if (!ObjectBuilders.Add(builder))
        {
            throw new ArgumentException("`builder` already registered.", nameof(builder));
        }

        return builder;
    }

    /// <inheritdoc cref="IProjectFactory.BuildAsync"/>
    public async ValueTask<OutputFile> BuildAsync(string[]? args = null, CancellationToken cancellationToken = default)
    {
        if (ObjectBuilders.Count == 0)
        {
            throw new ArgumentException("No file builders were added.");
        }

        if (!ObjectBuilders.OfType<ProjectBuilder>().Any())
        {
            throw new ArgumentException("No project builder were added.");
        }

        // Create temporary directory for testing
        await _tempDirectory.CreateAsync(cancellationToken).ConfigureAwait(false);

        if (_testPackageBuilder is not null)
        {
            var lookupPaths = ObjectBuilders
                .OfType<ProjectBuilder>()
                .Select(x => x.ItemGroup.Items)
                .OfType<IReference>()
                .SelectMany(x => x.LookUpPaths)
                .Distinct()
                .ToArray();

            if (lookupPaths.Length > 0)
            {
                _testPackageBuilder.SetPackagePaths(lookupPaths);

                // Prepare nuget packages for testings
                await _testPackageBuilder.CreateAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        await Parallel
            .ForEachAsync(
                ObjectBuilders,
                cancellationToken,
                async (file, token) => await file.CreateAsync(token).ConfigureAwait(false)
            )
            .ConfigureAwait(false);

        var result = await ExecuteDotNetCommandAsync([Constants.CommandRestore, "-v", "quiet"], cancellationToken)
            .ConfigureAwait(false);

        LogProcessExitCode(Constants.CommandRestore, result.ExitCode);

        result = await ExecuteDotNetCommandAsync([Constants.CommandBuild, .. (args ?? [])], cancellationToken)
            .ConfigureAwait(false);

        LogProcessExitCode(Constants.CommandBuild, result.ExitCode);

        var sarifBytes = await File.ReadAllBytesAsync(
                _tempDirectory.GetFilePath(Constants.OutputFileName),
                cancellationToken
            )
            .ConfigureAwait(false);

        var sarif = JsonSerializer.Deserialize<OutputFile>(sarifBytes)!;

        EnrichSarifResults(sarif);

        if (!sarif.HasNoErrorsOrWarnings())
        {
            LogSarifResult(string.Join('\n', sarif.Results.Select(x => x.ToString())));
        }

        return sarif;
    }

    private async Task<CommandResult> ExecuteDotNetCommandAsync(
        IEnumerable<string> args,
        CancellationToken cancellationToken
    ) =>
        await Cli.Wrap("dotnet")
            .WithWorkingDirectory(_tempDirectory.FullPath)
            .WithArguments(args)
            .WithEnvironmentVariables(env =>
            {
                if (EnvironmentVariables.Count == 0)
                {
                    return;
                }

                LogEnvironmentVariables();

                foreach (var kvp in EnvironmentVariables)
                {
                    LogEnvironmentVariable(kvp.Key, kvp.Value);
                    _ = env.Set(kvp.Key, kvp.Value);
                }
            })
            .WithStandardOutputPipe(PipeTarget.ToDelegate(TrackStandardOutput))
            .WithStandardErrorPipe(PipeTarget.ToDelegate(TrackErrorOutput))
            .WithValidation(CommandResultValidation.None)
            .ExecuteAsync(cancellationToken);

    /// <summary>
    /// Regular expression pattern used to extract diagnostic rule IDs from build output.
    /// Matches patterns like "NU1234" or "NEP5678".
    /// </summary>
    /// <remarks>
    /// The pattern matches NuGet (NU) or NetEvolve.ProjectBuilders (NEP) diagnostic codes
    /// followed by a four-digit number. This is used to filter and enrich SARIF output
    /// with diagnostic information extracted from the dotnet build command output.
    /// </remarks>
#if NET8_0_OR_GREATER
    [GeneratedRegex(RulesFilterPattern, RegexOptions.Compiled)]
    public static partial Regex RulesFilter();
#else
    public static Regex RulesFilter() => _rulesFilter;

    private static readonly Regex _rulesFilter = new Regex(RulesFilterPattern, RegexOptions.Compiled);
#endif

    private void EnrichSarifResults(OutputFile? sarif)
    {
        if (_output.Count == 0 || sarif is null)
        {
            return;
        }

        var outputLines = _output
            .SelectMany(x =>
                x.Split(_newLineSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            )
            .ToArray();
        var filteredResults = new List<OutputRunResult>();

        foreach (var outputLine in outputLines)
        {
            var match = RulesFilter().Match(outputLine);
            if (!match.Success)
            {
                continue;
            }

            var ruleId = match.Value.TrimEnd(':');
            var previousColonIndex = outputLine.LastIndexOf(':', match.Index);
            var ruleLevel = outputLine.Substring(previousColonIndex + 1, match.Index - previousColonIndex - 1).Trim();

            var message = outputLine[(match.Index + match.Length + 1)..];

            filteredResults.Add(
                new OutputRunResult
                {
                    RuleId = ruleId,
                    Level = ruleLevel,
                    Message = new OutputRunResultMessage { Text = message },
                }
            );
        }

        var distinctRules = filteredResults.DistinctBy(x => new { x.RuleId, x.Level }).ToList();
        sarif.Runs.Add(new OutputRun { Results = distinctRules });
    }

    /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
    public ValueTask DisposeAsync() => DisposeAsync(disposing: true);

    private async ValueTask DisposeAsync(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                await Parallel
                    .ForEachAsync(ObjectBuilders, async (file, _) => await file.DisposeAsync().ConfigureAwait(false))
                    .ConfigureAwait(false);
            }

            _disposedValue = true;
        }
    }

    private void TrackStandardOutput(string message)
    {
        LogStandardOutput(message);
        _output.Add(message);
    }

    private void TrackErrorOutput(string message)
    {
        LogErrorOutput(message);
        _output.Add(message);
    }

    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "{Message}")]
    private partial void LogStandardOutput(string message);

    [LoggerMessage(EventId = 2, Level = LogLevel.Error, Message = "{Message}")]
    private partial void LogErrorOutput(string message);

    [LoggerMessage(EventId = 3, Level = LogLevel.Information, Message = "Process `{Name}` exit code: {ExitCode}")]
    private partial void LogProcessExitCode(string name, int exitCode);

    [LoggerMessage(EventId = 4, Level = LogLevel.Information, Message = "Result:\n{result}")]
    private partial void LogSarifResult(string result);

    [LoggerMessage(EventId = 5, Level = LogLevel.Debug, Message = "Environment Variables:")]
    private partial void LogEnvironmentVariables();

    [LoggerMessage(EventId = 6, Level = LogLevel.Debug, Message = "  `{Key}`: `{Value}`")]
    private partial void LogEnvironmentVariable(string key, string? value);
}
