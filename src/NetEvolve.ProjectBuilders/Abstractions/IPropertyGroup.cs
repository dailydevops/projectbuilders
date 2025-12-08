namespace NetEvolve.ProjectBuilders.Abstractions;

using System.Collections.ObjectModel;

/// <summary>
/// Represents a PropertyGroup element in an MSBuild project file.
/// </summary>
/// <remarks>
/// <para>
/// PropertyGroup elements contain project properties that define build behavior and metadata.
/// Properties are key-value pairs that configure the build system, affect compilation, and
/// control the packaging and deployment of projects.
/// </para>
/// <para>
/// Common properties include TargetFramework, Configuration, Nullable, AssemblyVersion,
/// and many others that influence how projects are built and packaged.
/// </para>
/// <para>
/// See <see href="https://learn.microsoft.com/en-us/visualstudio/msbuild/propertygroup-element-msbuild"/> for more information.
/// </para>
/// </remarks>
public interface IPropertyGroup
{
    /// <summary>
    /// Gets a read-only collection of properties in this property group.
    /// </summary>
    /// <value>A read-only collection of <see cref="IPropertyGroupItem"/> objects.</value>
    ReadOnlyCollection<IPropertyGroupItem> Items { get; }

    /// <summary>
    /// Adds a new property of the specified type to this property group.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the property to add. Must be a non-abstract class implementing <see cref="IPropertyGroupItem"/>
    /// with a public parameterless constructor.
    /// </typeparam>
    /// <returns>
    /// The newly created and added property instance.
    /// </returns>
    T Add<T>()
        where T : class, IPropertyGroupItem, new();
}
