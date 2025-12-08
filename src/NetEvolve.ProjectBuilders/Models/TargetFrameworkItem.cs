namespace NetEvolve.ProjectBuilders.Models;

using System.Linq;
using Microsoft.Extensions.Primitives;
using NetEvolve.ProjectBuilders.Abstractions;

/// <summary>
/// Represents a target framework property group item for a project file.
/// </summary>
/// <remarks>
/// <para>
/// This internal record implements <see cref="IPropertyGroupItem{T}"/> for managing target framework
/// specifications in MSBuild project files. It provides an abstraction over the MSBuild properties
/// <c>TargetFramework</c> (for single framework) and <c>TargetFrameworks</c> (for multiple frameworks).
/// </para>
/// <para>
/// The property name automatically switches between "TargetFramework" and "TargetFrameworks" based on
/// the number of configured frameworks:
/// <list type="bullet">
/// <item><description>Single framework: Uses "TargetFramework" property</description></item>
/// <item><description>Multiple frameworks: Uses "TargetFrameworks" property (semicolon-delimited)</description></item>
/// </list>
/// </para>
/// <para>
/// Example in project file:
/// <code>
/// <!-- Single framework -->
/// &lt;TargetFramework&gt;net8.0&lt;/TargetFramework&gt;
///
/// <!-- Multiple frameworks -->
/// &lt;TargetFrameworks&gt;net6.0;net8.0;net9.0&lt;/TargetFrameworks&gt;
/// </code>
/// </para>
/// </remarks>
internal sealed record TargetFrameworkItem : IPropertyGroupItem<TargetFramework>
{
    /// <summary>
    /// Gets the MSBuild property name based on the number of frameworks.
    /// </summary>
    /// <remarks>
    /// Returns "TargetFramework" for a single framework or "TargetFrameworks" for multiple frameworks.
    /// This automatic selection ensures compatibility with MSBuild project file semantics.
    /// </remarks>
    /// <value>
    /// <c>TargetFramework</c> if one framework is configured; otherwise, <c>TargetFrameworks</c>.
    /// </value>
    public string Name => Values.Count > 1 ? "TargetFrameworks" : "TargetFramework";

    /// <summary>
    /// Gets or sets the target framework values as a semicolon-delimited string collection.
    /// </summary>
    /// <remarks>
    /// Framework values are stored as framework monikers (e.g., "net8.0", "net8.0-android").
    /// Multiple frameworks are joined with semicolons when serialized to the project file.
    /// </remarks>
    /// <value>The collection of framework moniker strings.</value>
    public StringValues Values { get; internal set; }

    /// <summary>
    /// Sets a single target framework.
    /// </summary>
    /// <remarks>
    /// This method replaces all existing frameworks with a single specified framework.
    /// The framework moniker is extracted from the <see cref="TargetFramework"/> value.
    /// </remarks>
    /// <param name="value">The framework to set as the target.</param>
    public void SetValue(TargetFramework value) => Values = value.Value;

    /// <summary>
    /// Adds multiple target frameworks to the current collection.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method appends additional frameworks to any existing frameworks rather than replacing them.
    /// The frameworks are stored as a semicolon-delimited collection for MSBuild compatibility.
    /// </para>
    /// <para>
    /// Multiple frameworks enable multi-targeting, allowing a single project to produce binaries for
    /// several frameworks simultaneously. The build system compiles code for each framework, allowing
    /// framework-specific conditional compilation via preprocessor symbols.
    /// </para>
    /// </remarks>
    /// <param name="values">An array of frameworks to add.</param>
    public void SetValues(TargetFramework[] values) =>
        Values = StringValues.Concat(Values, values.Select(x => x.Value).ToArray());
}
