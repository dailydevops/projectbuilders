namespace NetEvolve.ProjectBuilders.Models;

using NetEvolve.ProjectBuilders.Abstractions;

/// <summary>
/// Represents a FrameworkReference item in a project file's ItemGroup.
/// </summary>
/// <remarks>
/// <para>
/// Framework references are built-in references to .NET framework assemblies that are included
/// with the SDK. Examples include System.Net.Http, System.Reflection, and other standard libraries.
/// </para>
/// <para>
/// Framework references use the "FrameworkReference" element in project files and are different
/// from Package References (NuGet packages) or Project References (other projects).
/// </para>
/// </remarks>
/// <seealso cref="IReference"/>
/// <seealso cref="PackageReferenceItem"/>
/// <seealso cref="ProjectReferenceItem"/>
internal sealed record FrameworkReferenceItem : IReference
{
    /// <inheritdoc/>
    public string Name => "FrameworkReference";

    /// <inheritdoc cref="IItemGroupItem.Include"/>
    public string Include { get; } = default!;
}
