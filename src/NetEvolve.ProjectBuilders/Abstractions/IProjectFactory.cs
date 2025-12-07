namespace NetEvolve.ProjectBuilders.Abstractions;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.ProjectBuilders.Models.Output;

/// <summary>
/// Allows you to create a .NET project and build it.
/// </summary>
public interface IProjectFactory : IAsyncDisposable
{
    /// <summary>
    /// Allows you to add an environment variable to the project execution. If the environment variable already exists, it will be updated.
    /// </summary>
    /// <param name="name">The name of the environment variable.</param>
    /// <param name="value">The value of the environment variable. A value of <see langword="null"/> is possible.</param>
    /// <returns>Fluent API</returns>
    IProjectFactory AddEnvironmentVariable(string name, string? value);

    /// <summary>
    /// Allows you to add multiple environment variables to the project execution. If an environment variable already exists, it will be updated.
    /// </summary>
    /// <param name="variables">
    /// The environment variables to add. Must not be <see langword="null"/>.
    /// </param>
    /// <returns>Fluent API</returns>
    IProjectFactory AddEnvironmentVariables(params KeyValuePair<string, string?>[] variables);

    /// <summary>
    /// Extensibility point to add a specific file Builder.
    /// </summary>
    /// <typeparam name="TInterface">The type if the file builder interface.</typeparam>
    /// <typeparam name="TImplementation">The type of the file builder.</typeparam>
    /// <returns>Fluent API</returns>
    TInterface AddFileBuilder<TInterface, TImplementation>(TImplementation builder)
        where TInterface : IFileBuilder
        where TImplementation : class, TInterface;

    /// <summary>
    /// Creates the whole project, builds it and catches the output, for later use while testing.
    /// </summary>
    /// <param name="args">Optional arguments to pass to the project builder.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A <see cref="ValueTask{TResult}"/> representing the asynchronous operation. The result contains the SARIF file.
    /// </returns>
    ValueTask<OutputFile> BuildAsync(string[]? args = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the directory builder.
    /// </summary>
    ISubdirectoryBuilder DirectoryBuilder { get; }
}
