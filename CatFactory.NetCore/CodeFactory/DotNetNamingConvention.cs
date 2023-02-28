using System.Collections.Generic;
using CatFactory.CodeFactory;

namespace CatFactory.NetCore.CodeFactory
{
    public class DotNetNamingConvention : ICodeNamingConvention
    {
        public static readonly List<char> InvalidChars;

        static DotNetNamingConvention() =>
            InvalidChars = new List<char>
            {
                ' ', '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '=', '+', ';', ':', '"', ',', '.', '/', '\\', '?', '|', '{', '}'
            };

        public DotNetNamingConvention()
        {
        }

        public string ValidName(string value)
        {
            foreach (var item in InvalidChars)
                value = value.Replace(item, '_');

            return value;
        }

        public string GetNamespace(params string[] values)
            => string.Join(".", values);

        public string GetInterfaceName(string value)
            => $"I{NamingConvention.GetPascalCase(ValidName(value))}";

        public string GetClassName(string value)
            => NamingConvention.GetPascalCase(ValidName(value));

        public string GetConstantName(string value)
            => NamingConvention.GetPascalCase(ValidName(value));

        public string GetFieldName(string value)
            => $"_{NamingConvention.GetCamelCase(ValidName(value))}";

        public string GetPropertyName(string value)
            => NamingConvention.GetPascalCase(ValidName(value));

        public string GetMethodName(string value)
            => NamingConvention.GetPascalCase(ValidName(value));

        public string GetParameterName(string value)
            => NamingConvention.GetCamelCase(ValidName(value));
    }
}
