namespace NetEvolve.ProjectBuilders.Abstractions;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Models.Output;

/// <summary>
/// Provides a fluent API for creating, configuring, and building .NET projects for testing.
/// </summary>
/// <remarks>
/// <para>
/// This interface is the primary entry point for the ProjectBuilders library. It enables
/// the creation of test projects with specific configurations, from basic structure to complex
/// multi-framework setups with custom properties and references.
/// </para>
/// <para>
/// The factory manages the complete lifecycle:
/// <list type="bullet">
/// <item><description>Creating a temporary, isolated directory structure</description></item>
/// <item><description>Adding and configuring project files (C#, VB.NET)</description></item>
/// <item><description>Adding configuration files (global.json)</description></item>
/// <item><description>Setting environment variables for build execution</description></item>
/// <item><description>Executing dotnet build and restore commands</description></item>
/// <item><description>Capturing and analyzing SARIF diagnostic output</description></item>
/// </list>
/// </para>
/// <para>
/// Usage example:
/// <code>
/// using var factory = ProjectFactory.Create();
/// factory
///     .AddCSharpProject(pb => pb
///         .WithDefaults()
///         .WithTargetFramework(TargetFramework.Net8))
///     .AddGlobalJson(configure: gb => gb.WithDefaults())
///     .AddEnvironmentVariable("MYVAR", "myvalue");
///
/// var result = await factory.BuildAsync();
/// if (result.HasErrors())
/// {
///     foreach (var error in result.Results.Where(r => r.Level == "error"))
///     {
///         Console.WriteLine($"{error.RuleId}: {error.Message}");
///     }
/// }
/// </code>
/// </para>
/// </remarks>
/// <seealso cref="ProjectFactory"/>
public interface IProjectFactory : IAsyncDisposable
{
    /// <summary>
    /// Adds or updates an environment variable for project execution.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Environment variables set through this method are passed to the dotnet build and restore
    /// commands when the project is built. If the variable already exists, its value is updated.
    /// </para>
    /// <para>
    /// Some variables are pre-configured for test execution (CI, DOTNET_CLI_TELEMETRY_OPTOUT, etc.)
    /// and can be overridden with this method.
    /// </para>
    /// </remarks>
    /// <param name="name">
    /// The name of the environment variable. Must not be <see langword="null"/> or whitespace.
    /// </param>
    /// <param name="value">
    /// The value of the environment variable. Can be <see langword="null"/> to set the variable
    /// without a value, or any string value.
    /// </param>
    /// <returns>The current <see cref="IProjectFactory"/> instance for fluent chaining.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="name"/> is <see langword="null"/>, empty, or whitespace.
    /// </exception>
    IProjectFactory AddEnvironmentVariable(string name, string? value);

    /// <summary>
    /// Adds multiple environment variables for project execution at once.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is a convenience method for adding multiple environment variables in a single call.
    /// It's equivalent to calling <see cref="AddEnvironmentVariable(string, string?)"/> multiple times.
    /// </para>
    /// <para>
    /// If any variable already exists, it is updated with the new value.
    /// </para>
    /// </remarks>
    /// <param name="variables">
    /// An array of key-value pairs representing environment variables to add.
    /// Must not be <see langword="null"/>. Individual values can be <see langword="null"/>.
    /// </param>
    /// <returns>The current <see cref="IProjectFactory"/> instance for fluent chaining.</returns>
    IProjectFactory AddEnvironmentVariables(params KeyValuePair<string, string?>[] variables);

    /// <summary>
    /// Adds a custom file builder to the project factory.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This extensibility point allows registration of custom <see cref="IFileBuilder"/> implementations
    /// to support specialized file generation beyond the built-in C#/VB.NET projects and global.json files.
    /// </para>
    /// <para>
    /// All registered builders are created when <see cref="BuildAsync(string[], CancellationToken)"/> is called.
    /// Attempting to add the same builder instance twice throws an exception.
    /// </para>
    /// </remarks>
    /// <typeparam name="TInterface">
    /// The interface type that the builder implements. Should be <see cref="IFileBuilder"/> or a derived interface.
    /// </typeparam>
    /// <typeparam name="TImplementation">
    /// The concrete implementation type. Must implement <typeparamref name="TInterface"/>.
    /// </typeparam>
    /// <param name="builder">
    /// The configured builder instance to register. Must not be <see langword="null"/>.
    /// </param>
    /// <returns>
    /// The builder instance cast to <typeparamref name="TInterface"/>, allowing for further
    /// configuration or fluent chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="builder"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the builder instance has already been registered.
    /// </exception>
    TInterface AddFileBuilder<TInterface, TImplementation>(TImplementation builder)
        where TInterface : IFileBuilder
        where TImplementation : class, TInterface;

    /// <summary>
    /// Creates the entire project structure, executes the build, and returns the SARIF diagnostic output.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method orchestrates the complete build process:
    /// <list type="number">
    /// <item><description>Creates all registered file builders (projects, configuration files, etc.)</description></item>
    /// <item><description>Executes dotnet restore to restore NuGet packages</description></item>
    /// <item><description>Executes dotnet build with optional additional arguments</description></item>
    /// <item><description>Captures build output and diagnostic information</description></item>
    /// <item><description>Parses the generated SARIF file for diagnostic results</description></item>
    /// <item><description>Enriches SARIF results with diagnostic information from build output</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// At least one project builder must be registered before calling this method, otherwise
    /// an <see cref="ArgumentException"/> is thrown.
    /// </para>
    /// </remarks>
    /// <param name="args">
    /// Optional command-line arguments to pass to the dotnet build command. Can be <see langword="null"/>
    /// or empty to use default build behavior. Examples: "/p:Configuration=Release", "/p:Platform=x64".
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the build operation to complete. Allows cancellation
    /// of long-running builds.
    /// </param>
    /// <returns>
    /// A <see cref="ValueTask{TResult}"/> that returns an <see cref="OutputFile"/> containing
    /// the parsed SARIF diagnostic information from the build, including any errors or warnings.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when no project builders have been registered or no project file has been added.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when the build operation is cancelled via the cancellation token.
    /// </exception>
    ValueTask<OutputFile> BuildAsync(string[]? args = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the temporary directory builder used by this factory.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This directory serves as the root for all project files, configuration files, and other
    /// outputs created during the project building process. It's automatically created when the
    /// first file is created and cleaned up when the factory is disposed.
    /// </para>
    /// <para>
    /// Direct access to the directory builder allows advanced scenarios where custom files need
    /// to be created or managed outside the standard builder pattern.
    /// </para>
    /// </remarks>
    /// <value>
    /// An <see cref="ISubdirectoryBuilder"/> representing the temporary directory for this factory.
    /// </value>
    ISubdirectoryBuilder DirectoryBuilder { get; }
}
