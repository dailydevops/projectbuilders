namespace NetEvolve.ProjectBuilders.Abstractions;

/// <summary>
/// Represents a test package builder that creates NuGet packages from referenced project sources.
/// </summary>
/// <remarks>
/// <para>
/// This interface enables the automatic creation of NuGet packages from project references,
/// which are then made available during the test project build process. This capability is
/// essential for validating how projects behave when consuming their own packages or dependencies.
/// </para>
/// <para>
/// The package builder:
/// <list type="bullet">
/// <item><description>Accepts paths to project files or .nuspec specifications</description></item>
/// <item><description>Packages them as NuGet packages with a test version (999.999.999)</description></item>
/// <item><description>Makes them available to the build process via a configured NuGet source</description></item>
/// <item><description>Handles platform-specific tooling (nuget.exe on Windows, nuget CLI on other platforms)</description></item>
/// </list>
/// </para>
/// </remarks>
/// <seealso cref="IObjectBuilder"/>
public interface ITestPackageBuilder : IObjectBuilder
{
    /// <summary>
    /// Sets the file system paths to projects or specifications used to create test packages.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method can be called multiple times to accumulate package paths. The method is
    /// thread-safe through internal locking to ensure thread-safe operations during the
    /// creation phase.
    /// </para>
    /// <para>
    /// Duplicate paths are automatically handled and will not be processed twice. Empty or
    /// whitespace paths are silently ignored.
    /// </para>
    /// </remarks>
    /// <param name="packagePaths">
    /// An array of absolute paths to .nuspec files or project files that should be packaged.
    /// Must not be <see langword="null"/>. Array elements that are empty or whitespace are ignored.
    /// </param>
    void SetPackagePaths(string[] packagePaths);
}
