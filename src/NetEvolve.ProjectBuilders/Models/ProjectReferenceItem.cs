namespace NetEvolve.ProjectBuilders.Models;

using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using NetEvolve.ProjectBuilders.Abstractions;
using NetEvolve.ProjectBuilders.Builders;

/// <summary>
/// Represents a ProjectReference item in a project file's ItemGroup.
/// </summary>
/// <remarks>
/// <para>
/// Project references represent dependencies on other projects within the same solution or
/// local file system. They enable projects to reference each other's compiled outputs and
/// ensure proper build ordering.
/// </para>
/// <para>
/// Project references use the "ProjectReference" element in project files and include
/// the relative path to the referenced project file.
/// </para>
/// <para>
/// The Include attribute contains the path to the referenced project file, and the LookUpPaths
/// property provides paths for locating the project's outputs and specifications.
/// </para>
/// </remarks>
/// <seealso cref="IReference"/>
/// <seealso cref="PackageReferenceItem"/>
/// <seealso cref="FrameworkReferenceItem"/>
internal sealed record ProjectReferenceItem : IReference
{
    /// <inheritdoc/>
    public string Name => "ProjectReference";

    /// <inheritdoc cref="IItemGroupItem.Include"/>
    /// <remarks>
    /// For project references, this contains the relative path to the referenced project file,
    /// such as "../OtherProject/OtherProject.csproj".
    /// </remarks>
    public string Include { get; } = default!;

    /// <inheritdoc cref="IReference.GeneratePathProperty"/>
    public bool GeneratePathProperty => false;

    /// <inheritdoc cref="IReference.IncludeAssets"/>
    public ReferenceAssets? IncludeAssets => null;

    /// <inheritdoc cref="IReference.ExcludeAssets"/>
    public ReferenceAssets? ExcludeAssets => null;

    /// <inheritdoc cref="IReference.PrivateAssets"/>
    public ReferenceAssets? PrivateAssets => null;

    /// <inheritdoc cref="IReference.LookUpPaths"/>
    /// <remarks>
    /// Returns paths for both the .nuspec file (if the project is packaged as a NuGet) and
    /// the project file itself, enabling the test package builder to locate and process them.
    /// </remarks>
    public IEnumerable<string> LookUpPaths
    {
        get
        {
            yield return Path.ChangeExtension(FullPath, ".nuspec");
            yield return FullPath;
        }
    }

    private string FullPath => Path.GetFullPath(Include);

    /// <inheritdoc cref="IItemGroupItem.GetXElement"/>
    public XElement GetXElement() => this.ToXElement();
}
