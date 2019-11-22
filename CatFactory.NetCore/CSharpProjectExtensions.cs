using System.Text.RegularExpressions;
using CatFactory.CodeFactory.Scaffolding;
using CatFactory.NetCore.CodeFactory;
using CatFactory.ObjectRelationalMapping;
using CatFactory.ObjectRelationalMapping.Programmability;

namespace CatFactory.NetCore
{
    public static class CSharpProjectExtensions
    {
        public static string PropertyNamePattern;

        static CSharpProjectExtensions()
        {
            PropertyNamePattern = @"^[0-9]+$";
        }

        public static string GetPropertyName<TProjectSettings>(this CSharpProject<TProjectSettings> project, string name) where TProjectSettings : class, IProjectSettings, new()
        {
            foreach (var item in DotNetNamingConvention.invalidChars)
                name = name.Replace(item, '_');

            var propertyName = project.CodeNamingConvention.GetPropertyName(name);

            return new Regex(PropertyNamePattern).IsMatch(propertyName) ? string.Format("V{0}", propertyName) : propertyName;
        }

        public static string GetPropertyName<TProjectSettings>(this CSharpProject<TProjectSettings> project, ITable table, IColumn column) where TProjectSettings : class, IProjectSettings, new()
            => table.Name == column.Name ? string.Format("{0}1", project.GetPropertyName(column.Name)) : project.GetPropertyName(column.Name);

        public static string GetPropertyName<TProjectSettings>(this CSharpProject<TProjectSettings> project, IView view, IColumn column) where TProjectSettings : class, IProjectSettings, new()
            => view.Name == column.Name ? string.Format("{0}1", project.GetPropertyName(column.Name)) : project.GetPropertyName(column.Name);

        public static string GetPropertyName<TProjectSettings>(this CSharpProject<TProjectSettings> project, Parameter parameter) where TProjectSettings : class, IProjectSettings, new()
            => project.GetPropertyName(parameter.Name);
    }
}

//static CSharpProjectExtensions()
//{
//}

//public static string
//    GetPropertyName<TProjectSettings>(this CSharpProject<TProjectSettings> project, string name)
//    where TProjectSettings : class, IProjectSettings, new() => project.CodeNamingConvention.GetPropertyName
//    (name);

//public static string GetPropertyName<TProjectSettings>(this CSharpProject<TProjectSettings> project,
//    ITable table, Column column) where TProjectSettings : class, IProjectSettings, new()
//    => table.Name == column.Name
//        ? $"{project.GetPropertyName(column.Name)}1"
//        : project.GetPropertyName(column.Name);

//public static string GetPropertyName<TProjectSettings>(this CSharpProject<TProjectSettings> project, IView view,
//    Column column) where TProjectSettings : class, IProjectSettings, new()
//    => view.Name == column.Name
//        ? $"{project.GetPropertyName(column.Name)}1"
//        : project.GetPropertyName(column.Name);

//public static string GetPropertyName<TProjectSettings>(this CSharpProject<TProjectSettings> project,
//    Parameter parameter) where TProjectSettings : class, IProjectSettings, new()
//    => project.GetPropertyName(parameter.Name);
