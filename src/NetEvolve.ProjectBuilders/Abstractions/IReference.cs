namespace NetEvolve.ProjectBuilders.Abstractions;

using System;
using System.Collections.Generic;

public interface IReference : IItemGroupItem
{
    IEnumerable<string> LookUpPaths => Array.Empty<string>();
}
