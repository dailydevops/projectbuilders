namespace NetEvolve.ProjectBuilders.Abstractions;

using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Represents the base abstraction for all object builders in the project creation pipeline.
/// </summary>
/// <remarks>
/// <para>
/// This interface defines the core contract for objects that can create and manage
/// file system artifacts asynchronously. All builders in the library implement this interface
/// to ensure consistent lifecycle management and resource cleanup.
/// </para>
/// <para>
/// Implementations are expected to provide proper resource disposal through the inherited
/// <see cref="IAsyncDisposable"/> interface, ensuring that all created temporary files and
/// directories are properly cleaned up.
/// </para>
/// </remarks>
public interface IObjectBuilder : IAsyncDisposable
{
    /// <summary>
    /// Gets the full path of the created object in the file system.
    /// </summary>
    /// <value>An absolute path to the object being built.</value>
    string FullPath { get; }

    /// <summary>
    /// Asynchronously creates the object and writes it to the file system.
    /// </summary>
    /// <remarks>
    /// This method is responsible for all I/O operations required to persist the object
    /// to the file system. Implementations should be exception-safe and properly handle
    /// cancellation tokens.
    /// </remarks>
    /// <param name="cancellationToken">
    /// A token that can be used to request cancellation of the creation operation.
    /// </param>
    /// <returns>
    /// A <see cref="ValueTask"/> representing the asynchronous operation.
    /// </returns>
    ValueTask CreateAsync(CancellationToken cancellationToken = default);
}
