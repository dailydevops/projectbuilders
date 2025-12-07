namespace NetEvolve.ProjectBuilders.Models;

using Microsoft.Extensions.Primitives;
using NetEvolve.ProjectBuilders.Abstractions;

internal sealed record NullableItem : IPropertyGroupItem<NullableOptions>
{
    public string Name => "Nullable";

    public StringValues Values { get; internal set; }

    public void SetValue(NullableOptions value) => Values = NullableItem.ConvertValue(value);

    public void SetValues(NullableOptions[] values) { }

    private static StringValues ConvertValue(NullableOptions value) =>
        value switch
        {
            NullableOptions.Enable => "enable",
            NullableOptions.Disable => "disable",
            NullableOptions.Warnings => "warnings",
            NullableOptions.Annotations => "annotations",
            _ => null,
        };
}
