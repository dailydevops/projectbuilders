namespace NetEvolve.ProjectBuilders.Abstractions;

/// <summary>
/// Represents a test package builder, which allows the builder to create test packages based on referenced projects.
/// </summary>
public interface ITestPackageBuilder : IObjectBuilder
{
    /// <summary>
    /// Sets the locations of the packages used to build the test package.
    /// </summary>
    /// <param name="packagePaths">The file system paths to the packages.</param>
    void SetPackagePaths(string[] packagePaths);
}
