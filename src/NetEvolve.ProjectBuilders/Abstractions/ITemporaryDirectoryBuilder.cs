namespace NetEvolve.ProjectBuilders.Abstractions;

using NetEvolve.ProjectBuilders.Builders;

/// <summary>
/// Represents a temporary directory builder for creating isolated, auto-cleaning temporary directories.
/// </summary>
/// <remarks>
/// <para>
/// This interface extends <see cref="ISubdirectoryBuilder"/> to provide specialized behavior
/// for managing temporary directories that are automatically created and cleaned up after use.
/// </para>
/// <para>
/// Temporary directories created through this interface are:
/// <list type="bullet">
/// <item><description>Created in the system's temporary folder with unique identifiers</description></item>
/// <item><description>Isolated from other test execution contexts</description></item>
/// <item><description>Automatically deleted when disposed, ensuring clean test environments</description></item>
/// <item><description>Protected against file system pollution and leftover test artifacts</description></item>
/// </list>
/// </para>
/// </remarks>
/// <seealso cref="ISubdirectoryBuilder"/>
/// <seealso cref="TemporaryDirectoryBuilder"/>
public interface ITemporaryDirectoryBuilder : ISubdirectoryBuilder;
