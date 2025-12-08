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
    /// <see href="https://learn.microsoft.com/en-us/nuget/consume-packages/package-references-in-project-files#generatepathproperty" />
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

    string? Aliases { get; }

    ReferenceAssets? IncludeAssets { get; }
    ReferenceAssets? ExcludeAssets { get; }
    ReferenceAssets? PrivateAssets { get; }
}
