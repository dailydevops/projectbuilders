namespace NetEvolve.ProjectBuilders.Abstractions;

using System;
using System.IO;

/// <summary>
/// Represents a builder for creating and configuring MSBuild project files.
/// </summary>
/// <remarks>
/// <para>
/// This interface provides a fluent API for constructing .NET project files (both .csproj and .vbproj).
/// It allows configuration of project SDK, target frameworks, properties, and item references in a
/// type-safe, programmatic manner.
/// </para>
/// <para>
/// The builder manages two main sections of a project file:
/// <list type="bullet">
/// <item><description><b>PropertyGroup</b>: Defines build properties like TargetFramework, Nullable, Configuration</description></item>
/// <item><description><b>ItemGroup</b>: Defines project items like PackageReference, ProjectReference, FrameworkReference</description></item>
/// </list>
/// </para>
/// <para>
/// Usage example:
/// <code>
/// var builder = factory.AddFileBuilder&lt;IProjectBuilder, ProjectBuilder&gt;(
///     new ProjectBuilder(directory, "test.csproj")
/// );
/// builder
///     .SetProjectSdk("Microsoft.NET.Sdk")
///     .WithTargetFramework(TargetFramework.Net8)
///     .WithNullable(NullableOptions.Enable);
/// </code>
/// </para>
/// </remarks>
/// <seealso cref="IFileBuilder"/>
public interface IProjectBuilder : IFileBuilder
{
    /// <summary>
    /// Creates a file in the project directory.
    /// </summary>
    /// <remarks>
    /// This method allows creation of additional files alongside the project file, such as
    /// source code files, configuration files, or other project resources.
    /// </remarks>
    /// <param name="fileName">The name of the file to create (e.g., "Program.cs", "appsettings.json").</param>
    /// <returns>
    /// A writeable <see cref="Stream"/> to the created file. The caller must dispose this stream.
    /// </returns>
    Stream CreateFile(string fileName);

    /// <summary>
    /// Gets or adds an item group item of the specified type.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If an item of the specified type already exists in the ItemGroup, it is returned.
    /// Otherwise, a new item of that type is created and added.
    /// </para>
    /// <para>
    /// This method is useful for ensuring only one instance of a particular item type exists,
    /// such as a single TargetFramework property.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">
    /// The type of the item to get or add. Must implement <see cref="IItemGroupItem"/> and
    /// have a public parameterless constructor.
    /// </typeparam>
    /// <returns>
    /// The existing or newly created item of type <typeparamref name="T"/>.
    /// </returns>
    T GetOrAddItemGroupItem<T>()
        where T : class, IItemGroupItem, new();

    /// <summary>
    /// Gets or adds a property group item of the specified type.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If a property of the specified type already exists in the PropertyGroup, it is returned.
    /// Otherwise, a new property of that type is created and added.
    /// </para>
    /// <para>
    /// This method is useful for ensuring only one instance of a particular property type exists,
    /// such as a single TargetFramework or Nullable property.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">
    /// The type of the property to get or add. Must implement <see cref="IPropertyGroupItem"/> and
    /// have a public parameterless constructor.
    /// </typeparam>
    /// <returns>
    /// The existing or newly created property of type <typeparamref name="T"/>.
    /// </returns>
    T GetOrAddPropertyGroupItem<T>()
        where T : class, IPropertyGroupItem, new();

    /// <summary>
    /// Sets the SDK used by the project file.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The SDK attribute on the Project element determines which build system and tools are used.
    /// Common values include:
    /// <list type="bullet">
    /// <item><description>Microsoft.NET.Sdk - Standard .NET Console, Class Library, ASP.NET Core applications</description></item>
    /// <item><description>Microsoft.NET.Sdk.Web - ASP.NET Core web applications</description></item>
    /// <item><description>Microsoft.NET.Sdk.WindowsDesktop - WPF and Windows Forms applications</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// If the SDK has already been set, calling this method overwrites the previous value.
    /// </para>
    /// </remarks>
    /// <param name="sdk">
    /// The SDK identifier (e.g., "Microsoft.NET.Sdk"). Must not be <see langword="null"/>, empty, or whitespace.
    /// </param>
    /// <returns>
    /// The current instance of <see cref="IProjectBuilder"/> for fluent chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="sdk"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="sdk"/> is empty or contains only whitespace.
    /// </exception>
    IProjectBuilder SetProjectSdk(string sdk);
}
