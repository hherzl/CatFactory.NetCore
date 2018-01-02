using System.Collections.Generic;
using CatFactory.CodeFactory;
using CatFactory.Mapping;

namespace CatFactory.DotNetCore
{
    public static class ColumnExtensions
    {
        public static List<char> invalidChars;
        public static ICodeNamingConvention namingConvention;

        static ColumnExtensions()
        {
            invalidChars = new List<char>
            {
                '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '=', '+', ';', ':', '"', ',', '.', '/', '?'
            };

            namingConvention = new DotNetNamingConvention();
        }

        public static string GetParameterName(this Column column)
            => namingConvention.GetParameterName(column.Name);

        public static string GetPropertyName(this Column column)
        {
            var name = column.Name;

            foreach (var item in invalidChars)
            {
                name = name.Replace(item, '_');
            }

            return namingConvention.GetPropertyName(name);
        }
    }
}
