namespace NetEvolve.ProjectBuilders.Abstractions;

using NetEvolve.ProjectBuilders.Models;

/// <summary>
/// Represents a global.json file builder.
/// </summary>
public interface IGlobalJsonBuilder : IFileBuilder
{
    /// <summary>
    /// If set to <see langword="true" />, the global.json file will allow prerelease versions.
    /// </summary>
    /// <param name="allowPrerelease">
    /// If set to <see langword="true" />, the global.json file will allow prerelease versions.
    /// </param>
    /// <returns>The current instance of the <see cref="IGlobalJsonBuilder"/>.</returns>
    IGlobalJsonBuilder SetAllowPrerelease(bool allowPrerelease);

    /// <summary>
    /// Sets the roll forward option for the global.json file.
    /// </summary>
    /// <param name="rollForward">
    /// The roll forward option for the global.json file.
    /// </param>
    /// <returns>The current instance of the <see cref="IGlobalJsonBuilder"/>.</returns>
    IGlobalJsonBuilder SetRollForward(RollForward rollForward);

    IGlobalJsonBuilder SetRuntimeSdk(string runtimeVersion);
}
