using System.Text.RegularExpressions;
using CatFactory.CodeFactory;
using CatFactory.NetCore.CodeFactory;
using CatFactory.ObjectRelationalMapping;
using CatFactory.ObjectRelationalMapping.Programmability;

namespace CatFactory.NetCore
{
    public static class DbObjectExtensions
    {
        public static ICodeNamingConvention NamingConvention;
        public static string PropertyNamePattern;

        static DbObjectExtensions()
        {
            NamingConvention = new DotNetNamingConvention();
            PropertyNamePattern = @"^[0-9]+$";
        }

        private static string GetName(string name)
        {
            foreach (var item in DotNetNamingConvention.invalidChars)
                name = name.Replace(item, '_');

            return NamingConvention.GetPropertyName(name);
        }

        public static string GetPropertyName(this Column column)
            => GetName(column.Name);

        public static string GetPropertyName(this Parameter parameter)
            => GetName(parameter.Name);

        public static string GetNameForEnclosing(this Column column)
            => string.Format("{0}1", column.Name);

        public static string GetPropertyNameHack(this ITable table, Column column)
        {
            var propertyName = table.Name == column.Name ? column.GetNameForEnclosing() : column.GetPropertyName();

            return new Regex(PropertyNamePattern).IsMatch(propertyName) ? string.Format("V{0}", propertyName) : propertyName;
        }

        public static string GetPropertyNameHack(this IView view, Column column)
        {
            var propertyName = view.Name == column.Name ? column.GetNameForEnclosing() : column.GetPropertyName();

            return new Regex(PropertyNamePattern).IsMatch(propertyName) ? string.Format("V{0}", propertyName) : propertyName;
        }
    }
}
