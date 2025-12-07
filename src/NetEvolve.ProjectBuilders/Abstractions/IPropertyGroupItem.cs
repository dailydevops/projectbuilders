namespace NetEvolve.ProjectBuilders.Abstractions;

using System.Linq;
using Microsoft.Extensions.Primitives;

/// <summary>
/// Represents a property element within a PropertyGroup in an MSBuild project file.
/// </summary>
/// <remarks>
/// <para>
/// Properties are name-value pairs that control build behavior. Unlike items, properties are
/// scalar values (though they can contain semicolon-separated lists). Properties affect how
/// the compiler is invoked, what frameworks are targeted, and many other aspects of the build.
/// </para>
/// <para>
/// Properties can be conditional, labeled for organization, and can reference other properties
/// or environment variables using MSBuild syntax $(PropertyName).
/// </para>
/// <para>
/// See <see href="https://learn.microsoft.com/en-us/visualstudio/msbuild/property-element-msbuild"/> for more information.
/// </para>
/// </remarks>
public interface IPropertyGroupItem
{
    /// <summary>
    /// Gets the name of the property.
    /// </summary>
    /// <remarks>
    /// The property name is the element type in the project file. Examples include
    /// TargetFramework, Nullable, Configuration, AssemblyVersion, and many others.
    /// </remarks>
    /// <value>The property name as a string.</value>
    string Name { get; }

    /// <summary>
    /// Gets the values assigned to this property.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Although most properties have a single value, this interface supports multiple values
    /// which are typically separated by semicolons when serialized to the project file.
    /// </para>
    /// <para>
    /// Empty or unset values are represented by <see cref="StringValues.Empty"/>.
    /// </para>
    /// </remarks>
    /// <value>A <see cref="StringValues"/> collection representing the property values.</value>
    StringValues Values { get; }

    /// <summary>
    /// Gets the optional condition that must evaluate to true for this property to be set.
    /// </summary>
    /// <remarks>
    /// This allows for conditional property assignment based on project properties, target frameworks,
    /// platforms, or other build conditions. When <see langword="null"/>, the property is always set.
    /// </remarks>
    /// <value>A condition string, or <see langword="null"/> if no condition is set.</value>
    string? Condition => null;

    /// <summary>
    /// Gets the optional label for organizing and identifying related properties.
    /// </summary>
    /// <remarks>
    /// Labels are optional tags that can be used to organize properties logically. They don't
    /// affect the build process but can be useful for documentation and organization of complex
    /// project files.
    /// </remarks>
    /// <value>A descriptive label string, or <see langword="null"/> if no label is set.</value>
    string? Label => null;

    /// <summary>
    /// Gets a value indicating whether the property is null or has no values.
    /// </summary>
    /// <remarks>
    /// A property is considered null or empty if its Values collection is empty or contains
    /// only whitespace, indicating that the property has not been assigned a meaningful value.
    /// </remarks>
    /// <value>
    /// <see langword="true"/> if the property has no values or only empty values;
    /// <see langword="false"/> if the property contains at least one non-empty value.
    /// </value>
    bool IsNullOrEmpty => StringValues.IsNullOrEmpty(Values);

    /// <summary>
    /// Gets the value of the property as a single semicolon-delimited string.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If the property contains multiple values, they are joined with semicolons to create
    /// a single string representation suitable for use in the project file.
    /// </para>
    /// <para>
    /// Duplicate values are removed when joining, ensuring each distinct value appears only once.
    /// </para>
    /// </remarks>
    /// <returns>
    /// A semicolon-delimited string of property values, or an empty string if the property has no values.
    /// </returns>
    string GetValue() => string.Join(';', Values.Distinct());
}
