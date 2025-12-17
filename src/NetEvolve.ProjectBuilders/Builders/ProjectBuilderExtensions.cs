namespace NetEvolve.ProjectBuilders.Builders;

using System;
using System.IO;
using System.Text;
using NetEvolve.Arguments;
using NetEvolve.ProjectBuilders.Abstractions;
using NetEvolve.ProjectBuilders.Models;

/// <summary>
/// Provides extension methods for <see cref="IProjectBuilder"/> to simplify project configuration.
/// </summary>
/// <remarks>
/// <para>
/// This static class provides convenient methods for configuring project properties and adding
/// source code files to projects. All methods use the fluent API pattern for chaining.
/// </para>
/// <para>
/// These extensions cover common scenarios like setting target frameworks, configuring nullable
/// reference handling, and adding C# or VB.NET source files.
/// </para>
/// </remarks>
public static class ProjectBuilderExtensions
{
    /// <summary>
    /// Adds a C# source file to the project.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method creates a .cs file in the project with the specified content.
    /// The file name extension is automatically added if not present.
    /// </para>
    /// <para>
    /// The content is encoded as UTF-8 before writing to the file.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">The type of the project builder.</typeparam>
    /// <param name="builder">The project builder to add the file to. Must not be <see langword="null"/>.</param>
    /// <param name="fileName">
    /// The name of the file to create (e.g., "Program", "Class1").
    /// Must not be <see langword="null"/>, empty, or whitespace.
    /// </param>
    /// <param name="content">
    /// The C# source code content to write to the file.
    /// Must not be <see langword="null"/>, empty, or whitespace.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="builder"/>, <paramref name="fileName"/>, or <paramref name="content"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="fileName"/> or <paramref name="content"/> is empty or whitespace.
    /// </exception>
    public static void AddCSharpFile<T>(this T builder, string fileName, string content)
        where T : class, IProjectBuilder
    {
        Argument.ThrowIfNull(builder);
        Argument.ThrowIfNullOrWhiteSpace(fileName);
        Argument.ThrowIfNullOrWhiteSpace(content);

        if (builder is ProjectBuilder projectBuilder)
        {
            using var file = projectBuilder.CreateFile($"{Path.GetFileNameWithoutExtension(fileName)}.cs");
            file.Write(Encoding.UTF8.GetBytes(content).AsSpan());
        }
    }

    /// <summary>
    /// Adds a VB.NET source file to the project.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method creates a .vb file in the project with the specified content.
    /// The file name extension is automatically added if not present.
    /// </para>
    /// <para>
    /// The content is encoded as UTF-8 before writing to the file.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">The type of the project builder.</typeparam>
    /// <param name="builder">The project builder to add the file to. Must not be <see langword="null"/>.</param>
    /// <param name="fileName">
    /// The name of the file to create (e.g., "Program", "Class1").
    /// Must not be <see langword="null"/>, empty, or whitespace.
    /// </param>
    /// <param name="content">
    /// The VB.NET source code content to write to the file.
    /// Must not be <see langword="null"/>, empty, or whitespace.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="builder"/>, <paramref name="fileName"/>, or <paramref name="content"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="fileName"/> or <paramref name="content"/> is empty or whitespace.
    /// </exception>
    public static void AddVBFile<T>(this T builder, string fileName, string content)
        where T : class, IProjectBuilder
    {
        Argument.ThrowIfNull(builder);
        Argument.ThrowIfNullOrWhiteSpace(fileName);
        Argument.ThrowIfNullOrWhiteSpace(content);

        if (builder is ProjectBuilder projectBuilder)
        {
            using var file = projectBuilder.CreateFile($"{Path.GetFileNameWithoutExtension(fileName)}.vb");
            file.Write(Encoding.UTF8.GetBytes(content).AsSpan());
        }
    }

    /// <summary>
    /// Applies default project configuration settings.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method applies the following defaults:
    /// <list type="bullet">
    /// <item><description>Target framework: .NET 8</description></item>
    /// <item><description>Nullable reference types: Enabled</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// These are reasonable defaults for modern .NET development and test projects.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">The type of the project builder.</typeparam>
    /// <param name="builder">The project builder to configure. Must not be <see langword="null"/>.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="builder"/> is <see langword="null"/>.
    /// </exception>
    public static T WithDefaults<T>(this T builder)
        where T : class, IProjectBuilder
    {
        Argument.ThrowIfNull(builder);

        return builder.WithTargetFramework(TargetFramework.Net8).WithNullable(NullableOptions.Enable);
    }

    /// <summary>
    /// Sets the <see cref="NullableOptions">Nullable</see> property in the project file.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The Nullable property controls how the C# compiler handles nullable reference types.
    /// This property is typically set at the project level to apply nullable reference semantics
    /// to all code in the project.
    /// </para>
    /// <para>
    /// CAUTION: This method overwrites any existing value for the Nullable property.
    /// </para>
    /// <para>
    /// See <see href="https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references#nullable-contexts"/>
    /// for more information on nullable contexts.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">The type of the project builder.</typeparam>
    /// <param name="builder">The project builder to configure. Must not be <see langword="null"/>.</param>
    /// <param name="nullable">The nullable reference option to set.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public static T WithNullable<T>(this T builder, NullableOptions nullable)
        where T : class, IProjectBuilder
    {
        if (builder is ProjectBuilder projectBuilder)
        {
            var propertyItem = projectBuilder.GetOrAddPropertyGroupItem<NullableItem>();
            propertyItem.SetValue(nullable);
        }

        return builder;
    }

