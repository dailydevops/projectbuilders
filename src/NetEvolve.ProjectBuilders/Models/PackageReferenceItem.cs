namespace NetEvolve.ProjectBuilders.Models;

using System.Reflection.Emit;
using System.Xml.Linq;
using NetEvolve.ProjectBuilders.Abstractions;
using NetEvolve.ProjectBuilders.Builders;
using NetEvolve.ProjectBuilders.Helpers;

/// <summary>
/// Represents a PackageReference item in a project file's ItemGroup.
/// </summary>
/// <remarks>
/// <para>
/// Package references represent NuGet package dependencies in a project. They specify the
/// external packages that the project depends on for compilation and runtime.
/// </para>
/// <para>
/// Package references use the "PackageReference" element in project files and are the modern
/// way to manage NuGet dependencies in SDK-style projects (as opposed to the older packages.config format).
/// </para>
/// <para>
/// The Include attribute typically contains the package name, optionally followed by a version specifier.
/// </para>
/// </remarks>
/// <seealso cref="IReference"/>
/// <seealso cref="ProjectReferenceItem"/>
/// <seealso cref="FrameworkReferenceItem"/>
internal sealed record PackageReferenceItem : IReference
{
    /// <inheritdoc/>
    public string Name => "PackageReference";

    /// <inheritdoc cref="IItemGroupItem.Include"/>
    /// <remarks>
    /// For package references, this typically contains the NuGet package name, such as "Newtonsoft.Json"
    /// or "Serilog".
    /// </remarks>
    public string? Include { get; set; }

    /// <inheritdoc cref="IReference.GeneratePathProperty"/>
    public bool GeneratePathProperty { get; set; }

    /// <inheritdoc cref="IReference.IncludeAssets"/>
    public ReferenceAssets? IncludeAssets { get; set; }

    /// <inheritdoc cref="IReference.ExcludeAssets"/>
    public ReferenceAssets? ExcludeAssets { get; set; }

    /// <inheritdoc cref="IReference.PrivateAssets"/>
    public ReferenceAssets? PrivateAssets { get; set; }

    public string? Version { get; set; }

    public string? VersionOverride
    {
        get;
        set
        {
            field = value;

            if (!string.IsNullOrWhiteSpace(field))
            {
                Version = null;
            }
        }
    }

    /// <inheritdoc cref="IReference.Aliases"/>
    public string? Aliases { get; set; }

    public XElement GetXElement()
    {
        var element = this.ToXElement();

        element.SetAttributeValue("Version", Version);
        element.SetAttributeValue("VersionOverride", VersionOverride);

        return element;
    }
}
