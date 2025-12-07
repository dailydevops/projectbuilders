namespace NetEvolve.ProjectBuilders.Builders;

using System;
using System.IO;
using System.Text;
using NetEvolve.Arguments;
using NetEvolve.ProjectBuilders.Abstractions;
using NetEvolve.ProjectBuilders.Models;

/// <summary>
/// Generic extensions for <see cref="IProjectBuilder"/> and all derived types.
/// </summary>
public static class ProjectBuilderExtensions
{
    /// <summary>
    /// Adds a C# file to the project.
    /// </summary>
    /// <typeparam name="T">The type of the project builder.</typeparam>
    /// <param name="builder">The project builder to set the target framework for.</param>
    /// <param name="fileName"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static void AddCSharpFile<T>(this T builder, string fileName, string content)
        where T : class, IProjectBuilder
    {
        Argument.ThrowIfNull(builder);
        Argument.ThrowIfNullOrWhiteSpace(fileName);
        Argument.ThrowIfNullOrWhiteSpace(content);

        if (builder is ProjectBuilder concreteBuilder)
        {
            using var file = concreteBuilder.CreateFile($"{Path.GetFileNameWithoutExtension(fileName)}.cs");
            file.Write(Encoding.UTF8.GetBytes(content).AsSpan());
        }
    }

    /// <summary>
    /// Adds a VB file to the project.
    /// </summary>
    /// <typeparam name="T">The type of the project builder.</typeparam>
    /// <param name="builder">The project builder to set the target framework for.</param>
    /// <param name="fileName"></param>
    /// <param name="content"></param>
    public static void AddVBFile<T>(this T builder, string fileName, string content)
        where T : class, IProjectBuilder
    {
        Argument.ThrowIfNull(builder);
        Argument.ThrowIfNullOrWhiteSpace(fileName);
        Argument.ThrowIfNullOrWhiteSpace(content);

        if (builder is ProjectBuilder concreteBuilder)
        {
            using var file = concreteBuilder.CreateFile($"{Path.GetFileNameWithoutExtension(fileName)}.vb");
            file.Write(Encoding.UTF8.GetBytes(content).AsSpan());
        }
    }

    public static T WithDefaults<T>(this T builder)
        where T : class, IProjectBuilder
    {
        Argument.ThrowIfNull(builder);

        return builder.WithTargetFramework(TargetFramework.Net8).WithNullable(NullableOptions.Enable);
    }

    /// <summary>
    /// Sets the <b>&lt;Nullable&gt;</b> property in the project file. Caution: this method will override any existing value.
    /// </summary>
    /// <typeparam name="T">The type of the project builder.</typeparam>
    /// <param name="builder">The project builder to set the target framework for.</param>
    /// <param name="nullable"></param>
    /// <returns></returns>
    public static T WithNullable<T>(this T builder, NullableOptions nullable)
        where T : class, IProjectBuilder
    {
        if (builder is ProjectBuilder concreteBuilder)
        {
            var propertyItem = concreteBuilder.GetOrAddPropertyGroupItem<NullableItem>();
            propertyItem.SetValue(nullable);
        }

        return builder;
    }

    /// <summary>
    /// Sets the <b>&lt;TargetFramework&gt;</b> property in the project file. Caution: this method will override any existing value.
    /// </summary>
    /// <typeparam name="T">The type of the project builder.</typeparam>
    /// <param name="builder">The project builder to set the target framework for.</param>
    /// <param name="targetFramework">The target framework to set. For example, <see cref="TargetFramework.Net5"/>.</param>
    /// <returns>The project builder with the target framework set.</returns>
    public static T WithTargetFramework<T>(this T builder, TargetFramework targetFramework)
        where T : class, IProjectBuilder
    {
        if (builder is ProjectBuilder concreteBuilder)
        {
            var propertyItem = concreteBuilder.GetOrAddPropertyGroupItem<TargetFrameworkItem>();

            propertyItem.SetValue(targetFramework);
        }

        return builder;
    }

    /// <summary>
    /// Sets the <b>&lt;TargetFrameworks&gt;</b> property in the project file. Caution: this method will override any existing value.
    /// </summary>
    /// <typeparam name="T">The type of the project builder.</typeparam>
    /// <param name="builder">The project builder to set the target frameworks for.</param>
    /// <param name="targetFrameworks">The target frameworks to set. For example, <see cref="TargetFramework.Net5"/> and <see cref="TargetFramework.Net8"/>.</param>
    /// <returns>The project builder with the target frameworks set.</returns>
    public static T WithTargetFrameworks<T>(this T builder, params TargetFramework[] targetFrameworks)
        where T : class, IProjectBuilder
    {
        if (builder is ProjectBuilder concreteBuilder)
        {
            var propertyItem = concreteBuilder.GetOrAddPropertyGroupItem<TargetFrameworkItem>();

            propertyItem.SetValues(targetFrameworks);
        }

        return builder;
    }
}
