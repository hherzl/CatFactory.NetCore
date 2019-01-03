using System.Linq;
using CatFactory.ObjectRelationalMapping;
using CatFactory.ObjectRelationalMapping.Programmability;

namespace CatFactory.NetCore
{
    public static class DatabaseExtensions
    {
        public static DatabaseTypeMap GetClrMapForType(this Database database, Column column)
        {
            var type = database.DatabaseTypeMaps.FirstOrDefault(item => item.DatabaseType == column.Type);

            if (type == null)
                return null;

            if (!string.IsNullOrEmpty(type.ParentDatabaseType))
            {
                var parentType = type.GetParentType(database.DatabaseTypeMaps);

                if (parentType == null)
                    return null;
                else
                    return parentType;
            }

            if (type.GetClrType() != null)
                return type;

            return null;
        }

        public static DatabaseTypeMap GetClrMapForType(this Database database, Parameter parameter)
        {
            var type = database.DatabaseTypeMaps.FirstOrDefault(item => item.DatabaseType == parameter.Type);

            if (type == null)
                return null;

            if (!string.IsNullOrEmpty(type.ParentDatabaseType))
            {
                var parentType = type.GetParentType(database.DatabaseTypeMaps);

                if (parentType == null)
                    return null;
                else
                    return parentType;
            }

            if (type.GetClrType() != null)
                return type;

            return null;
        }

        public static bool HasTypeMappedToClr(this Database database, Column column)
        {
            var type = database.DatabaseTypeMaps.FirstOrDefault(item => item.DatabaseType == column.Type);

            if (type == null)
                return false;

            if (!string.IsNullOrEmpty(type.ParentDatabaseType))
            {
                var parentType = type.GetParentType(database.DatabaseTypeMaps);

                if (parentType == null)
                    return false;
                else
                    return true;
            }

            if (type.GetClrType() != null)
                return true;

            return false;
        }

        public static bool HasTypeMappedToClr(this Database database, Parameter parameter)
        {
            var type = database.DatabaseTypeMaps.FirstOrDefault(item => item.DatabaseType == parameter.Type);

            if (type == null)
                return false;

            if (!string.IsNullOrEmpty(type.ParentDatabaseType))
            {
                var parentType = type.GetParentType(database.DatabaseTypeMaps);

                if (parentType == null)
                    return false;
                else
                    return true;
            }

            if (type.GetClrType() != null)
                return true;

            return false;
        }

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
