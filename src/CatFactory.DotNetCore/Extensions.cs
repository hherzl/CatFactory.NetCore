using System;
using CatFactory.CodeFactory;
using CatFactory.Mapping;

namespace CatFactory.DotNetCore
{
    public static class Extensions
    {
        private static INamingConvention namingConvention;

        static Extensions()
        {
            namingConvention = new DotNetNamingConvention() as INamingConvention;
        }

        public static String GetParameterName(this Column column)
        {
            return namingConvention.GetParameterName(column.Name);
        }

        public static String GetPropertyName(this Column column)
        {
            return namingConvention.GetPropertyName(column.Name);
        }
    }
}
