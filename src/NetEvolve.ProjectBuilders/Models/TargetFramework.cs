namespace NetEvolve.ProjectBuilders.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NetEvolve.Arguments;

/// <summary>
/// Represents a .NET target framework moniker and provides predefined constants for common frameworks.
/// </summary>
/// <remarks>
/// <para>
/// This readonly struct encapsulates information about a .NET target framework, including its name,
/// value (moniker), and optional platform specification. The library provides predefined instances
/// for all official .NET frameworks from .NET 5 through .NET 10, including platform-specific variants.
/// </para>
/// <para>
/// Target frameworks determine which .NET runtime and APIs are available for a project. The framework
/// is specified in project files via the TargetFramework (single) or TargetFrameworks (multiple) property.
/// </para>
/// <para>
/// Features:
/// <list type="bullet">
/// <item><description>Predefined constants for all official .NET frameworks and versions</description></item>
/// <item><description>Support for platform-specific frameworks (iOS, Android, Browser, etc.)</description></item>
/// <item><description>Extensibility via the <see cref="Create"/> method for custom frameworks</description></item>
/// <item><description>Immutable readonly struct for value semantics</description></item>
/// </list>
/// </para>
/// <para>
/// See <see href="https://learn.microsoft.com/en-us/dotnet/standard/frameworks"/> for
/// comprehensive information on .NET target frameworks and <see href="https://learn.microsoft.com/en-us/dotnet/standard/frameworks#net-5-os-specific-tfms"/>
/// for platform-specific framework monikers.
/// </para>
/// </remarks>
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "As designed")]
public readonly record struct TargetFramework
{
    /// <summary>
    /// Gets the programmatic name of the target framework.
    /// </summary>
    /// <remarks>
    /// This is the name used as a property identifier, typically matching the C# property name
    /// (e.g., "Net8", "Net9Android").
    /// </remarks>
    /// <value>The framework name as a string.</value>
    public string Name { get; }

    /// <summary>
    /// Gets the framework moniker string used in project files.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is the value that appears in the TargetFramework or TargetFrameworks property in .csproj files.
    /// Examples: "net5.0", "net8.0", "net9.0-android", "net10.0-ios".
    /// </para>
    /// <para>
    /// The value follows the .NET Target Framework Moniker (TFM) format defined by Microsoft.
    /// </para>
    /// </remarks>
    /// <value>The target framework moniker string.</value>
    public string Value { get; }

    /// <summary>
    /// Gets the platform this framework targets, if platform-specific.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For cross-platform frameworks like "net8.0", this is <see langword="null"/>.
    /// For platform-specific frameworks like "net8.0-android", this indicates the target platform.
    /// </para>
    /// <para>
    /// Platform-specific frameworks include mobile (Android, iOS, tvOS, macOS, Mac Catalyst),
    /// web (Browser/WebAssembly), and desktop (Windows) platforms.
    /// </para>
    /// </remarks>
    /// <value>The <see cref="TargetPlatform"/>, or <see langword="null"/> for cross-platform frameworks.</value>
    public TargetPlatform? Platform { get; }

    private TargetFramework(string name, string value, TargetPlatform? platform)
    {
        Name = name;
        Value = value;
        Platform = platform;
    }

    private static readonly HashSet<TargetFramework> _values = [];
    internal static TargetFramework[] Values => [.. _values];

    /// <summary>
    /// Creates a custom target framework instance.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method allows definition of custom or future target frameworks not included in the
    /// predefined set. Each unique combination of name and value can be registered once; attempting
    /// to register a duplicate throws an exception.
    /// </para>
    /// <para>
    /// The custom framework is registered in a global registry and can be retrieved alongside
    /// the predefined frameworks.
    /// </para>
    /// </remarks>
    /// <param name="name">
    /// The programmatic name for the framework (e.g., "Net5", "CustomFramework").
    /// Must not be <see langword="null"/>, empty, or whitespace.
    /// </param>
    /// <param name="value">
    /// The moniker string for the framework (e.g., "net5.0", "custom1.0").
    /// Must not be <see langword="null"/>, empty, or whitespace and should follow TFM conventions.
    /// </param>
    /// <param name="platform">
    /// The platform, if any, that this framework is specific to. Can be <see langword="null"/>
    /// for cross-platform frameworks. Defaults to <see langword="null"/>.
    /// </param>
    /// <returns>
    /// A new <see cref="TargetFramework"/> instance representing the created framework.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="name"/> or <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="name"/> or <paramref name="value"/> is empty or whitespace,
    /// or when a framework with the same name and value is already registered.
    /// </exception>
    public static TargetFramework Create(string name, string value, TargetPlatform? platform = null)
    {
        Argument.ThrowIfNullOrWhiteSpace(name);
        Argument.ThrowIfNullOrWhiteSpace(value);

        var targetFramework = new TargetFramework(name, value, platform);

        if (!_values.Add(targetFramework))
        {
            throw new ArgumentException("TargetFramework already registered.", nameof(name));
        }

        return targetFramework;
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Returns the framework moniker string (e.g., "net8.0").
    /// </remarks>
    public override string ToString() => Value;

    /// <summary>
    /// Gets the target framework for .NET 5.
    /// </summary>
    /// <remarks>
    /// .NET 5 is a legacy LTS framework. Consider using newer versions for new projects.
    /// </remarks>
    /// <value>The TargetFramework instance for "net5.0".</value>
    public static TargetFramework Net5 { get; } = Create(nameof(Net5), "net5.0");

    /// <summary>
    /// Gets the target framework for .NET 5 on Android.
    /// </summary>
    /// <remarks>
    /// Used for Android mobile applications targeting .NET 5 APIs.
    /// </remarks>
    /// <value>The TargetFramework instance for "net5.0-android".</value>
    public static TargetFramework Net5Android { get; } =
        Create(nameof(Net5Android), "net5.0-android", TargetPlatform.Android);

    /// <summary>
    /// Gets the target framework for .NET 5 on Browser (Blazor WebAssembly).
    /// </summary>
    /// <remarks>
    /// Used for Blazor WebAssembly applications targeting .NET 5 APIs.
    /// </remarks>
    /// <value>The TargetFramework instance for "net5.0-browser".</value>
    public static TargetFramework Net5Browser { get; } =
        Create(nameof(Net5Browser), "net5.0-browser", TargetPlatform.Browser);

    /// <summary>
    /// Gets the target framework for .NET 5 on iOS.
    /// </summary>
    /// <remarks>
    /// Used for iOS mobile applications targeting .NET 5 APIs.
    /// </remarks>
    /// <value>The TargetFramework instance for "net5.0-ios".</value>
    public static TargetFramework Net5iOS { get; } = Create(nameof(Net5iOS), "net5.0-ios", TargetPlatform.iOS);

    /// <summary>
    /// Gets the target framework for .NET 5 on Mac Catalyst.
    /// </summary>
    /// <remarks>
    /// Used for Mac Catalyst applications (iOS apps running on macOS) targeting .NET 5 APIs.
    /// </remarks>
    /// <value>The TargetFramework instance for "net5.0-maccatalyst".</value>
    public static TargetFramework Net5MacCatalyst { get; } =
        Create(nameof(Net5MacCatalyst), "net5.0-maccatalyst", TargetPlatform.MacCatalyst);

    /// <summary>
    /// Gets the target framework for .NET 5 on macOS.
    /// </summary>
    /// <remarks>
    /// Used for desktop applications targeting .NET 5 APIs on macOS.
    /// </remarks>
    /// <value>The TargetFramework instance for "net5.0-macos".</value>
    public static TargetFramework Net5MacOs { get; } = Create(nameof(Net5MacOs), "net5.0-macos", TargetPlatform.MacOs);

    /// <summary>
    /// Gets the target framework for .NET 5 on Tizen.
    /// </summary>
    /// <remarks>
    /// Used for Tizen-based applications targeting .NET 5 APIs.
    /// </remarks>
    /// <value>The TargetFramework instance for "net5.0-tizen".</value>
    public static TargetFramework Net5Tizen { get; } = Create(nameof(Net5Tizen), "net5.0-tizen", TargetPlatform.Tizen);

    /// <summary>
    /// Gets the target framework for .NET 5 on tvOS.
    /// </summary>
    /// <remarks>
    /// Used for Apple TV applications targeting .NET 5 APIs.
    /// </remarks>
    /// <value>The TargetFramework instance for "net5.0-tvos".</value>
    public static TargetFramework Net5tvOS { get; } = Create(nameof(Net5tvOS), "net5.0-tvos", TargetPlatform.tvOS);

    /// <summary>
    /// Gets the target framework for .NET 5 on Windows.
    /// </summary>
    /// <remarks>
    /// Used for Windows desktop applications targeting .NET 5 APIs.
    /// </remarks>
    /// <value>The TargetFramework instance for "net5.0-windows".</value>
    public static TargetFramework Net5Windows { get; } =
        Create(nameof(Net5Windows), "net5.0-windows", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET 6.
    /// </summary>
    /// <remarks>
    /// .NET 6 is an LTS (Long-Term Support) framework released in November 2021.
    /// </remarks>
    /// <value>The TargetFramework instance for "net6.0".</value>
    public static TargetFramework Net6 { get; } = Create(nameof(Net6), "net6.0");

    /// <summary>
    /// Gets the target framework for .NET 6 on Android.
    /// </summary>
    /// <value>The TargetFramework instance for "net6.0-android".</value>
    public static TargetFramework Net6Android { get; } =
        Create(nameof(Net6Android), "net6.0-android", TargetPlatform.Android);

    /// <summary>
    /// Gets the target framework for .NET 6 on Browser (Blazor WebAssembly).
    /// </summary>
    /// <value>The TargetFramework instance for "net6.0-browser".</value>
    public static TargetFramework Net6Browser { get; } =
        Create(nameof(Net6Browser), "net6.0-browser", TargetPlatform.Browser);

    /// <summary>
    /// Gets the target framework for .NET 6 on iOS.
    /// </summary>
    /// <value>The TargetFramework instance for "net6.0-ios".</value>
    public static TargetFramework Net6iOS { get; } = Create(nameof(Net6iOS), "net6.0-ios", TargetPlatform.iOS);

    /// <summary>
    /// Gets the target framework for .NET 6 on Mac Catalyst.
    /// </summary>
    /// <value>The TargetFramework instance for "net6.0-maccatalyst".</value>
    public static TargetFramework Net6MacCatalyst { get; } =
        Create(nameof(Net6MacCatalyst), "net6.0-maccatalyst", TargetPlatform.MacCatalyst);

    /// <summary>
    /// Gets the target framework for .NET 6 on macOS.
    /// </summary>
    /// <value>The TargetFramework instance for "net6.0-macos".</value>
    public static TargetFramework Net6MacOs { get; } = Create(nameof(Net6MacOs), "net6.0-macos", TargetPlatform.MacOs);

    /// <summary>
    /// Gets the target framework for .NET 6 on Tizen.
    /// </summary>
    /// <value>The TargetFramework instance for "net6.0-tizen".</value>
    public static TargetFramework Net6Tizen { get; } = Create(nameof(Net6Tizen), "net6.0-tizen", TargetPlatform.Tizen);

    /// <summary>
    /// Gets the target framework for .NET 6 on tvOS.
    /// </summary>
    /// <value>The TargetFramework instance for "net6.0-tvos".</value>
    public static TargetFramework Net6tvOS { get; } = Create(nameof(Net6tvOS), "net6.0-tvos", TargetPlatform.tvOS);

    /// <summary>
    /// Gets the target framework for .NET 6 on Windows.
    /// </summary>
    /// <value>The TargetFramework instance for "net6.0-windows".</value>
    public static TargetFramework Net6Windows { get; } =
        Create(nameof(Net6Windows), "net6.0-windows", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET 7.
    /// </summary>
    /// <remarks>
    /// .NET 7 was released in November 2022 and entered maintenance mode in May 2023.
    /// </remarks>
    /// <value>The TargetFramework instance for "net7.0".</value>
    public static TargetFramework Net7 { get; } = Create(nameof(Net7), "net7.0");

    /// <summary>
    /// Gets the target framework for .NET 7 on Android.
    /// </summary>
    /// <value>The TargetFramework instance for "net7.0-android".</value>
    public static TargetFramework Net7Android { get; } =
        Create(nameof(Net7Android), "net7.0-android", TargetPlatform.Android);

    /// <summary>
    /// Gets the target framework for .NET 7 on Browser (Blazor WebAssembly).
    /// </summary>
    /// <value>The TargetFramework instance for "net7.0-browser".</value>
    public static TargetFramework Net7Browser { get; } =
        Create(nameof(Net7Browser), "net7.0-browser", TargetPlatform.Browser);

    /// <summary>
    /// Gets the target framework for .NET 7 on iOS.
    /// </summary>
    /// <value>The TargetFramework instance for "net7.0-ios".</value>
    public static TargetFramework Net7iOS { get; } = Create(nameof(Net7iOS), "net7.0-ios", TargetPlatform.iOS);

    /// <summary>
    /// Gets the target framework for .NET 7 on Mac Catalyst.
    /// </summary>
    /// <value>The TargetFramework instance for "net7.0-maccatalyst".</value>
    public static TargetFramework Net7MacCatalyst { get; } =
        Create(nameof(Net7MacCatalyst), "net7.0-maccatalyst", TargetPlatform.MacCatalyst);

    /// <summary>
    /// Gets the target framework for .NET 7 on macOS.
    /// </summary>
    /// <value>The TargetFramework instance for "net7.0-macos".</value>
    public static TargetFramework Net7MacOs { get; } = Create(nameof(Net7MacOs), "net7.0-macos", TargetPlatform.MacOs);

    /// <summary>
    /// Gets the target framework for .NET 7 on Tizen.
    /// </summary>
    /// <value>The TargetFramework instance for "net7.0-tizen".</value>
    public static TargetFramework Net7Tizen { get; } = Create(nameof(Net7Tizen), "net7.0-tizen", TargetPlatform.Tizen);

    /// <summary>
    /// Gets the target framework for .NET 7 on tvOS.
    /// </summary>
    /// <value>The TargetFramework instance for "net7.0-tvos".</value>
    public static TargetFramework Net7tvOS { get; } = Create(nameof(Net7tvOS), "net7.0-tvos", TargetPlatform.tvOS);

    /// <summary>
    /// Gets the target framework for .NET 7 on Windows.
    /// </summary>
    /// <value>The TargetFramework instance for "net7.0-windows".</value>
    public static TargetFramework Net7Windows { get; } =
        Create(nameof(Net7Windows), "net7.0-windows", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET 8.
    /// </summary>
    /// <remarks>
    /// .NET 8 is an LTS (Long-Term Support) framework released in November 2023.
    /// </remarks>
    /// <value>The TargetFramework instance for "net8.0".</value>
    public static TargetFramework Net8 { get; } = Create(nameof(Net8), "net8.0");

    /// <summary>
    /// Gets the target framework for .NET 8 on Android.
    /// </summary>
    /// <value>The TargetFramework instance for "net8.0-android".</value>
    public static TargetFramework Net8Android { get; } =
        Create(nameof(Net8Android), "net8.0-android", TargetPlatform.Android);

    /// <summary>
    /// Gets the target framework for .NET 8 on Browser (Blazor WebAssembly).
    /// </summary>
    /// <value>The TargetFramework instance for "net8.0-browser".</value>
    public static TargetFramework Net8Browser { get; } =
        Create(nameof(Net8Browser), "net8.0-browser", TargetPlatform.Browser);

    /// <summary>
    /// Gets the target framework for .NET 8 on iOS.
    /// </summary>
    /// <value>The TargetFramework instance for "net8.0-ios".</value>
    public static TargetFramework Net8iOS { get; } = Create(nameof(Net8iOS), "net8.0-ios", TargetPlatform.iOS);

    /// <summary>
    /// Gets the target framework for .NET 8 on Mac Catalyst.
    /// </summary>
    /// <value>The TargetFramework instance for "net8.0-maccatalyst".</value>
    public static TargetFramework Net8MacCatalyst { get; } =
        Create(nameof(Net8MacCatalyst), "net8.0-maccatalyst", TargetPlatform.MacCatalyst);

    /// <summary>
    /// Gets the target framework for .NET 8 on macOS.
    /// </summary>
    /// <value>The TargetFramework instance for "net8.0-macos".</value>
    public static TargetFramework Net8MacOs { get; } = Create(nameof(Net8MacOs), "net8.0-macos", TargetPlatform.MacOs);

    /// <summary>
    /// Gets the target framework for .NET 8 on Tizen.
    /// </summary>
    /// <value>The TargetFramework instance for "net8.0-tizen".</value>
    public static TargetFramework Net8Tizen { get; } = Create(nameof(Net8Tizen), "net8.0-tizen", TargetPlatform.Tizen);

    /// <summary>
    /// Gets the target framework for .NET 8 on tvOS.
    /// </summary>
    /// <value>The TargetFramework instance for "net8.0-tvos".</value>
    public static TargetFramework Net8tvOS { get; } = Create(nameof(Net8tvOS), "net8.0-tvos", TargetPlatform.tvOS);

    /// <summary>
    /// Gets the target framework for .NET 8 on Windows.
    /// </summary>
    /// <value>The TargetFramework instance for "net8.0-windows".</value>
    public static TargetFramework Net8Windows { get; } =
        Create(nameof(Net8Windows), "net8.0-windows", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET 9.
    /// </summary>
    /// <remarks>
    /// .NET 9 was released in November 2024 and is a current release.
    /// </remarks>
    /// <value>The TargetFramework instance for "net9.0".</value>
    public static TargetFramework Net9 { get; } = Create(nameof(Net9), "net9.0");

    /// <summary>
    /// Gets the target framework for .NET 9 on Android.
    /// </summary>
    /// <value>The TargetFramework instance for "net9.0-android".</value>
    public static TargetFramework Net9Android { get; } =
        Create(nameof(Net9Android), "net9.0-android", TargetPlatform.Android);

    /// <summary>
    /// Gets the target framework for .NET 9 on Browser (Blazor WebAssembly).
    /// </summary>
    /// <value>The TargetFramework instance for "net9.0-browser".</value>
    public static TargetFramework Net9Browser { get; } =
        Create(nameof(Net9Browser), "net9.0-browser", TargetPlatform.Browser);

    /// <summary>
    /// Gets the target framework for .NET 9 on iOS.
    /// </summary>
    /// <value>The TargetFramework instance for "net9.0-ios".</value>
    public static TargetFramework Net9iOS { get; } = Create(nameof(Net9iOS), "net9.0-ios", TargetPlatform.iOS);

    /// <summary>
    /// Gets the target framework for .NET 9 on Mac Catalyst.
    /// </summary>
    /// <value>The TargetFramework instance for "net9.0-maccatalyst".</value>
    public static TargetFramework Net9MacCatalyst { get; } =
        Create(nameof(Net9MacCatalyst), "net9.0-maccatalyst", TargetPlatform.MacCatalyst);

    /// <summary>
    /// Gets the target framework for .NET 9 on macOS.
    /// </summary>
    /// <value>The TargetFramework instance for "net9.0-macos".</value>
    public static TargetFramework Net9MacOs { get; } = Create(nameof(Net9MacOs), "net9.0-macos", TargetPlatform.MacOs);

    /// <summary>
    /// Gets the target framework for .NET 9 on Tizen.
    /// </summary>
    /// <value>The TargetFramework instance for "net9.0-tizen".</value>
    public static TargetFramework Net9Tizen { get; } = Create(nameof(Net9Tizen), "net9.0-tizen", TargetPlatform.Tizen);

    /// <summary>
    /// Gets the target framework for .NET 9 on tvOS.
    /// </summary>
    /// <value>The TargetFramework instance for "net9.0-tvos".</value>
    public static TargetFramework Net9tvOS { get; } = Create(nameof(Net9tvOS), "net9.0-tvos", TargetPlatform.tvOS);

    /// <summary>
    /// Gets the target framework for .NET 9 on Windows.
    /// </summary>
    /// <value>The TargetFramework instance for "net9.0-windows".</value>
    public static TargetFramework Net9Windows { get; } =
        Create(nameof(Net9Windows), "net9.0-windows", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET 10.
    /// </summary>
    /// <remarks>
    /// .NET 10 is an LTS (Long-Term Support) framework released in November 2025.
    /// </remarks>
    /// <value>The TargetFramework instance for "net10.0".</value>
    public static TargetFramework Net10 { get; } = Create(nameof(Net10), "net10.0");

    /// <summary>
    /// Gets the target framework for .NET 10 on Android.
    /// </summary>
    /// <value>The TargetFramework instance for "net10.0-android".</value>
    public static TargetFramework Net10Android { get; } =
        Create(nameof(Net10Android), "net10.0-android", TargetPlatform.Android);

    /// <summary>
    /// Gets the target framework for .NET 10 on Browser (Blazor WebAssembly).
    /// </summary>
    /// <value>The TargetFramework instance for "net10.0-browser".</value>
    public static TargetFramework Net10Browser { get; } =
        Create(nameof(Net10Browser), "net10.0-browser", TargetPlatform.Browser);

    /// <summary>
    /// Gets the target framework for .NET 10 on iOS.
    /// </summary>
    /// <value>The TargetFramework instance for "net10.0-ios".</value>
    public static TargetFramework Net10iOS { get; } = Create(nameof(Net10iOS), "net10.0-ios", TargetPlatform.iOS);

    /// <summary>
    /// Gets the target framework for .NET 10 on Mac Catalyst.
    /// </summary>
    /// <value>The TargetFramework instance for "net10.0-maccatalyst".</value>
    public static TargetFramework Net10MacCatalyst { get; } =
        Create(nameof(Net10MacCatalyst), "net10.0-maccatalyst", TargetPlatform.MacCatalyst);

    /// <summary>
    /// Gets the target framework for .NET 10 on macOS.
    /// </summary>
    /// <value>The TargetFramework instance for "net10.0-macos".</value>
    public static TargetFramework Net10MacOs { get; } =
        Create(nameof(Net10MacOs), "net10.0-macos", TargetPlatform.MacOs);

    /// <summary>
    /// Gets the target framework for .NET 10 on Tizen.
    /// </summary>
    /// <value>The TargetFramework instance for "net10.0-tizen".</value>
    public static TargetFramework Net10Tizen { get; } =
        Create(nameof(Net10Tizen), "net10.0-tizen", TargetPlatform.Tizen);

    /// <summary>
    /// Gets the target framework for .NET 10 on tvOS.
    /// </summary>
    /// <value>The TargetFramework instance for "net10.0-tvos".</value>
    public static TargetFramework Net10tvOS { get; } = Create(nameof(Net10tvOS), "net10.0-tvos", TargetPlatform.tvOS);

    /// <summary>
    /// Gets the target framework for .NET 10 on Windows.
    /// </summary>
    /// <value>The TargetFramework instance for "net10.0-windows".</value>
    public static TargetFramework Net10Windows { get; } =
        Create(nameof(Net10Windows), "net10.0-windows", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET Standard 1.0.
    /// </summary>
    /// <remarks>
    /// .NET Standard 1.0 is a legacy framework specification. Use newer .NET versions for new projects.
    /// </remarks>
    /// <value>The TargetFramework instance for "netstandard1.0".</value>
    public static TargetFramework NetStandard1_0 { get; } = Create(nameof(NetStandard1_0), "netstandard1.0");

    /// <summary>
    /// Gets the target framework for .NET Standard 1.1.
    /// </summary>
    /// <value>The TargetFramework instance for "netstandard1.1".</value>
    public static TargetFramework NetStandard1_1 { get; } = Create(nameof(NetStandard1_1), "netstandard1.1");

    /// <summary>
    /// Gets the target framework for .NET Standard 1.2.
    /// </summary>
    /// <value>The TargetFramework instance for "netstandard1.2".</value>
    public static TargetFramework NetStandard1_2 { get; } = Create(nameof(NetStandard1_2), "netstandard1.2");

    /// <summary>
    /// Gets the target framework for .NET Standard 1.3.
    /// </summary>
    /// <value>The TargetFramework instance for "netstandard1.3".</value>
    public static TargetFramework NetStandard1_3 { get; } = Create(nameof(NetStandard1_3), "netstandard1.3");

    /// <summary>
    /// Gets the target framework for .NET Standard 1.4.
    /// </summary>
    /// <value>The TargetFramework instance for "netstandard1.4".</value>
    public static TargetFramework NetStandard1_4 { get; } = Create(nameof(NetStandard1_4), "netstandard1.4");

    /// <summary>
    /// Gets the target framework for .NET Standard 1.5.
    /// </summary>
    /// <value>The TargetFramework instance for "netstandard1.5".</value>
    public static TargetFramework NetStandard1_5 { get; } = Create(nameof(NetStandard1_5), "netstandard1.5");

    /// <summary>
    /// Gets the target framework for .NET Standard 1.6.
    /// </summary>
    /// <value>The TargetFramework instance for "netstandard1.6".</value>
    public static TargetFramework NetStandard1_6 { get; } = Create(nameof(NetStandard1_6), "netstandard1.6");

    /// <summary>
    /// Gets the target framework for .NET Standard 2.0.
    /// </summary>
    /// <remarks>
    /// .NET Standard 2.0 is widely supported and still used for cross-platform library compatibility.
    /// </remarks>
    /// <value>The TargetFramework instance for "netstandard2.0".</value>
    public static TargetFramework NetStandard2_0 { get; } = Create(nameof(NetStandard2_0), "netstandard2.0");

    /// <summary>
    /// Gets the target framework for .NET Standard 2.1.
    /// </summary>
    /// <remarks>
    /// .NET Standard 2.1 is the final version of the .NET Standard specification.
    /// </remarks>
    /// <value>The TargetFramework instance for "netstandard2.1".</value>
    public static TargetFramework NetStandard2_1 { get; } = Create(nameof(NetStandard2_1), "netstandard2.1");

    /// <summary>
    /// Gets the target framework for .NET Core 1.0.
    /// </summary>
    /// <remarks>
    /// .NET Core 1.0 is a legacy framework. Migrate to modern .NET versions for new projects.
    /// </remarks>
    /// <value>The TargetFramework instance for "netcoreapp1.0".</value>
    public static TargetFramework NetCoreApp1_0 { get; } = Create(nameof(NetCoreApp1_0), "netcoreapp1.0");

    /// <summary>
    /// Gets the target framework for .NET Core 1.1.
    /// </summary>
    /// <value>The TargetFramework instance for "netcoreapp1.1".</value>
    public static TargetFramework NetCoreApp1_1 { get; } = Create(nameof(NetCoreApp1_1), "netcoreapp1.1");

    /// <summary>
    /// Gets the target framework for .NET Core 2.0.
    /// </summary>
    /// <value>The TargetFramework instance for "netcoreapp2.0".</value>
    public static TargetFramework NetCoreApp2_0 { get; } = Create(nameof(NetCoreApp2_0), "netcoreapp2.0");

    /// <summary>
    /// Gets the target framework for .NET Core 2.1.
    /// </summary>
    /// <remarks>
    /// .NET Core 2.1 is a legacy LTS framework. Consider upgrading to .NET 6 or later.
    /// </remarks>
    /// <value>The TargetFramework instance for "netcoreapp2.1".</value>
    public static TargetFramework NetCoreApp2_1 { get; } = Create(nameof(NetCoreApp2_1), "netcoreapp2.1");

    /// <summary>
    /// Gets the target framework for .NET Core 2.2.
    /// </summary>
    /// <value>The TargetFramework instance for "netcoreapp2.2".</value>
    public static TargetFramework NetCoreApp2_2 { get; } = Create(nameof(NetCoreApp2_2), "netcoreapp2.2");

    /// <summary>
    /// Gets the target framework for .NET Core 3.0.
    /// </summary>
    /// <value>The TargetFramework instance for "netcoreapp3.0".</value>
    public static TargetFramework NetCoreApp3_0 { get; } = Create(nameof(NetCoreApp3_0), "netcoreapp3.0");

    /// <summary>
    /// Gets the target framework for .NET Core 3.1.
    /// </summary>
    /// <remarks>
    /// .NET Core 3.1 is a legacy LTS framework with support ending in December 2022. Migrate to .NET 6 or later.
    /// </remarks>
    /// <value>The TargetFramework instance for "netcoreapp3.1".</value>
    public static TargetFramework NetCoreApp3_1 { get; } = Create(nameof(NetCoreApp3_1), "netcoreapp3.1");

    /// <summary>
    /// Gets the target framework for .NET Framework 4.6.
    /// </summary>
    /// <remarks>
    /// .NET Framework 4.6 is a legacy desktop-only framework. Migrate to modern .NET for new projects.
    /// </remarks>
    /// <value>The TargetFramework instance for "net46".</value>
    public static TargetFramework NetFramework4_6 { get; } =
        Create(nameof(NetFramework4_6), "net46", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET Framework 4.6.1.
    /// </summary>
    /// <value>The TargetFramework instance for "net461".</value>
    public static TargetFramework NetFramework4_6_1 { get; } =
        Create(nameof(NetFramework4_6_1), "net461", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET Framework 4.6.2.
    /// </summary>
    /// <value>The TargetFramework instance for "net462".</value>
    public static TargetFramework NetFramework4_6_2 { get; } =
        Create(nameof(NetFramework4_6_2), "net462", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET Framework 4.7.
    /// </summary>
    /// <value>The TargetFramework instance for "net47".</value>
    public static TargetFramework NetFramework4_7 { get; } =
        Create(nameof(NetFramework4_7), "net47", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET Framework 4.7.1.
    /// </summary>
    /// <value>The TargetFramework instance for "net471".</value>
    public static TargetFramework NetFramework4_7_1 { get; } =
        Create(nameof(NetFramework4_7_1), "net471", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET Framework 4.7.2.
    /// </summary>
    /// <value>The TargetFramework instance for "net472".</value>
    public static TargetFramework NetFramework4_7_2 { get; } =
        Create(nameof(NetFramework4_7_2), "net472", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET Framework 4.8.
    /// </summary>
    /// <remarks>
    /// .NET Framework 4.8 is the final major version of the .NET Framework. Migrate to modern .NET for new projects.
    /// </remarks>
    /// <value>The TargetFramework instance for "net48".</value>
    public static TargetFramework NetFramework4_8 { get; } =
        Create(nameof(NetFramework4_8), "net48", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET Framework 4.8.1.
    /// </summary>
    /// <remarks>
    /// .NET Framework 4.8.1 is a minor update to .NET Framework 4.8, released in 2023 for specific scenarios.
    /// </remarks>
    /// <value>The TargetFramework instance for "net481".</value>
    public static TargetFramework NetFramework4_8_1 { get; } =
        Create(nameof(NetFramework4_8_1), "net481", TargetPlatform.Windows);
}
