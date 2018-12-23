using System.Linq;
using System.Text.RegularExpressions;
using CatFactory.CodeFactory;
using CatFactory.NetCore.CodeFactory;
using CatFactory.ObjectRelationalMapping;
using CatFactory.ObjectRelationalMapping.Programmability;

namespace CatFactory.NetCore
{
    public static class DatabaseExtensions
    {
        public static ICodeNamingConvention NamingConvention;
        public static string PropertyNamePattern;

        static DatabaseExtensions()
        {
            NamingConvention = new DotNetNamingConvention();
            PropertyNamePattern = @"^[0-9]+$";
        }

        public static string GetPropertyName(this Column column)
        {
            var name = column.Name;

            foreach (var item in DotNetNamingConvention.invalidChars)
                name = name.Replace(item, '_');

            return NamingConvention.GetPropertyName(name);
        }

        public static string GetPropertyName(this Parameter parameter)
        {
            var name = parameter.Name;

            foreach (var item in DotNetNamingConvention.invalidChars)
                name = name.Replace(item, '_');

            return NamingConvention.GetPropertyName(name);
        }

        public static string GetPropertyNameHack(this ITable table, Column column)
        {
            var propertyName = column.HasSameNameEnclosingType(table) ? column.GetNameForEnclosing() : column.GetPropertyName();

            var regex = new Regex(PropertyNamePattern);

            if (regex.IsMatch(propertyName))
                propertyName = string.Format("V{0}", propertyName);

            return propertyName;
        }

        public static string GetPropertyNameHack(this IView view, Column column)
        {
            var propertyName = column.HasSameNameEnclosingType(view) ? column.GetNameForEnclosing() : column.GetPropertyName();

            var regex = new Regex(PropertyNamePattern);

            if (regex.IsMatch(propertyName))
                propertyName = string.Format("V{0}", propertyName);

            return propertyName;
        }

        public static bool HasSameNameEnclosingType(this Column column, ITable table)
            => column.Name == table.Name;

        public static bool HasSameNameEnclosingType(this Column column, IView view)
            => column.Name == view.Name;

        public static string GetNameForEnclosing(this Column column)
            => string.Format("{0}1", column.Name);

        public static string ResolveDatabaseType(this Database database, Column column)
        {
            var map = database.DatabaseTypeMaps.FirstOrDefault(item => item.DatabaseType == column.Type);

            if (map == null || map.GetClrType() == null)
                return "object";

            return map.AllowClrNullable ? string.Format("{0}?", map.GetClrType().Name) : map.GetClrType().Name;
        }

        public static DatabaseTypeMap ResolveType(this Database database, string type)
            => database.DatabaseTypeMaps.FirstOrDefault(item => item.DatabaseType == type);

        public static string ResolveDbType(this Database database, Column column)
        {
            var map = database.DatabaseTypeMaps.FirstOrDefault(item => item.DatabaseType == column.Type);

            if (map == null || map.GetClrType() == null)
                return "object";

            return string.Format("DbType.{0}", map.DbTypeEnum);
        }
    }
}
