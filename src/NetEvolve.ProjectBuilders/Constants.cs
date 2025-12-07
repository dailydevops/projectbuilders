namespace NetEvolve.ProjectBuilders;

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;

internal static class Constants
{
    public const string RuntimeSdkDefault = "8.0.204";
    public const string RuntimeSdkLTS = "10.0.100";

    public const string CommandBuild = "build";
    public const string CommandRestore = "restore";

    public const string CSharpProjectFileName = "test.csproj";
    public const string VBNetProjectFileName = "test.vbproj";

    public const string OutputFileName = "result.sarif";

#pragma warning disable IDE1006 // Naming Styles
    public static readonly XmlWriterSettings XmlSettings = new XmlWriterSettings
    {
        OmitXmlDeclaration = true,
        Indent = true,
        IndentChars = "  ",
    };

    public static readonly JsonSerializerOptions JsonSettings = new JsonSerializerOptions
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
    };

#pragma warning restore IDE1006 // Naming Styles
}
