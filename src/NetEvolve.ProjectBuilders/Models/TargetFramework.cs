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
    /// <exception cref="ArgumentNullException">If the target framework is already registered.</exception>
    public static TargetFramework Create(string name, string value, TargetPlatform platform = TargetPlatform.None)
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
    /// Gets the target framework for .NET 5 on Windows.
    /// </summary>
    public static TargetFramework Net5Windows { get; } =
        Create(nameof(Net5Windows), "net5.0-windows", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET 6.
    /// </summary>
    public static TargetFramework Net6 { get; } = Create(nameof(Net6), "net6.0");

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
    /// Gets the target framework for .NET 7 on Windows.
    /// </summary>
    public static TargetFramework Net7Windows { get; } =
        Create(nameof(Net7Windows), "net7.0-windows", TargetPlatform.Windows);

    /// <summary>
    /// Gets the target framework for .NET 8.
    /// </summary>
    public static TargetFramework Net8 { get; } = Create(nameof(Net8), "net8.0");

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
    /// Gets the target framework for .NET 9 on Windows.
    /// </summary>
    public static TargetFramework Net9Windows { get; } =
        Create(nameof(Net9Windows), "net9.0-windows", TargetPlatform.Windows);

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
}
