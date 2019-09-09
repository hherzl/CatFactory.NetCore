using System.Linq;
using CatFactory.ObjectRelationalMapping;
using CatFactory.ObjectRelationalMapping.Programmability;

namespace CatFactory.NetCore
{
    public static class DatabaseExtensions
    {
        public static DatabaseTypeMap GetClrMapForType(this Database database, string type)
        {
            if (type.Contains("("))
                type = type.Substring(0, type.IndexOf("("));

            var dbTypeMap = database.DatabaseTypeMaps.FirstOrDefault(item => item.DatabaseType == type);

            if (dbTypeMap == null)
                return null;

            if (!string.IsNullOrEmpty(dbTypeMap.ParentDatabaseType))
                return dbTypeMap.GetParentType(database.DatabaseTypeMaps);

            return dbTypeMap.GetClrType() == null ? null : dbTypeMap;
        }

        public static DatabaseTypeMap GetClrMapForType(this Database database, Column column)
            => database.GetClrMapForType(column.Type);

        public static DatabaseTypeMap GetClrMapForType(this Database database, Parameter parameter)
            => database.GetClrMapForType(parameter.Type);

        public static bool HasTypeMappedToClr(this Database database, string type)
        {
            var dbTypeMap = database.DatabaseTypeMaps.FirstOrDefault(item => item.DatabaseType == type);

            if (dbTypeMap == null)
                return false;

            if (!string.IsNullOrEmpty(dbTypeMap.ParentDatabaseType))
                return dbTypeMap.GetParentType(database.DatabaseTypeMaps) == null ? false : true;

            if (dbTypeMap.GetClrType() != null)
                return true;

            return false;
        }

        public static bool HasTypeMappedToClr(this Database database, Column column)
            => database.HasTypeMappedToClr(column.Type);

        public static bool HasTypeMappedToClr(this Database database, Parameter parameter)
            => database.HasTypeMappedToClr(parameter.Type);

        public static string ResolveDatabaseType(this Database database, string type, bool useNullableIfDefinitionAllows = true)
        {
            if (type.Contains("("))
                type = type.Substring(0, type.IndexOf("("));

            var dbTypeMap = database.DatabaseTypeMaps.FirstOrDefault(item => item.DatabaseType == type);

            if (dbTypeMap == null || dbTypeMap.GetClrType() == null)
            {
                if (type == "Name")
                {
                    if (!string.IsNullOrEmpty(dbTypeMap.ParentDatabaseType))
                    {
                        var parentType = database.ResolveDatabaseType(dbTypeMap.ParentDatabaseType);

                        if (parentType != null)
                            return parentType;
                    }
                }

                return "object";
            }

            if (useNullableIfDefinitionAllows)
                return dbTypeMap.AllowClrNullable ? string.Format("{0}?", dbTypeMap.GetClrType().Name) : dbTypeMap.GetClrType().Name;
            else
                return dbTypeMap.GetClrType().Name;
        }

        public static string ResolveDatabaseType(this Database database, Column column, bool useNullableIfDefinitionAllows = true)
            => database.ResolveDatabaseType(column.Type, useNullableIfDefinitionAllows);

        public static string ResolveDatabaseType(this Database database, Parameter parameter, bool useNullableIfDefinitionAllows = true)
            => database.ResolveDatabaseType(parameter.Type, useNullableIfDefinitionAllows);

        public static string ResolveDbType(this Database database, string type)
        {
            if (type.Contains("("))
                type = type.Substring(0, type.IndexOf("("));

            var dbTypeMap = database.DatabaseTypeMaps.FirstOrDefault(item => item.DatabaseType == type);

            if (dbTypeMap == null || dbTypeMap.GetClrType() == null)
                return "object";

            return string.Format("DbType.{0}", dbTypeMap.DbTypeEnum);
        }

        public static string ResolveDbType(this Database database, Column column)
            => database.ResolveDbType(column.Type);

        public static string ResolveDbType(this Database database, Parameter parameter)
            => database.ResolveDbType(parameter.Type);
    }
}
