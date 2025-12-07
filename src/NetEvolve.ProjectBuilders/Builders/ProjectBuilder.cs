namespace NetEvolve.ProjectBuilders.Builders;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using NetEvolve.ProjectBuilders.Abstractions;
using NetEvolve.ProjectBuilders.Models;

/// <inheritdoc cref="IProjectBuilder" />
internal sealed class ProjectBuilder : IProjectBuilder
{
    private readonly ISubdirectoryBuilder _directory;

    private readonly Dictionary<string, string> _projectAttributes;

    internal ItemGroup ItemGroup { get; }
    internal PropertyGroup PropertyGroup { get; }

    private readonly string _projectName;
    private const string NameProjectAttributeSdk = "Sdk";

    public string FullPath => Path.Combine(_directory.FullPath, _projectName);

    internal ProjectBuilder(ISubdirectoryBuilder directory, string projectExtension)
    {
        _directory = directory;

        _projectName = projectExtension;
        _projectAttributes = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            { NameProjectAttributeSdk, "Microsoft.NET.Sdk" },
        };

        PropertyGroup = new PropertyGroup();
        PropertyGroup.Add("ErrorLog", $"{Constants.OutputFileName},version=2.1");
        _ = this.WithTargetFramework(TargetFramework.Net8);
        ItemGroup = new ItemGroup();
    }

    /// <inheritdoc cref="IObjectBuilder.CreateAsync(CancellationToken)" />
    public ValueTask CreateAsync(CancellationToken cancellationToken = default)
    {
        var document = CreateDocument();

        using var file = _directory.CreateFile(_projectName);
        using var writer = XmlWriter.Create(file, Constants.XmlSettings);

        document.WriteTo(writer);

        return ValueTask.CompletedTask;
    }

    /// <inheritdoc cref="IProjectBuilder.CreateFile(string)" />
    public Stream CreateFile(string fileName) => _directory.CreateFile(fileName);

    /// <inheritdoc cref="IProjectBuilder.GetOrAddItemGroupItem{T}" />
    public T GetOrAddItemGroupItem<T>()
        where T : class, IItemGroupItem, new()
    {
        var item = ItemGroup.Items.OfType<T>().FirstOrDefault() ?? ItemGroup.Add<T>();
        return item;
    }

    /// <inheritdoc cref="IProjectBuilder.GetOrAddPropertyGroupItem{T}" />
    public T GetOrAddPropertyGroupItem<T>()
        where T : class, IPropertyGroupItem, new()
    {
        var item = PropertyGroup.Items.OfType<T>().FirstOrDefault() ?? PropertyGroup.Add<T>();
        return item;
    }

    /// <inheritdoc cref="IProjectBuilder.SetProjectSdk(string)" />
    public IProjectBuilder SetProjectSdk(string sdk)
    {
        var currentValue = _projectAttributes[NameProjectAttributeSdk];
        if (!currentValue.Equals(sdk, StringComparison.Ordinal))
        {
            _projectAttributes[NameProjectAttributeSdk] = sdk;
        }

        return this;
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    private XDocument CreateDocument()
    {
        var project = new XElement("Project");

        foreach (var attributePair in _projectAttributes)
        {
            if (string.IsNullOrWhiteSpace(attributePair.Value))
            {
                continue;
            }

            project.Add(new XAttribute(attributePair.Key, attributePair.Value));
        }

        AppendPropertyGroups(project);
        AppendItemGroups(project);

        return new XDocument(null, project);
    }

    private void AppendItemGroups(XElement project)
    {
        if (ItemGroup.Items.Count == 0)
        {
            return;
        }

        var result = new XElement("ItemGroup");

        foreach (var item in ItemGroup.Items)
        {
            var element = new XElement(item.Name);

            if (!string.IsNullOrWhiteSpace(item.Condition))
            {
                element.Add(new XAttribute("Condition", item.Condition));
            }

            if (!string.IsNullOrWhiteSpace(item.Label))
            {
                element.Add(new XAttribute("Label", item.Label));
            }

            result.Add(element);
        }

        project.Add(result);
    }

    private void AppendPropertyGroups(XElement project)
    {
        if (PropertyGroup.Items.Count == 0)
        {
            return;
        }

        var result = new XElement("PropertyGroup");

        foreach (var item in PropertyGroup.Items)
        {
            var value = item.GetValue();
            if (string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            var element = new XElement(item.Name, value);

            if (!string.IsNullOrWhiteSpace(item.Condition))
            {
                element.Add(new XAttribute("Condition", item.Condition));
            }

            if (!string.IsNullOrWhiteSpace(item.Label))
            {
                element.Add(new XAttribute("Label", item.Label));
            }

            result.Add(element);
        }

        project.Add(result);
    }
}
