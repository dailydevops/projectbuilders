namespace NetEvolve.ProjectBuilders.Builders;

using System;
using System.Diagnostics.CodeAnalysis;
using NetEvolve.Arguments;
using NetEvolve.ProjectBuilders.Abstractions;

/// <summary>
/// Provides extension methods for <see cref="IProjectFactory"/> to simplify project creation.
/// </summary>
/// <remarks>
/// <para>
/// This static class provides convenient factory methods for creating the most common project types:
/// C# projects, VB.NET projects, and global.json configuration files.
/// </para>
/// <para>
/// Each method uses the fluent API pattern and accepts optional configuration actions for
/// further customization of the created builders.
/// </para>
/// </remarks>
[SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "As designed.")]
public static class ProjectFactoryExtensions
{
    /// <summary>
    /// Adds a C# project to the factory.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method creates a C# project file (.csproj) with default configuration and registers it
    /// with the factory. The created project can be further configured using the optional configuration action.
    /// </para>
    /// <para>
    /// The project is configured to use the Microsoft.NET.Sdk by default.
    /// </para>
    /// </remarks>
    /// <param name="factory">
    /// The factory object to which the project will be added. Must not be <see langword="null"/>.
    /// </param>
    /// <param name="configure">
    /// An optional configuration action that receives the <see cref="IProjectBuilder"/> for the
    /// created project, allowing for customization. Can be <see langword="null"/> to skip configuration.
    /// </param>
    /// <returns>
    /// The factory object for fluent method chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="factory"/> is <see langword="null"/>.
    /// </exception>
    /// <example>
    /// <code>
    /// factory.AddCSharpProject(pb => pb
    ///     .WithDefaults()
    ///     .WithTargetFramework(TargetFramework.Net8)
    ///     .AddCSharpFile("Program.cs", "// Hello World"));
    /// </code>
    /// </example>
    public static IProjectFactory AddCSharpProject(
        this IProjectFactory factory,
        Action<IProjectBuilder>? configure = null
    )
    {
        ArgumentNullException.ThrowIfNull(factory);

        var builder = factory.AddFileBuilder<IProjectBuilder, ProjectBuilder>(
            new ProjectBuilder(factory.DirectoryBuilder, Constants.CSharpProjectFileName)
        );

        configure?.Invoke(builder);

        return factory;
    }

    /// <summary>
    /// Adds a global.json SDK configuration file to the factory.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method creates a global.json file that specifies the .NET SDK version and roll-forward
    /// policy for the project. The file is registered with the factory and can be further customized.
    /// </para>
    /// <para>
    /// The global.json file is useful for ensuring all developers and CI/CD systems use the same
    /// SDK version when building the project.
    /// </para>
    /// <para>
    /// See <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/global-json"/> for more information.
    /// </para>
    /// </remarks>
    /// <param name="factory">
    /// The factory object to which the file will be added. Must not be <see langword="null"/>.
    /// </param>
    /// <param name="runtimeVersion">
    /// The .NET SDK version to specify in the global.json file. Defaults to <see cref="Constants.RuntimeSdkDefault"/>
    /// if not provided. Must not be <see langword="null"/>, empty, or whitespace.
    /// </param>
    /// <param name="configure">
    /// An optional configuration action that receives the <see cref="IGlobalJsonBuilder"/> for the
    /// created file, allowing for customization. Can be <see langword="null"/> to skip configuration.
    /// </param>
    /// <returns>
    /// The factory object for fluent method chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="factory"/> or <paramref name="runtimeVersion"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="runtimeVersion"/> is empty or contains only whitespace.
    /// </exception>
    /// <example>
    /// <code>
    /// factory.AddGlobalJson(
    ///     runtimeVersion: "8.0.204",
    ///     configure: gb => gb
    ///         .SetAllowPrerelease(false)
    ///         .SetRollForward(RollForward.LatestMinor));
    /// </code>
    /// </example>
    public static IProjectFactory AddGlobalJson(
        this IProjectFactory factory,
        string runtimeVersion = Constants.RuntimeSdkDefault,
        Action<IGlobalJsonBuilder>? configure = null
    )
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentException.ThrowIfNullOrWhiteSpace(runtimeVersion);

        var builder = factory.AddFileBuilder<IGlobalJsonBuilder, GlobalJsonBuilder>(
            new GlobalJsonBuilder(factory.DirectoryBuilder, runtimeVersion)
        );

        configure?.Invoke(builder);

        return factory;
    }

    /// <summary>
    /// Adds a VB.NET project to the factory.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method creates a VB.NET project file (.vbproj) with default configuration and registers it
    /// with the factory. The created project can be further configured using the optional configuration action.
    /// </para>
    /// <para>
    /// The project is configured to use the Microsoft.NET.Sdk by default.
    /// </para>
    /// </remarks>
    /// <param name="factory">
    /// The factory object to which the project will be added. Must not be <see langword="null"/>.
    /// </param>
    /// <param name="configure">
    /// An optional configuration action that receives the <see cref="IProjectBuilder"/> for the
    /// created project, allowing for customization. Can be <see langword="null"/> to skip configuration.
    /// </param>
    /// <returns>
    /// The factory object for fluent method chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="factory"/> is <see langword="null"/>.
    /// </exception>
    /// <example>
    /// <code>
    /// factory.AddVBProject(pb => pb
    ///     .WithDefaults()
    ///     .WithTargetFramework(TargetFramework.Net8));
    /// </code>
    /// </example>
    public static IProjectFactory AddVBProject(this IProjectFactory factory, Action<IProjectBuilder>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(factory);

        var builder = factory.AddFileBuilder<IProjectBuilder, ProjectBuilder>(
            new ProjectBuilder(factory.DirectoryBuilder, Constants.VBNetProjectFileName)
        );

        configure?.Invoke(builder);

        return factory;
    }
}
