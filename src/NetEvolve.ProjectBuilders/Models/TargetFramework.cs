namespace NetEvolve.ProjectBuilders.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NetEvolve.Arguments;

/// <summary>
///
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/standard/frameworks" />
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/standard/frameworks#net-5-os-specific-tfms"/>
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "As designed")]
public readonly record struct TargetFramework
{
    /// <summary>
    /// Gets the name of the target framework.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the value of the target framework, which is used in the project file. For example, "net5.0".
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Gets the platform, if any, that the target framework is specific to.
    /// </summary>
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
    /// Creates a new instance of <see cref="TargetFramework"/>. Allows you to create a custom target framework.
    /// </summary>
    /// <param name="name">
    /// The name of the target framework. For example, "Net5".
    /// </param>
    /// <param name="value">
    /// The value of the target framework, which is used in the project file. For example, "net5.0".
    /// </param>
    /// <param name="platform">
    /// The platform, if any, that the target framework is specific to.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="TargetFramework"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> or <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the target framework is already registered or when <paramref name="name"/> or <paramref name="value"/> is empty or whitespace.</exception>
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

    /// <inheritdoc cref="object.ToString" />
    public override string ToString() => Value;

    /// <summary>
    /// Gets the target framework for .NET 5.
    /// </summary>
    public static TargetFramework Net5 { get; } = Create(nameof(Net5), "net5.0");

    /// <summary>
    /// Gets the target framework for .NET 5 on Android.
    /// </summary>
    public static TargetFramework Net5Android { get; } =
        Create(nameof(Net5Android), "net5.0-android", TargetPlatform.Android);

    /// <summary>
    /// Gets the target framework for .NET 5 on Browser (Blazor WebAssembly).
    /// </summary>
    public static TargetFramework Net5Browser { get; } =
        Create(nameof(Net5Browser), "net5.0-browser", TargetPlatform.Browser);

    /// <summary>
    /// Gets the target framework for .NET 5 on iOS.
    /// </summary>
    public static TargetFramework Net5iOS { get; } = Create(nameof(Net5iOS), "net5.0-ios", TargetPlatform.iOS);

    /// <summary>
    /// Gets the target framework for .NET 5 on Mac Catalyst.
    /// </summary>
    public static TargetFramework Net5MacCatalyst { get; } =
        Create(nameof(Net5MacCatalyst), "net5.0-maccatalyst", TargetPlatform.MacCatalyst);

    /// <summary>
    /// Gets the target framework for .NET 5 on macOS.
    /// </summary>
    public static TargetFramework Net5MacOs { get; } = Create(nameof(Net5MacOs), "net5.0-macos", TargetPlatform.MacOs);

    /// <summary>
    /// Gets the target framework for .NET 5 on Tizen.
    /// </summary>
    public static TargetFramework Net5Tizen { get; } = Create(nameof(Net5Tizen), "net5.0-tizen", TargetPlatform.Tizen);

    /// <summary>
    /// Gets the target framework for .NET 5 on tvOS.
    /// </summary>
    public static TargetFramework Net5tvOS { get; } = Create(nameof(Net5tvOS), "net5.0-tvos", TargetPlatform.tvOS);

    /// <summary>
    /// Gets the target framework for .NET 5 on Windows.
    /// </summary>
    public static TargetFramework Net5Windows { get; } =
        Create(nameof(Net5Windows), "net5.0-windows", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET 6.
    /// </summary>
    public static TargetFramework Net6 { get; } = Create(nameof(Net6), "net6.0");

    /// <summary>
    /// Gets the target framework for .NET 6 on Android.
    /// </summary>
    public static TargetFramework Net6Android { get; } =
        Create(nameof(Net6Android), "net6.0-android", TargetPlatform.Android);

    /// <summary>
    /// Gets the target framework for .NET 6 on Browser (Blazor WebAssembly).
    /// </summary>
    public static TargetFramework Net6Browser { get; } =
        Create(nameof(Net6Browser), "net6.0-browser", TargetPlatform.Browser);

    /// <summary>
    /// Gets the target framework for .NET 6 on iOS.
    /// </summary>
    public static TargetFramework Net6iOS { get; } = Create(nameof(Net6iOS), "net6.0-ios", TargetPlatform.iOS);

    /// <summary>
    /// Gets the target framework for .NET 6 on Mac Catalyst.
    /// </summary>
    public static TargetFramework Net6MacCatalyst { get; } =
        Create(nameof(Net6MacCatalyst), "net6.0-maccatalyst", TargetPlatform.MacCatalyst);

    /// <summary>
    /// Gets the target framework for .NET 6 on macOS.
    /// </summary>
    public static TargetFramework Net6MacOs { get; } = Create(nameof(Net6MacOs), "net6.0-macos", TargetPlatform.MacOs);

    /// <summary>
    /// Gets the target framework for .NET 6 on Tizen.
    /// </summary>
    public static TargetFramework Net6Tizen { get; } = Create(nameof(Net6Tizen), "net6.0-tizen", TargetPlatform.Tizen);

    /// <summary>
    /// Gets the target framework for .NET 6 on tvOS.
    /// </summary>
    public static TargetFramework Net6tvOS { get; } = Create(nameof(Net6tvOS), "net6.0-tvos", TargetPlatform.tvOS);

    /// <summary>
    /// Gets the target framework for .NET 6 on Windows.
    /// </summary>
    public static TargetFramework Net6Windows { get; } =
        Create(nameof(Net6Windows), "net6.0-windows", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET 7.
    /// </summary>
    public static TargetFramework Net7 { get; } = Create(nameof(Net7), "net7.0");

    /// <summary>
    /// Gets the target framework for .NET 7 on Android.
    /// </summary>
    public static TargetFramework Net7Android { get; } =
        Create(nameof(Net7Android), "net7.0-android", TargetPlatform.Android);

    /// <summary>
    /// Gets the target framework for .NET 7 on Browser (Blazor WebAssembly).
    /// </summary>
    public static TargetFramework Net7Browser { get; } =
        Create(nameof(Net7Browser), "net7.0-browser", TargetPlatform.Browser);

    /// <summary>
    /// Gets the target framework for .NET 7 on iOS.
    /// </summary>
    public static TargetFramework Net7iOS { get; } = Create(nameof(Net7iOS), "net7.0-ios", TargetPlatform.iOS);

    /// <summary>
    /// Gets the target framework for .NET 7 on Mac Catalyst.
    /// </summary>
    public static TargetFramework Net7MacCatalyst { get; } =
        Create(nameof(Net7MacCatalyst), "net7.0-maccatalyst", TargetPlatform.MacCatalyst);

    /// <summary>
    /// Gets the target framework for .NET 7 on macOS.
    /// </summary>
    public static TargetFramework Net7MacOs { get; } = Create(nameof(Net7MacOs), "net7.0-macos", TargetPlatform.MacOs);

    /// <summary>
    /// Gets the target framework for .NET 7 on Tizen.
    /// </summary>
    public static TargetFramework Net7Tizen { get; } = Create(nameof(Net7Tizen), "net7.0-tizen", TargetPlatform.Tizen);

    /// <summary>
    /// Gets the target framework for .NET 7 on tvOS.
    /// </summary>
    public static TargetFramework Net7tvOS { get; } = Create(nameof(Net7tvOS), "net7.0-tvos", TargetPlatform.tvOS);

    /// <summary>
    /// Gets the target framework for .NET 7 on Windows.
    /// </summary>
    public static TargetFramework Net7Windows { get; } =
        Create(nameof(Net7Windows), "net7.0-windows", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET 8.
    /// </summary>
    public static TargetFramework Net8 { get; } = Create(nameof(Net8), "net8.0");

    /// <summary>
    /// Gets the target framework for .NET 8 on Android.
    /// </summary>
    public static TargetFramework Net8Android { get; } =
        Create(nameof(Net8Android), "net8.0-android", TargetPlatform.Android);

    /// <summary>
    /// Gets the target framework for .NET 8 on Browser (Blazor WebAssembly).
    /// </summary>
    public static TargetFramework Net8Browser { get; } =
        Create(nameof(Net8Browser), "net8.0-browser", TargetPlatform.Browser);

    /// <summary>
    /// Gets the target framework for .NET 8 on iOS.
    /// </summary>
    public static TargetFramework Net8iOS { get; } = Create(nameof(Net8iOS), "net8.0-ios", TargetPlatform.iOS);

    /// <summary>
    /// Gets the target framework for .NET 8 on Mac Catalyst.
    /// </summary>
    public static TargetFramework Net8MacCatalyst { get; } =
        Create(nameof(Net8MacCatalyst), "net8.0-maccatalyst", TargetPlatform.MacCatalyst);

    /// <summary>
    /// Gets the target framework for .NET 8 on macOS.
    /// </summary>
    public static TargetFramework Net8MacOs { get; } = Create(nameof(Net8MacOs), "net8.0-macos", TargetPlatform.MacOs);

    /// <summary>
    /// Gets the target framework for .NET 8 on Tizen.
    /// </summary>
    public static TargetFramework Net8Tizen { get; } = Create(nameof(Net8Tizen), "net8.0-tizen", TargetPlatform.Tizen);

    /// <summary>
    /// Gets the target framework for .NET 8 on tvOS.
    /// </summary>
    public static TargetFramework Net8tvOS { get; } = Create(nameof(Net8tvOS), "net8.0-tvos", TargetPlatform.tvOS);

    /// <summary>
    /// Gets the target framework for .NET 8 on Windows.
    /// </summary>
    public static TargetFramework Net8Windows { get; } =
        Create(nameof(Net8Windows), "net8.0-windows", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET 9.
    /// </summary>
    public static TargetFramework Net9 { get; } = Create(nameof(Net9), "net9.0");

    /// <summary>
    /// Gets the target framework for .NET 9 on Android.
    /// </summary>
    public static TargetFramework Net9Android { get; } =
        Create(nameof(Net9Android), "net9.0-android", TargetPlatform.Android);

    /// <summary>
    /// Gets the target framework for .NET 9 on Browser (Blazor WebAssembly).
    /// </summary>
    public static TargetFramework Net9Browser { get; } =
        Create(nameof(Net9Browser), "net9.0-browser", TargetPlatform.Browser);

    /// <summary>
    /// Gets the target framework for .NET 9 on iOS.
    /// </summary>
    public static TargetFramework Net9iOS { get; } = Create(nameof(Net9iOS), "net9.0-ios", TargetPlatform.iOS);

    /// <summary>
    /// Gets the target framework for .NET 9 on Mac Catalyst.
    /// </summary>
    public static TargetFramework Net9MacCatalyst { get; } =
        Create(nameof(Net9MacCatalyst), "net9.0-maccatalyst", TargetPlatform.MacCatalyst);

    /// <summary>
    /// Gets the target framework for .NET 9 on macOS.
    /// </summary>
    public static TargetFramework Net9MacOs { get; } = Create(nameof(Net9MacOs), "net9.0-macos", TargetPlatform.MacOs);

    /// <summary>
    /// Gets the target framework for .NET 9 on Tizen.
    /// </summary>
    public static TargetFramework Net9Tizen { get; } = Create(nameof(Net9Tizen), "net9.0-tizen", TargetPlatform.Tizen);

    /// <summary>
    /// Gets the target framework for .NET 9 on tvOS.
    /// </summary>
    public static TargetFramework Net9tvOS { get; } = Create(nameof(Net9tvOS), "net9.0-tvos", TargetPlatform.tvOS);

    /// <summary>
    /// Gets the target framework for .NET 9 on Windows.
    /// </summary>
    public static TargetFramework Net9Windows { get; } =
        Create(nameof(Net9Windows), "net9.0-windows", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET 10.
    /// </summary>
    public static TargetFramework Net10 { get; } = Create(nameof(Net10), "net10.0");

    /// <summary>
    /// Gets the target framework for .NET 10 on Android.
    /// </summary>
    public static TargetFramework Net10Android { get; } =
        Create(nameof(Net10Android), "net10.0-android", TargetPlatform.Android);

    /// <summary>
    /// Gets the target framework for .NET 10 on Browser (Blazor WebAssembly).
    /// </summary>
    public static TargetFramework Net10Browser { get; } =
        Create(nameof(Net10Browser), "net10.0-browser", TargetPlatform.Browser);

    /// <summary>
    /// Gets the target framework for .NET 10 on iOS.
    /// </summary>
    public static TargetFramework Net10iOS { get; } = Create(nameof(Net10iOS), "net10.0-ios", TargetPlatform.iOS);

    /// <summary>
    /// Gets the target framework for .NET 10 on Mac Catalyst.
    /// </summary>
    public static TargetFramework Net10MacCatalyst { get; } =
        Create(nameof(Net10MacCatalyst), "net10.0-maccatalyst", TargetPlatform.MacCatalyst);

    /// <summary>
    /// Gets the target framework for .NET 10 on macOS.
    /// </summary>
    public static TargetFramework Net10MacOs { get; } =
        Create(nameof(Net10MacOs), "net10.0-macos", TargetPlatform.MacOs);

    /// <summary>
    /// Gets the target framework for .NET 10 on Tizen.
    /// </summary>
    public static TargetFramework Net10Tizen { get; } =
        Create(nameof(Net10Tizen), "net10.0-tizen", TargetPlatform.Tizen);

    /// <summary>
    /// Gets the target framework for .NET 10 on tvOS.
    /// </summary>
    public static TargetFramework Net10tvOS { get; } = Create(nameof(Net10tvOS), "net10.0-tvos", TargetPlatform.tvOS);

    /// <summary>
    /// Gets the target framework for .NET 10 on Windows.
    /// </summary>
    public static TargetFramework Net10Windows { get; } =
        Create(nameof(Net10Windows), "net10.0-windows", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET Standard 1.0.
    /// </summary>
    public static TargetFramework NetStandard1_0 { get; } = Create(nameof(NetStandard1_0), "netstandard1.0");

    /// <summary>
    /// Gets the target framework for .NET Standard 1.1.
    /// </summary>
    public static TargetFramework NetStandard1_1 { get; } = Create(nameof(NetStandard1_1), "netstandard1.1");

    /// <summary>
    /// Gets the target framework for .NET Standard 1.2.
    /// </summary>
    public static TargetFramework NetStandard1_2 { get; } = Create(nameof(NetStandard1_2), "netstandard1.2");

    /// <summary>
    /// Gets the target framework for .NET Standard 1.3.
    /// </summary>
    public static TargetFramework NetStandard1_3 { get; } = Create(nameof(NetStandard1_3), "netstandard1.3");

    /// <summary>
    /// Gets the target framework for .NET Standard 1.4.
    /// </summary>
    public static TargetFramework NetStandard1_4 { get; } = Create(nameof(NetStandard1_4), "netstandard1.4");

    /// <summary>
    /// Gets the target framework for .NET Standard 1.5.
    /// </summary>
    public static TargetFramework NetStandard1_5 { get; } = Create(nameof(NetStandard1_5), "netstandard1.5");

    /// <summary>
    /// Gets the target framework for .NET Standard 1.6.
    /// </summary>
    public static TargetFramework NetStandard1_6 { get; } = Create(nameof(NetStandard1_6), "netstandard1.6");

    /// <summary>
    /// Gets the target framework for .NET Standard 2.0.
    /// </summary>
    public static TargetFramework NetStandard2_0 { get; } = Create(nameof(NetStandard2_0), "netstandard2.0");

    /// <summary>
    /// Gets the target framework for .NET Standard 2.1.
    /// </summary>
    public static TargetFramework NetStandard2_1 { get; } = Create(nameof(NetStandard2_1), "netstandard2.1");

    /// <summary>
    /// Gets the target framework for .NET Core 1.0.
    /// </summary>
    public static TargetFramework NetCoreApp1_0 { get; } = Create(nameof(NetCoreApp1_0), "netcoreapp1.0");

    /// <summary>
    /// Gets the target framework for .NET Core 1.1.
    /// </summary>
    public static TargetFramework NetCoreApp1_1 { get; } = Create(nameof(NetCoreApp1_1), "netcoreapp1.1");

    /// <summary>
    /// Gets the target framework for .NET Core 2.0.
    /// </summary>
    public static TargetFramework NetCoreApp2_0 { get; } = Create(nameof(NetCoreApp2_0), "netcoreapp2.0");

    /// <summary>
    /// Gets the target framework for .NET Core 2.1.
    /// </summary>
    public static TargetFramework NetCoreApp2_1 { get; } = Create(nameof(NetCoreApp2_1), "netcoreapp2.1");

    /// <summary>
    /// Gets the target framework for .NET Core 2.2.
    /// </summary>
    public static TargetFramework NetCoreApp2_2 { get; } = Create(nameof(NetCoreApp2_2), "netcoreapp2.2");

    /// <summary>
    /// Gets the target framework for .NET Core 3.0.
    /// </summary>
    public static TargetFramework NetCoreApp3_0 { get; } = Create(nameof(NetCoreApp3_0), "netcoreapp3.0");

    /// <summary>
    /// Gets the target framework for .NET Core 3.1.
    /// </summary>
    public static TargetFramework NetCoreApp3_1 { get; } = Create(nameof(NetCoreApp3_1), "netcoreapp3.1");

    /// <summary>
    /// Gets the target framework for .NET Framework 4.6.
    /// </summary>
    public static TargetFramework NetFramework4_6 { get; } =
        Create(nameof(NetFramework4_6), "net46", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET Framework 4.6.1.
    /// </summary>
    public static TargetFramework NetFramework4_6_1 { get; } =
        Create(nameof(NetFramework4_6_1), "net461", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET Framework 4.6.2.
    /// </summary>
    public static TargetFramework NetFramework4_6_2 { get; } =
        Create(nameof(NetFramework4_6_2), "net462", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET Framework 4.7.
    /// </summary>
    public static TargetFramework NetFramework4_7 { get; } =
        Create(nameof(NetFramework4_7), "net47", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET Framework 4.7.1.
    /// </summary>
    public static TargetFramework NetFramework4_7_1 { get; } =
        Create(nameof(NetFramework4_7_1), "net471", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET Framework 4.7.2.
    /// </summary>
    public static TargetFramework NetFramework4_7_2 { get; } =
        Create(nameof(NetFramework4_7_2), "net472", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET Framework 4.8.
    /// </summary>
    public static TargetFramework NetFramework4_8 { get; } =
        Create(nameof(NetFramework4_8), "net48", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET Framework 4.8.1.
    /// </summary>
    public static TargetFramework NetFramework4_8_1 { get; } =
        Create(nameof(NetFramework4_8_1), "net481", TargetPlatform.Windows);
}
