using System;
using CatFactory.CodeFactory;
using CatFactory.Mapping;

namespace CatFactory.DotNetCore
{
    public static partial class MyClass
    {

    }

    public static class ColumnExtensions
    {
        private static ICodeNamingConvention namingConvention;

        static ColumnExtensions()
        {
            namingConvention = new DotNetNamingConvention() as ICodeNamingConvention;
        }

        public static String GetParameterName(this Column column)
            => namingConvention.GetParameterName(column.Name);

        public static String GetPropertyName(this Column column)
            => namingConvention.GetPropertyName(column.Name);
    }
}
