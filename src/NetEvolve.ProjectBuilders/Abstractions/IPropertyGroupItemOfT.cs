namespace NetEvolve.ProjectBuilders.Abstractions;

using NetEvolve.ProjectBuilders.Models;

/// <summary>
/// Represents a strongly-typed property element in a PropertyGroup that supports value mutation.
/// </summary>
/// <typeparam name="T">The type of values this property accepts.</typeparam>
/// <remarks>
/// <para>
/// This generic interface extends <see cref="IPropertyGroupItem"/> to provide methods for setting
/// property values in a type-safe manner. Implementations enforce validation and type conversions
/// appropriate for the specific property type.
/// </para>
/// <para>
/// For example, <see cref="TargetFrameworkItem"/> implements this interface with <see cref="TargetFramework"/>
/// to ensure only valid target framework values are set.
/// </para>
/// </remarks>
/// <seealso cref="IPropertyGroupItem"/>
public interface IPropertyGroupItem<in T> : IPropertyGroupItem
{
    /// <summary>
    /// Sets the value of the property to a single value.
    /// </summary>
    /// <remarks>
    /// <para>
    /// CAUTION: This method overwrites any existing values. If you need to append rather than
    /// replace, use <see cref="SetValues(T[])"/> instead or consider the property's append semantics.
    /// </para>
    /// <para>
    /// The value is converted to the appropriate string representation for storage in the project file.
    /// </para>
    /// </remarks>
    /// <param name="value">
    /// The value to set. The method should handle <see langword="null"/> appropriately based on
    /// the implementation and property type.
    /// </param>
    void SetValue(T value);

    /// <summary>
    /// Sets the values of the property to multiple values.
    /// </summary>
    /// <remarks>
    /// <para>
    /// CAUTION: This method overwrites any existing values. Multiple values are typically
    /// stored as semicolon-separated entries in the project file.
    /// </para>
    /// <para>
    /// Each value is converted to the appropriate string representation for storage.
    /// </para>
    /// </remarks>
    /// <param name="values">
    /// An array of values to set. Must not be <see langword="null"/>. Empty array results in
    /// clearing all values from the property.
    /// </param>
    void SetValues(T[] values);
}