    /// <summary>
    /// Sets the target framework for the project.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The target framework determines which .NET runtime and APIs are available for the project.
    /// Common values include .NET 5, 6, 7, 8, and preview versions.
    /// </para>
    /// <para>
    /// CAUTION: This method overwrites any existing target framework setting.
    /// Use <see cref="WithTargetFrameworks{T}(T, TargetFramework[])"/> for multi-targeting.
    /// </para>
    /// <para>
    /// See <see href="https://learn.microsoft.com/en-us/dotnet/standard/frameworks"/>
    /// for detailed information on target frameworks.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">The type of the project builder.</typeparam>
    /// <param name="builder">The project builder to configure. Must not be <see langword="null"/>.</param>
    /// <param name="targetFramework">
    /// The target framework to set. For example, <see cref="TargetFramework.Net8"/>,
    /// <see cref="TargetFramework.Net9"/>, or custom frameworks created with <see cref="TargetFramework.Create"/>.
    /// </param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public static T WithTargetFramework<T>(this T builder, TargetFramework targetFramework)
        where T : class, IProjectBuilder
    {
        if (builder is ProjectBuilder projectBuilder)
        {
            var propertyItem = projectBuilder.GetOrAddPropertyGroupItem<TargetFrameworkItem>();

            propertyItem.SetValue(targetFramework);
        }

        return builder;
    }

    /// <summary>
    /// Sets multiple target frameworks for the project (multi-targeting).
    /// </summary>
    /// <remarks>
    /// <para>
    /// Multi-targeting allows a single project to build against multiple target frameworks.
    /// The build process creates separate assemblies for each framework.
    /// </para>
    /// <para>
    /// CAUTION: This method overwrites any existing target framework settings.
    /// </para>
    /// <para>
    /// See <see href="https://learn.microsoft.com/en-us/dotnet/standard/library-guidance/cross-platform-targeting"/>
    /// for guidance on multi-targeting strategies.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">The type of the project builder.</typeparam>
    /// <param name="builder">The project builder to configure. Must not be <see langword="null"/>.</param>
    /// <param name="targetFrameworks">
    /// One or more target frameworks to set. For example, <see cref="TargetFramework.Net6"/>,
    /// <see cref="TargetFramework.Net8"/>, and <see cref="TargetFramework.Net9"/>.
    /// </param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public static T WithTargetFrameworks<T>(this T builder, params TargetFramework[] targetFrameworks)
        where T : class, IProjectBuilder
    {
        if (builder is ProjectBuilder projectBuilder)
        {
            var propertyItem = projectBuilder.GetOrAddPropertyGroupItem<TargetFrameworkItem>();

            propertyItem.SetValues(targetFrameworks);
        }

        return builder;
    }

    /// <summary>
    /// Adds a NuGet package reference to the project.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method adds a PackageReference item to the project, enabling the project to consume
    /// a NuGet package and its dependencies. Package references are the modern way to manage
    /// dependencies in .NET projects.
    /// </para>
    /// <para>
    /// See <see href="https://learn.microsoft.com/en-us/nuget/consume-packages/package-references-in-project-files"/> for more information.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">The type of the project builder.</typeparam>
    /// <param name="builder">The project builder to add the package reference to. Must not be <see langword="null"/>.</param>
    /// <param name="name">The package ID of the NuGet package to reference. Must not be <see langword="null"/> or empty.</param>
    /// <param name="version">The optional version of the package to reference. When <see langword="null"/>, uses centrally managed version or latest version.</param>
    /// <param name="versionOverride">The optional version override for the package. Overrides centrally managed versions.</param>
    /// <param name="generatePathProperty">When <see langword="true"/>, generates a property containing the path to the package. Default is <see langword="false"/>.</param>
    /// <param name="aliases">The optional comma-separated list of alias names for the reference. See <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/extern-alias"/>.</param>
    /// <param name="includeAssets">The optional asset types to include from the package. When <see langword="null"/>, all assets are included.</param>
    /// <param name="excludeAssets">The optional asset types to exclude from the package. Takes precedence over <paramref name="includeAssets"/>.</param>
    /// <param name="privateAssets">The optional asset types that should not flow to consuming projects. Commonly used for analyzers and build tools.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> is <see langword="null"/> or whitespace.</exception>
    public static T AddPackageReference<T>(
        this T builder,
        string name,
        string? version = null,
        string? versionOverride = null,
        bool generatePathProperty = false,
        string? aliases = null,
        ReferenceAssets? includeAssets = null,
        ReferenceAssets? excludeAssets = null,
        ReferenceAssets? privateAssets = null
    )
        where T : class, IProjectBuilder
    {
        Argument.ThrowIfNullOrWhiteSpace(name);

        if (builder is ProjectBuilder projectBuilder)
        {
            var item = projectBuilder.GetOrAddItemGroupItem<PackageReferenceItem>();
            item.Include = name;
            item.Version = string.IsNullOrWhiteSpace(version) ? null : version;
            item.VersionOverride = string.IsNullOrWhiteSpace(versionOverride) ? null : versionOverride;
            item.GeneratePathProperty = generatePathProperty;
            item.Aliases = aliases;
            item.IncludeAssets = includeAssets;
            item.ExcludeAssets = excludeAssets;
            item.PrivateAssets = privateAssets;
        }
        return builder;
    }
}
