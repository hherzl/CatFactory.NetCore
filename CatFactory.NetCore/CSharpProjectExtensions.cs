using System.Text.RegularExpressions;
using CatFactory.CodeFactory.Scaffolding;
using CatFactory.NetCore.CodeFactory;
using CatFactory.ObjectRelationalMapping;
using CatFactory.ObjectRelationalMapping.Programmability;

namespace CatFactory.NetCore;

public static class CSharpProjectExtensions
{
    private static readonly string PropertyNamePattern;

    static CSharpProjectExtensions()
    {
        PropertyNamePattern = @"^[0-9]+$";
    }

    public static string GetPropName<TProjectSettings>(this CSharpProject<TProjectSettings> project, string name) where TProjectSettings : class, IProjectSettings, new()
    {
        foreach (var item in DotNetNamingConvention.InvalidChars)
            name = name.Replace(item, '_');

        var propName = project.CodeNamingConvention.GetPropertyName(name);
        return new Regex(PropertyNamePattern).IsMatch(propName) ? string.Format("V{0}", propName) : propName;
    }

    public static string GetPropName<TProjectSettings>(this CSharpProject<TProjectSettings> project, ITable table, IColumn column) where TProjectSettings : class, IProjectSettings, new()
        => table.Name == column.Name ? string.Format("{0}1", project.GetPropName(column.Name)) : project.GetPropName(column.Name);

    public static string GetPropName<TProjectSettings>(this CSharpProject<TProjectSettings> project, IView view, IColumn column) where TProjectSettings : class, IProjectSettings, new()
        => view.Name == column.Name ? string.Format("{0}1", project.GetPropName(column.Name)) : project.GetPropName(column.Name);

    public static string GetPropName<TProjectSettings>(this CSharpProject<TProjectSettings> project, Parameter parameter) where TProjectSettings : class, IProjectSettings, new()
        => project.GetPropName(parameter.Name);
}
