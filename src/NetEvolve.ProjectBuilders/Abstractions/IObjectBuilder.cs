namespace NetEvolve.ProjectBuilders.Abstractions;

using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Represents an abstract object builder.
/// </summary>
public interface IObjectBuilder : IAsyncDisposable
{
    /// <summary>
    /// Full path of the object.
    /// </summary>
    string FullPath { get; }

    /// <summary>Executes the creation of the object.</summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    ValueTask CreateAsync(CancellationToken cancellationToken = default);
}
