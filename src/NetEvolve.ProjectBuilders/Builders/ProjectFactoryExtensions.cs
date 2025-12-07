namespace NetEvolve.ProjectBuilders.Builders;

using System;
using System.Diagnostics.CodeAnalysis;
using NetEvolve.Arguments;
using NetEvolve.ProjectBuilders.Abstractions;

/// <summary>
/// Provides extension methods for the <see cref="IProjectFactory"/> interface.
/// </summary>
[SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "As designed.")]
public static class ProjectFactoryExtensions
{
    /// <summary>
    /// Adds a <b>C#</b> project to the factory.
    /// </summary>
    /// <param name="factory">
    /// The factory object, which will be used to add the project.
    /// </param>
    /// <param name="configure">
    /// An optional configuration action to configure the project.
    /// </param>
    /// <returns>
    /// The factory object, for fluent chaining.
    /// </returns>
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
    /// Adds a <i>global.json</i> file to the factory.
    /// </summary>
    /// <param name="factory">
    /// The factory object, which will be used to add the file.
    /// </param>
    /// <param name="runtimeVersion">
    /// The runtime version to use in the file.
    /// </param>
    /// <param name="configure">
    /// An optional configuration action to configure the file.
    /// </param>
    /// <returns>
    /// The factory object, for fluent chaining.
    /// </returns>
    public static IProjectFactory AddGlobalJson(
        this IProjectFactory factory,
        string runtimeVersion = Constants.RuntimeSdkDefault,
        Action<IGlobalJsonBuilder>? configure = null
    )
    {
        ArgumentNullException.ThrowIfNull(factory);
        Argument.ThrowIfNullOrWhiteSpace(runtimeVersion);

        var builder = factory.AddFileBuilder<IGlobalJsonBuilder, GlobalJsonBuilder>(
            new GlobalJsonBuilder(factory.DirectoryBuilder, runtimeVersion)
        );

        configure?.Invoke(builder);

        return factory;
    }

    /// <summary>
    /// Adds a <b>VB.NET</b> project to the factory.
    /// </summary>
    /// <param name="factory">
    /// The factory object, which will be used to add the project.
    /// </param>
    /// <param name="configure">
    /// An optional configuration action to configure the project.
    /// </param>
    /// <returns>
    /// The factory object, for fluent chaining.
    /// </returns>
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
