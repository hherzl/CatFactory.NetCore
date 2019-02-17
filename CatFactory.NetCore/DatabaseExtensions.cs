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
                return type.GetParentType(database.DatabaseTypeMaps);

            return type.GetClrType() == null ? null : type;
        }

        public static DatabaseTypeMap GetClrMapForType(this Database database, Parameter parameter)
        {
            var type = database.DatabaseTypeMaps.FirstOrDefault(item => item.DatabaseType == parameter.Type);

            if (type == null)
                return null;

            if (!string.IsNullOrEmpty(type.ParentDatabaseType))
                return type.GetParentType(database.DatabaseTypeMaps);

            return type.GetClrType() == null ? null : type;
        }

        public static bool HasTypeMappedToClr(this Database database, Column column)
        {
            var type = database.DatabaseTypeMaps.FirstOrDefault(item => item.DatabaseType == column.Type);

            if (type == null)
                return false;

            if (!string.IsNullOrEmpty(type.ParentDatabaseType))
                return type.GetParentType(database.DatabaseTypeMaps) == null ? false : true;

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
                return type.GetParentType(database.DatabaseTypeMaps) == null ? false : true;

            if (type.GetClrType() != null)
                return true;

            return false;
        }

        public static string ResolveDatabaseType(this Database database, Column column)
        {
            var databaseTypeMap = database.DatabaseTypeMaps.FirstOrDefault(item => item.DatabaseType == column.Type);

            if (databaseTypeMap == null || databaseTypeMap.GetClrType() == null)
                return "object";

            return databaseTypeMap.AllowClrNullable ? string.Format("{0}?", databaseTypeMap.GetClrType().Name) : databaseTypeMap.GetClrType().Name;
        }

        public static DatabaseTypeMap ResolveDatabaseType(this Database database, string type)
            => database.DatabaseTypeMaps.FirstOrDefault(item => item.DatabaseType == type);

        public static string ResolveDbType(this Database database, Column column)
        {
            var databaseTypeMap = database.DatabaseTypeMaps.FirstOrDefault(item => item.DatabaseType == column.Type);

            if (databaseTypeMap == null || databaseTypeMap.GetClrType() == null)
                return "object";

            return string.Format("DbType.{0}", databaseTypeMap.DbTypeEnum);
        }
    }
}
