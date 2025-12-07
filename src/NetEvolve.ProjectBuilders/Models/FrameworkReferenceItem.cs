namespace NetEvolve.ProjectBuilders.Models;

using NetEvolve.ProjectBuilders.Abstractions;

internal sealed record FrameworkReferenceItem : IReference
{
    public string Name => "FrameworkReference";

    /// <inheritdoc cref="IItemGroupItem.Include"/>
    public string Include { get; } = default!;
}
