namespace NetEvolve.ProjectBuilders.Abstractions;

using System.Xml.Linq;
using NetEvolve.ProjectBuilders.Helpers;

/// <summary>
/// Represents an item element within an ItemGroup in an MSBuild project file.
/// </summary>
/// <remarks>
/// <para>
/// Item group items are the fundamental units of information that get passed to MSBuild tasks.
/// Each item can represent various types of project content such as package references,
/// project references, assembly references, or source code files.
/// </para>
/// <para>
/// Items have metadata that can be set through properties, and they support conditional evaluation
/// through the Condition attribute and organizational grouping through the Label attribute.
/// </para>
/// <para>
/// See <see href="https://learn.microsoft.com/en-us/visualstudio/msbuild/item-element-msbuild"/> for more information.
/// </para>
/// </remarks>
public interface IItemGroupItem
{
    /// <summary>
    /// Gets the name (element type) of the item group item.
    /// </summary>
    /// <remarks>
    /// The name determines the type of item within the project. Common names include:
    /// <list type="bullet">
    /// <item><description>PackageReference - NuGet package dependencies</description></item>
    /// <item><description>ProjectReference - Local project dependencies</description></item>
    /// <item><description>Reference - Assembly references</description></item>
    /// <item><description>FrameworkReference - Built-in framework references</description></item>
    /// </list>
    /// </remarks>
    /// <value>The item type name as a string.</value>
    string Name { get; }

    /// <summary>
    /// Gets the optional condition that must evaluate to true for this item to be included.
    /// </summary>
    /// <remarks>
    /// This allows for conditional inclusion of items based on project properties, target frameworks,
    /// platforms, or other build conditions. When <see langword="null"/>, the item is always included.
    /// </remarks>
    /// <value>A condition string (e.g., "'$(TargetFramework)' == 'net8.0'"), or <see langword="null"/> if no condition is set.</value>
    string? Condition => null;

    /// <summary>
    /// Gets the optional label for organizing and identifying related items.
    /// </summary>
    /// <remarks>
    /// Labels are optional tags that can be used to organize items logically or identify groups
    /// of related items. They don't affect the build process but can be useful for documentation
    /// and organization of complex project files.
    /// </remarks>
    /// <value>A descriptive label string, or <see langword="null"/> if no label is set.</value>
    string? Label => null;

    /// <summary>
    /// Gets the value to include in the item.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The Include attribute specifies what the item refers to. For package references, this would be
    /// the package name; for project references, it would be the project file path; for file items,
    /// it would be the file path pattern.
    /// </para>
    /// <para>
    /// Wildcards and property references (e.g., $(SomeProperty)) are typically supported depending
    /// on the item type.
    /// </para>
    /// </remarks>
    /// <value>The include value as a string, typically a package name or file path.</value>
    string? Include { get; }

    XElement GetXElement();
}
