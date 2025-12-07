namespace NetEvolve.ProjectBuilders.Abstractions;

/// <inheritdoc cref="IPropertyGroupItem" />
public interface IPropertyGroupItem<in T> : IPropertyGroupItem
{
    /// <summary>
    /// Sets the value of the property group item. CAUTION: This will overwrite any existing value.
    /// </summary>
    /// <param name="value">
    /// The value to set.
    /// </param>
    void SetValue(T value);

    /// <summary>
    /// Sets the values of the property group item. CAUTION: This will overwrite any existing values.
    /// </summary>
    /// <param name="values">
    /// The values to set.
    /// </param>
    void SetValues(T[] values);
}
