namespace NetEvolve.ProjectBuilders.Abstractions;

/// <summary>
/// Represents a test package builder, which allows the builder to create test packages based on referenced projects.
/// </summary>
public interface ITestPackageBuilder : IObjectBuilder
{
    void SetPackagePaths(string[] packagePaths);
}
