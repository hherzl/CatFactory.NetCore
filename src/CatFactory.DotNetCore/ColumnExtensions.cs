using System;
using CatFactory.CodeFactory;
using CatFactory.Mapping;

namespace CatFactory.DotNetCore
{
    public static class ColumnExtensions
    {
        private static INamingConvention namingConvention;

        static ColumnExtensions()
        {
            namingConvention = new DotNetNamingConvention() as INamingConvention;
        }

        public static String GetParameterName(this Column column)
            => namingConvention.GetParameterName(column.Name);

        public static String GetPropertyName(this Column column)
            => namingConvention.GetPropertyName(column.Name);
    }
}
