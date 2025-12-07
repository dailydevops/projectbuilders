namespace NetEvolve.ProjectBuilders.Models;

using System.Linq;
using Microsoft.Extensions.Primitives;
using NetEvolve.ProjectBuilders.Abstractions;

internal sealed record TargetFrameworkItem : IPropertyGroupItem<TargetFramework>
{
    public string Name => Values.Count > 1 ? "TargetFrameworks" : "TargetFramework";

    public StringValues Values { get; internal set; }

    public void SetValue(TargetFramework value) => Values = value.Value;

    public void SetValues(TargetFramework[] values) =>
        Values = StringValues.Concat(Values, values.Select(x => x.Value).ToArray());
}
