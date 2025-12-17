namespace NetEvolve.ProjectBuilders.Models;

/// <summary>
/// Specifies the roll-forward policy for a .NET SDK version selection.
/// </summary>
/// <remarks>
/// <para>
/// This enumeration controls how the .NET runtime resolves an SDK version when the exact specified version
/// is not available. The policy determines the level of flexibility in accepting newer versions within
/// various version component bounds (major, minor, feature band, patch).
/// </para>
/// <para>
/// Roll-forward policies are typically specified in <c>global.json</c> files to control SDK version selection
/// during project builds. Each policy represents a different strategy for handling version mismatches:
/// strict (Disable), exact-with-fallback (Patch), and progressive looser matching policies (Feature, Minor, Major).
/// </para>
/// <para>
/// See <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/global-json#rollforward"/> for
/// official documentation on SDK roll-forward policies.
/// </para>
/// </remarks>
public enum RollForward
{
    /// <summary>
    /// Disables forward rolling; no policy is applied.
    /// </summary>
    /// <remarks>
    /// This value indicates that SDK version selection will not attempt to use an alternative version.
    /// The specified version must be found or the command will fail.
    /// </remarks>
    None = 0,

    /// <summary>
    /// Uses the highest installed .NET SDK with a version greater than or equal to the specified version across all components.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is the most permissive policy, accepting any newer version regardless of major version bumps.
    /// It's suitable when you want the latest available SDK functionality without any version constraints.
    /// </para>
    /// <para>
    /// Example: For specified version 5.0.0, this policy accepts 5.0.5, 6.0.0, 7.1.3, etc.
    /// </para>
    /// </remarks>
    LatestMajor = 1,

    /// <summary>
    /// Uses the highest installed minor, feature band, and patch version matching the specified major version.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Allows upgrades within the same major version line but not to a different major version.
    /// Suitable for long-term support releases where you want to stay within a major version family.
    /// </para>
    /// <para>
    /// Example: For specified version 5.0.0, this policy accepts 5.3.8 but not 6.0.0.
    /// </para>
    /// </remarks>
    LatestMinor = 2,

    /// <summary>
    /// Uses the highest installed feature band and patch version matching the specified major and minor version.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Allows upgrades within the same major.minor version but not to different minor versions.
    /// Provides more control than LatestMinor for environments with stricter version requirements.
    /// </para>
    /// <para>
    /// Example: For specified version 5.0.0, this policy accepts 5.0.8 but not 5.1.0.
    /// </para>
    /// </remarks>
    LatestFeature = 3,

    /// <summary>
    /// Uses the latest installed patch version matching the specified major, minor, and feature band.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The most conservative upgrade policy, allowing only patch-level updates.
    /// Recommended for production environments where version stability is critical.
    /// </para>
    /// <para>
    /// Example: For specified version 5.0.0, this policy accepts 5.0.8 but not 5.0.0-preview.
    /// </para>
    /// </remarks>
    LatestPatch = 4,

    /// <summary>
    /// Requires an exact version match; no roll-forward is allowed.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The most restrictive policy. The specified version must be installed, or the command will fail.
    /// No automatic version resolution or fallback mechanisms are applied.
    /// </para>
    /// <para>
    /// Use this policy in strictly controlled environments where reproducible builds with exact versions are essential.
    /// </para>
    /// </remarks>
    Disable = 5,
}
