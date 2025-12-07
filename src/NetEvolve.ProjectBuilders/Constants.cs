namespace NetEvolve.ProjectBuilders;

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;

/// <summary>
/// Contains internal constants used throughout the NetEvolve.ProjectBuilders library.
/// </summary>
/// <remarks>
/// This static class defines configuration constants for SDK versions, command names,
/// project file names, and default serialization settings for XML and JSON output.
/// </remarks>
internal static class Constants
{
    /// <summary>
    /// Default runtime SDK version (8.0.204).
    /// </summary>
    public const string RuntimeSdkDefault = "8.0.204";

    /// <summary>
    /// Long-term support runtime SDK version (10.0.100).
    /// </summary>
    public const string RuntimeSdkLTS = "10.0.100";

    /// <summary>
    /// The dotnet build command name.
    /// </summary>
    public const string CommandBuild = "build";

    /// <summary>
    /// The dotnet restore command name.
    /// </summary>
    public const string CommandRestore = "restore";

    /// <summary>
    /// The default C# project file name for testing.
    /// </summary>
    public const string CSharpProjectFileName = "test.csproj";

    /// <summary>
    /// The default VB.NET project file name for testing.
    /// </summary>
    public const string VBNetProjectFileName = "test.vbproj";

    /// <summary>
    /// The default SARIF output file name for diagnostic results.
    /// </summary>
    public const string OutputFileName = "result.sarif";

#pragma warning disable IDE1006 // Naming Styles
    /// <summary>
    /// Default XML writer settings for project file generation.
    /// Omits the XML declaration, enables indentation with 2-space characters.
    /// </summary>
    public static readonly XmlWriterSettings XmlSettings = new XmlWriterSettings
    {
        OmitXmlDeclaration = true,
        Indent = true,
        IndentChars = "  ",
    };

    /// <summary>
    /// Default JSON serialization options for SARIF output and global.json files.
    /// Enables pretty-printing and omits properties with default values.
    /// </summary>
    public static readonly JsonSerializerOptions JsonSettings = new JsonSerializerOptions
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
    };

#pragma warning restore IDE1006 // Naming Styles
}
