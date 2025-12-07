namespace NetEvolve.ProjectBuilders.Models;

using Microsoft.Extensions.Primitives;
using NetEvolve.ProjectBuilders.Abstractions;

internal sealed record PropertyGroupItem : IPropertyGroupItem
{
    public string Name { get; }

    public StringValues Values { get; }

    public PropertyGroupItem(string key, string? value)
    {
        Name = key;
        Values = value;
    }
}
