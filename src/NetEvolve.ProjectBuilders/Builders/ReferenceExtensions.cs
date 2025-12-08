namespace NetEvolve.ProjectBuilders.Builders;

using System.Xml.Linq;
using NetEvolve.ProjectBuilders.Abstractions;
using NetEvolve.ProjectBuilders.Models;

internal static class ReferenceExtensions
{
    public static XElement ToXElement(this IReference item)
    {
        var element = new XElement(item.Name);

        element.SetAttributeValue("Include", item.Include);
        element.SetAttributeValue("Condition", item.Condition);
        element.SetAttributeValue("Label", item.Label);
        element.SetAttributeValue("GeneratePathProperty", item.GeneratePathProperty ? true : null);

        element.SetElementValue("IncludeAssets", item.IncludeAssets.GetValue());
        element.SetElementValue("ExcludeAssets", item.ExcludeAssets.GetValue());
        element.SetElementValue("PrivateAssets", item.PrivateAssets.GetValue());

        return element;
    }
}
