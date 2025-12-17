namespace NetEvolve.ProjectBuilders.XUnit.Tests.Integration;

using System;

internal static class ProjectHelpers
{
    public static bool DirectoryFilter(string path)
    {
        // Path contains `obj/` or `bin/` folders are ignored
        if (
            path.Contains(
                $"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}",
                StringComparison.OrdinalIgnoreCase
            )
            || path.Contains(
                $"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}",
                StringComparison.OrdinalIgnoreCase
            )
        )
        {
            return false;
        }

        // Path ends with `.sarif` files are ignored
        if (path.EndsWith(".sarif", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return true;
    }
}
