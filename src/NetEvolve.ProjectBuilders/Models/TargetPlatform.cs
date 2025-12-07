namespace NetEvolve.ProjectBuilders.Models;

/// <summary>
/// Represents the target platform of a project.
/// </summary>
public enum TargetPlatform
{
    /// <summary>
    /// No target platform restriction, the project can be built on any platform.
    /// </summary>
    None = 0,

    /// <summary>
    /// Windows platform.
    /// </summary>
    Windows,

    /// <summary>
    /// Android platform.
    /// </summary>
    Android,

    /// <summary>
    /// iOS platform.
    /// </summary>
    iOS,

    /// <summary>
    /// Mac Catalyst platform.
    /// </summary>
    MacCatalyst,

    /// <summary>
    /// Mac OS platform.
    /// </summary>
    MacOs,

    /// <summary>
    /// tvOS platform.
    /// </summary>
    tvOS,

    /// <summary>
    /// Tizen platform.
    /// </summary>
    Tizen,

    /// <summary>
    /// Browser platform, Blazor support.
    /// </summary>
    Browser,
}
