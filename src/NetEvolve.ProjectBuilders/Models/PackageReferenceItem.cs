namespace NetEvolve.ProjectBuilders.Models;

using NetEvolve.ProjectBuilders.Abstractions;

internal sealed record PackageReferenceItem : IReference
{
    public string Name => "PackageReference";

    /// <inheritdoc cref="IItemGroupItem.Include"/>
    public string Include { get; } = default!;
}
