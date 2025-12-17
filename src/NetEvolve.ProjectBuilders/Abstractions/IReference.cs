namespace NetEvolve.ProjectBuilders.Abstractions;

using System.Collections.Generic;
using NetEvolve.ProjectBuilders.Models;

/// <summary>
/// Represents an item that provides lookup paths for resolving referenced packages or projects.
/// </summary>
/// <remarks>
/// <para>
/// This interface extends <see cref="IItemGroupItem"/> to add the concept of lookup paths,
/// which are file system directories where the referenced item's metadata (like .nuspec files)
/// can be found. This is used during test setup to locate and process package or project references.
/// </para>
/// <para>
/// Implementations include <see cref="PackageReferenceItem"/> and <see cref="ProjectReferenceItem"/>,
/// which provide paths for locating NuGet package specifications and project files respectively.
/// </para>
/// </remarks>
/// <seealso cref="IItemGroupItem"/>
/// <seealso cref="PackageReferenceItem"/>
/// <seealso cref="ProjectReferenceItem"/>
/// <seealso cref="FrameworkReferenceItem"/>
public interface IReference : IItemGroupItem
{
    /// <summary>
    /// Gets a value indicating whether MSBuild should generate a property containing the resolved package path for this reference.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When set to <see langword="true"/>, MSBuild generates a property named <c>Pkg{PackageId}</c> (with dots replaced by underscores)
    /// that contains the full path to the package folder. This allows you to reference files from the package in your build process.
    /// </para>
    /// <para>
    /// This property is particularly useful when you need to access package content files, tools, or other resources during the build.
    /// The generated property can be used in MSBuild targets, tasks, or other build-time operations.
    /// </para>
    /// <para>
    /// For more information about this feature, see
    /// <see href="https://learn.microsoft.com/en-us/nuget/consume-packages/package-references-in-project-files#generatepathproperty">GeneratePathProperty documentation</see>.
    /// </para>
    /// </remarks>
    /// <value>
    /// <see langword="true"/> if MSBuild should generate a path property for this package reference; otherwise, <see langword="false"/>.
    /// </value>
    /// <seealso href="https://learn.microsoft.com/en-us/nuget/consume-packages/package-references-in-project-files#generatepathproperty"/>
    bool GeneratePathProperty { get; }

    /// <summary>
    /// Gets the collection of directory paths used for lookup operations.
    /// </summary>
    /// <remarks>
    /// <para>
    /// These paths are used to locate referenced resources such as NuGet packages or project files.
    /// For project references, paths might include both the .nuspec file location and the project file path.
    /// </para>
    /// <para>
    /// The default implementation returns an empty collection, allowing subclasses to override with
    /// appropriate paths based on their reference type.
    /// </para>
    /// </remarks>
    /// <returns>
    /// An enumerable of absolute file system paths where referenced resources can be found.
    /// Returns an empty enumeration if no lookup paths are applicable.
    /// </returns>
    IEnumerable<string> LookUpPaths => [];

    /// <summary>
    /// Gets the optional alias name(s) to use for referencing this reference in code.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Aliases allow you to reference the same assembly using different names to avoid naming conflicts.
    /// Multiple aliases can be specified by separating them with commas.
    /// </para>
    /// <para>
    /// See <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/extern-alias"/> for more information on extern aliases.
    /// </para>
    /// </remarks>
    /// <value>A comma-separated list of alias names, or <see langword="null"/> if no aliases are defined.</value>
    string? Aliases { get; }

    /// <summary>
    /// Gets the asset types to include from this reference.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Controls which assets from the package are included in the build. When <see langword="null"/>, all assets are included by default.
    /// </para>
    /// <para>
    /// See <see href="https://learn.microsoft.com/en-us/nuget/consume-packages/package-references-in-project-files#controlling-dependency-assets"/> for more information.
    /// </para>
    /// </remarks>
    /// <value>A flags enumeration of <see cref="ReferenceAssets"/> values, or <see langword="null"/> to include all assets.</value>
    ReferenceAssets? IncludeAssets { get; }

    /// <summary>
    /// Gets the asset types to exclude from this reference.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Controls which assets from the package are excluded from the build. Takes precedence over <see cref="IncludeAssets"/>.
    /// </para>
    /// <para>
    /// See <see href="https://learn.microsoft.com/en-us/nuget/consume-packages/package-references-in-project-files#controlling-dependency-assets"/> for more information.
    /// </para>
    /// </remarks>
    /// <value>A flags enumeration of <see cref="ReferenceAssets"/> values, or <see langword="null"/> to not exclude any assets.</value>
    ReferenceAssets? ExcludeAssets { get; }

    /// <summary>
    /// Gets the asset types that flow privately and are not exposed to dependent projects.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Controls which assets from the package do not flow to consuming projects. Commonly used with analyzer packages or build tools.
    /// </para>
    /// <para>
    /// See <see href="https://learn.microsoft.com/en-us/nuget/consume-packages/package-references-in-project-files#controlling-dependency-assets"/> for more information.
    /// </para>
    /// </remarks>
    /// <value>A flags enumeration of <see cref="ReferenceAssets"/> values, or <see langword="null"/> to allow all assets to flow.</value>
    ReferenceAssets? PrivateAssets { get; }
}
