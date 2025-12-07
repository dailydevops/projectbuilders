namespace NetEvolve.ProjectBuilders.Models;

using System.Collections.Generic;
using System.IO;
using NetEvolve.ProjectBuilders.Abstractions;

internal sealed record ProjectReferenceItem : IReference
{
    public string Name => "ProjectReference";

    /// <inheritdoc cref="IItemGroupItem.Include"/>
    public string Include { get; } = default!;

    /// <inheritdoc cref="IReference.LookUpPaths"/>
    public IEnumerable<string> LookUpPaths
    {
        get
        {
            yield return Path.ChangeExtension(FullPath, ".nuspec");
            yield return FullPath;
        }
    }

    private string FullPath => Path.GetFullPath(Include);
}
