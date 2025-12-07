namespace NetEvolve.ProjectBuilders.Models;

/// <summary>
/// Specifies the target platform for a .NET application or library.
/// </summary>
/// <remarks>
/// <para>
/// This enumeration represents the operating system or runtime platform that a .NET project targets.
/// Platform-specific target frameworks allow developers to create applications optimized for specific
/// platforms while potentially sharing common code through multi-targeting.
/// </para>
/// <para>
/// Platforms are typically specified in target framework monikers (TFM) as suffixes, such as
/// "net8.0-android", "net8.0-ios", or "net8.0-browser". The combination of framework and platform
/// determines which APIs and runtime features are available.
/// </para>
/// <para>
/// See <see href="https://learn.microsoft.com/en-us/dotnet/standard/frameworks#net-5-os-specific-tfms"/>
/// for more information on platform-specific target frameworks.
/// </para>
/// </remarks>
public enum TargetPlatform
{
    /// <summary>
    /// No platform restriction; the framework is cross-platform.
    /// </summary>
    /// <remarks>
    /// This value indicates the framework runs on all platforms without platform-specific targeting.
    /// Examples include standard .NET frameworks like "net8.0" used for general-purpose libraries.
    /// </remarks>
    None = 0,

    /// <summary>
    /// Windows desktop or server platform.
    /// </summary>
    /// <remarks>
    /// Used for Windows-specific applications including desktop applications, Windows Forms,
    /// WPF, and Windows service projects. Also applies to .NET Framework applications.
    /// </remarks>
    Windows,

    /// <summary>
    /// Android mobile platform.
    /// </summary>
    /// <remarks>
    /// Targets Android devices via .NET MAUI or Xamarin.Android. Enables development of Android
    /// mobile applications using C# and .NET libraries.
    /// </remarks>
    Android,

    /// <summary>
    /// Apple iOS mobile platform.
    /// </summary>
    /// <remarks>
    /// Targets Apple iPhone and iPad devices via .NET MAUI or Xamarin.iOS. Enables development of
    /// native iOS applications using C# and .NET libraries.
    /// </remarks>
    iOS,

    /// <summary>
    /// Mac Catalyst platform (iOS apps running on macOS).
    /// </summary>
    /// <remarks>
    /// Targets iPad applications running on Mac via Mac Catalyst technology. Allows iOS applications
    /// to run on macOS with desktop-optimized interfaces.
    /// </remarks>
    MacCatalyst,

    /// <summary>
    /// Apple macOS desktop platform.
    /// </summary>
    /// <remarks>
    /// Targets Apple Macintosh computers running macOS. Used for Mac desktop applications via
    /// .NET MAUI or native macOS frameworks. Also includes support for creating command-line tools.
    /// </remarks>
    MacOs,

    /// <summary>
    /// Apple tvOS platform (Apple TV).
    /// </summary>
    /// <remarks>
    /// Targets Apple TV devices. Enables development of applications for Apple TV set-top boxes
    /// using C# and .NET libraries.
    /// </remarks>
    tvOS,

    /// <summary>
    /// Samsung Tizen platform.
    /// </summary>
    /// <remarks>
    /// Targets devices running Samsung Tizen OS, including smart TVs and wearables. Support is
    /// community-maintained and may be limited compared to other major platforms.
    /// </remarks>
    Tizen,

    /// <summary>
    /// Browser platform using WebAssembly (Blazor).
    /// </summary>
    /// <remarks>
    /// <para>
    /// Targets web browsers via Blazor WebAssembly (WASM) technology. Enables C# code to run
    /// directly in browsers with near-native performance through WebAssembly.
    /// </para>
    /// <para>
    /// Differs from server-side Blazor which runs on the server; WASM provides client-side execution.
    /// </para>
    /// </remarks>
    Browser,
}
