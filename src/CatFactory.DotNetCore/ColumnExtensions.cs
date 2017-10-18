using CatFactory.CodeFactory;
using CatFactory.Mapping;

namespace CatFactory.DotNetCore
{
    public static class ColumnExtensions
    {
        private static ICodeNamingConvention namingConvention;

        static ColumnExtensions()
        {
            namingConvention = new DotNetNamingConvention();
        }

        public static string GetParameterName(this Column column)
            => namingConvention.GetParameterName(column.Name);

        public static string GetPropertyName(this Column column)
            => namingConvention.GetPropertyName(column.Name);
    }
}
