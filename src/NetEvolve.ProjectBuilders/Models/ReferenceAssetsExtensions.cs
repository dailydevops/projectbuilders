namespace NetEvolve.ProjectBuilders.Models;

internal static class ReferenceAssetsExtensions
{
    public static string? GetValue(this ReferenceAssets? assets)
    {
        if (assets is null || assets == ReferenceAssets.None)
        {
            return null;
        }

        if (assets == ReferenceAssets.All)
        {
            return "all";
        }

#pragma warning disable CA1308 // Normalize strings to uppercase
        var parts = Enum.GetValues<ReferenceAssets>()
            .Where(a => a != ReferenceAssets.None && assets.Value.HasFlag(a))
            .Select(a => a.ToString().ToLowerInvariant());
#pragma warning restore CA1308 // Normalize strings to uppercase

        return string.Join(';', parts);
    }
}
