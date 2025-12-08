namespace NetEvolve.ProjectBuilders.Models;

/// <summary>
/// Represents the types of assets that can be controlled in a package or project reference.
/// </summary>
/// <remarks>
/// <para>
/// This flags enumeration is used with <see cref="Abstractions.IReference.IncludeAssets"/>,
/// <see cref="Abstractions.IReference.ExcludeAssets"/>, and <see cref="Abstractions.IReference.PrivateAssets"/>
/// to control which assets from a NuGet package are included, excluded, or kept private.
/// </para>
/// <para>
/// See <see href="https://learn.microsoft.com/en-us/nuget/consume-packages/package-references-in-project-files#controlling-dependency-assets"/> for more information.
/// </para>
/// </remarks>
[Flags]
public enum ReferenceAssets
{
    /// <summary>
    /// No assets are included or referenced.
    /// </summary>
    None = 0,

    /// <summary>
    /// Assets in the lib folder that are used during compilation.
    /// </summary>
    Compile = 1 << 0,

    /// <summary>
    /// Assets in the lib and runtimes folders that are copied to the output directory.
    /// </summary>
    Runtime = 1 << 1,

    /// <summary>
    /// Assets in the contentFiles folder that are available to the project.
    /// </summary>
    ContentFiles = 1 << 2,

    /// <summary>
    /// Assets in the build folder, including MSBuild props and targets files.
    /// </summary>
    Build = 1 << 3,

    /// <summary>
    /// Assets in the buildMultitargeting folder used for cross-targeting scenarios.
    /// </summary>
    BuildMultiTargeting = 1 << 4,

    /// <summary>
    /// Assets in the buildTransitive folder that flow transitively to consuming projects.
    /// </summary>
    BuildTransitive = 1 << 5,

    /// <summary>
    /// Roslyn analyzer assemblies in the analyzers folder.
    /// </summary>
    Analyzers = 1 << 6,

    /// <summary>
    /// Native assets in the native folder.
    /// </summary>
    Native = 1 << 7,

    /// <summary>
    /// All asset types combined.
    /// </summary>
    All = Compile | Runtime | ContentFiles | Build | BuildMultiTargeting | BuildTransitive | Analyzers | Native,
}
