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
    /// Rolls forward from the specified version through feature bands, then minor versions, then major versions.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This policy allows the most aggressive roll-forward behavior. Starting with the specified version,
    /// it searches for higher patch levels in the feature band, then higher feature bands in the minor version,
    /// then higher minor versions in the major version, and finally rolls forward to higher major versions.
    /// </para>
    /// <para>
    /// Use this when compatibility across major versions is acceptable and you want automatic upgrades to the latest available SDK.
    /// </para>
    /// </remarks>
    Major = 5,

    /// <summary>
    /// Rolls forward from the specified version through feature bands and minor versions within the same major version.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Similar to Major but stops at major version boundaries, never rolling forward to a different major version.
    /// This is useful for staying within a major version family while allowing flexibility for minor upgrades.
    /// </para>
    /// <para>
    /// Example: For specified version 5.0.100, rolls forward through 5.0.x → 5.1.x → 5.2.x but not to 6.x.
    /// </para>
    /// </remarks>
    Minor = 6,

    /// <summary>
    /// Rolls forward from the specified version through feature bands within the same major and minor version.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Restricts roll-forward to feature band upgrades only, never allowing minor or major version changes.
    /// This is suitable for environments requiring tight version control within a minor version line.
    /// </para>
    /// <para>
    /// Example: For specified version 5.0.100, rolls forward through 5.0.100 → 5.0.200 → 5.0.300 but not to 5.1.x.
    /// </para>
    /// </remarks>
    Feature = 7,

    /// <summary>
    /// Uses the specified version, rolling forward only to the latest patch level if the exact version is not available.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A conservative policy that tries to use the exact specified version first, then falls back to higher patch levels
    /// but never to different minor, feature band, or major versions.
    /// </para>
    /// <para>
    /// This is the legacy default behavior from earlier .NET SDK versions and provides a good balance
    /// between stability and flexibility.
    /// </para>
    /// <para>
    /// Example: For specified version 5.0.100, accepts 5.0.100 or higher (5.0.101, 5.0.105, etc.) but not 5.0.99 or 5.1.x.
    /// </para>
    /// </remarks>
    Patch = 8,

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
    Disable = 9,
}
