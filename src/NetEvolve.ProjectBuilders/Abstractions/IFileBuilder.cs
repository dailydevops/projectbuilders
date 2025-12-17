namespace NetEvolve.ProjectBuilders.Abstractions;

/// <summary>
/// Represents a generic file builder interface for creating project files.
/// </summary>
/// <remarks>
/// <para>
/// This interface serves as a base abstraction for all file builders in the project.
/// It extends <see cref="IObjectBuilder"/> to provide async creation and disposal capabilities
/// for file-based objects used in the project building process.
/// </para>
/// <para>
/// Implementations of this interface are responsible for creating specific types of files,
/// such as project files (.csproj, .vbproj) and configuration files (global.json).
/// </para>
/// </remarks>
/// <seealso cref="IObjectBuilder"/>
/// <seealso cref="IProjectBuilder"/>
/// <seealso cref="IGlobalJsonBuilder"/>
public interface IFileBuilder : IObjectBuilder;
