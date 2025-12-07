namespace NetEvolve.ProjectBuilders.Abstractions;

using System.Collections.Generic;

public interface IReference : IItemGroupItem
{
    IEnumerable<string> LookUpPaths => [];
}
