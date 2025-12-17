namespace NetEvolve.ProjectBuilders.Builders;

using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.Arguments;
using NetEvolve.ProjectBuilders.Abstractions;
using NetEvolve.ProjectBuilders.Models;

/// <summary>
/// Builds global.json SDK configuration files with fluent API support.
/// </summary>
/// <remarks>
/// <para>
/// This class implements <see cref="IGlobalJsonBuilder"/> to provide a convenient way to create
/// global.json configuration files that specify SDK versions and roll-forward policies for
/// .NET projects.
/// </para>
/// <para>
/// The generated global.json file follows the .NET SDK global.json format and includes:
/// <list type="bullet">
/// <item><description>SDK version specification</description></item>
/// <item><description>Prerelease version allowance configuration</description></item>
/// <item><description>Roll-forward policy selection</description></item>
/// </list>
/// </para>
/// <para>
/// See <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/global-json"/> for detailed information
/// on global.json file format and options.
/// </para>
/// </remarks>
/// <seealso cref="IGlobalJsonBuilder"/>
/// <inheritdoc cref="IGlobalJsonBuilder" />
internal sealed class GlobalJsonBuilder : IGlobalJsonBuilder
{
    private const string FileName = "global.json";
    private readonly ISubdirectoryBuilder _directory;

    private bool? _allowPrerelease;
    private RollForward? _rollForward;
    private string _runtimeVersion;

    public string FullPath => Path.Combine(_directory.FullPath, FileName);

    /// <inheritdoc cref="IGlobalJsonBuilder.SetAllowPrerelease(bool)" />
    public IGlobalJsonBuilder SetAllowPrerelease(bool allowPrerelease)
    {
        _allowPrerelease = allowPrerelease;

        return this;
    }

    /// <inheritdoc cref="IGlobalJsonBuilder.SetRollForward(RollForward)" />
    public IGlobalJsonBuilder SetRollForward(RollForward rollForward)
    {
        _rollForward = rollForward;

        return this;
    }

    /// <inheritdoc cref="IGlobalJsonBuilder.SetRuntimeSdk(string)" />
    public IGlobalJsonBuilder SetRuntimeSdk(string runtimeVersion)
    {
        Argument.ThrowIfNullOrWhiteSpace(runtimeVersion);

        _runtimeVersion = runtimeVersion;

        return this;
    }

    internal GlobalJsonBuilder(ISubdirectoryBuilder directory, string runtimeVersion)
    {
        _directory = directory;
        _runtimeVersion = runtimeVersion;
    }

    public async ValueTask CreateAsync(CancellationToken cancellationToken = default)
    {
        var document = CreateDocument();

        var file = _directory.CreateFile(FileName);
        await using (file.ConfigureAwait(false))
        {
            await JsonSerializer
                .SerializeAsync(file, document, Constants.JsonSettings, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    private JsonObject CreateDocument()
    {
        // https://learn.microsoft.com/en-us/dotnet/core/tools/global-json
        var sdk = new JsonObject { ["version"] = _runtimeVersion };

        if (_allowPrerelease.HasValue)
        {
            sdk["allowPrerelease"] = _allowPrerelease.Value;
        }

        var rollForward = RollForwardToString(_rollForward);

        if (rollForward is not null)
        {
            sdk["rollForward"] = rollForward;
        }

        var node = new JsonObject { ["sdk"] = sdk };

        return node;
    }

    private static string? RollForwardToString(RollForward? value) =>
        value switch
        {
            RollForward.LatestPatch => "latestPatch",
            RollForward.LatestFeature => "latestFeature",
            RollForward.LatestMinor => "latestMinor",
            RollForward.LatestMajor => "latestMajor",
            RollForward.Disable => "disable",
            _ => null,
        };

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
