namespace NetEvolve.ProjectBuilders.Abstractions;

using NetEvolve.ProjectBuilders.Models;

/// <summary>
/// Represents a builder for creating and configuring global.json SDK configuration files.
/// </summary>
/// <remarks>
/// <para>
/// This interface provides a fluent API for building global.json files that configure
/// the .NET SDK version and roll-forward behavior for a project or directory tree.
/// </para>
/// <para>
/// The global.json file is used to:
/// <list type="bullet">
/// <item><description>Pin a specific .NET SDK version for the project</description></item>
/// <item><description>Configure SDK roll-forward policies</description></item>
/// <item><description>Allow or disallow prerelease SDK versions</description></item>
/// </list>
/// </para>
/// <para>
/// See <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/global-json"/> for more information.
/// </para>
/// </remarks>
/// <seealso cref="IFileBuilder"/>
public interface IGlobalJsonBuilder : IFileBuilder
{
    /// <summary>
    /// Configures whether prerelease SDK versions are allowed for this project.
    /// </summary>
    /// <remarks>
    /// When set to <see langword="true"/>, the .NET SDK resolution will consider prerelease
    /// versions that match the specified major, minor, and patch versions. The default is
    /// <see langword="false"/>, which restricts to released SDK versions only.
    /// </remarks>
    /// <param name="allowPrerelease">
    /// <see langword="true"/> to allow prerelease SDK versions; <see langword="false"/> otherwise.
    /// </param>
    /// <returns>The current instance of the <see cref="IGlobalJsonBuilder"/> for fluent chaining.</returns>
    IGlobalJsonBuilder SetAllowPrerelease(bool allowPrerelease);

    /// <summary>
    /// Sets the roll-forward behavior for SDK version resolution.
    /// </summary>
    /// <remarks>
    /// The roll-forward policy determines how the SDK resolves versions when the exact
    /// specified version is not available. Different policies allow rolling forward to
    /// different release levels (patch, feature, minor, or major).
    /// </remarks>
    /// <param name="rollForward">
    /// The roll-forward policy to apply. For example, <see cref="RollForward.LatestMinor"/>
    /// allows rolling forward to the latest patch and feature level within the same major.minor version.
    /// </param>
    /// <returns>The current instance of the <see cref="IGlobalJsonBuilder"/> for fluent chaining.</returns>
    /// <seealso cref="RollForward"/>
    IGlobalJsonBuilder SetRollForward(RollForward rollForward);

    /// <summary>
    /// Sets the .NET SDK version to use for this project or directory tree.
    /// </summary>
    /// <remarks>
    /// The version string should be in semantic versioning format, for example "8.0.204" or "10.0.100".
    /// This version becomes the preferred SDK version for all dotnet commands executed in this
    /// directory and its subdirectories (unless overridden by nested global.json files).
    /// </remarks>
    /// <param name="runtimeVersion">
    /// The SDK version as a string (e.g., "8.0.204", "10.0.100").
    /// Must not be <see langword="null"/>, empty, or whitespace.
    /// </param>
    /// <returns>The current instance of the <see cref="IGlobalJsonBuilder"/> for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="runtimeVersion"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="runtimeVersion"/> is empty or whitespace.
    /// </exception>
    IGlobalJsonBuilder SetRuntimeSdk(string runtimeVersion);
}
