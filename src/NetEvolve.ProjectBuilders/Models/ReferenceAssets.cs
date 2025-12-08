namespace NetEvolve.ProjectBuilders.Models;

/// <see href="https://learn.microsoft.com/en-us/nuget/consume-packages/package-references-in-project-files#controlling-dependency-assets" />
[Flags]
public enum ReferenceAssets
{
    None = 0,
    Compile = 1 << 0,
    Runtime = 1 << 1,
    ContentFiles = 1 << 2,
    Build = 1 << 3,
    BuildMultiTargeting = 1 << 4,
    BuildTransitive = 1 << 5,
    Analyzers = 1 << 6,
    Native = 1 << 7,
    All = Compile | Runtime | ContentFiles | Build | BuildMultiTargeting | BuildTransitive | Analyzers | Native,
}
