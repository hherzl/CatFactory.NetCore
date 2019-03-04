using System.Collections.Generic;
using System.Linq;
using CatFactory.CodeFactory;

namespace CatFactory.NetCore.CodeFactory
{
    public class DotNetNamingConvention : ICodeNamingConvention
    {
        public static List<char> invalidChars;

        static DotNetNamingConvention()
        {
            invalidChars = new List<char>
            {
                ' ', '~', '!', '#', '$', '%', '^', '&', '*', '(', ')', '-', '=', '+', ';', ':', '"', ',', '.', '/', '\\', '?'
            };
        }

        public DotNetNamingConvention()
        {
        }

        public string ValidName(string value)
        {
            foreach (var item in invalidChars)
                value = value?.Replace(item, '_');

            return string.Join("", value?.Split(' ')?.Select(item => NamingConvention.GetPascalCase(item)));
        }

        public string GetNamespace(params string[] values)
            => string.Join(".", values);

        public string GetInterfaceName(string value)
            => ValidName(string.Format("I{0}", NamingConvention.GetPascalCase(value)));

        public string GetClassName(string value)
            => ValidName(NamingConvention.GetPascalCase(value));

        public string GetFieldName(string value)
            => ValidName(string.Format("m_{0}", NamingConvention.GetCamelCase(value)));

        public string GetConstantName(string value)
            => ValidName(NamingConvention.GetCamelCase(value));

        public string GetPropertyName(string value)
            => ValidName(NamingConvention.GetPascalCase(value));

        public string GetMethodName(string value)
            => ValidName(NamingConvention.GetPascalCase(value));

        public string GetParameterName(string value)
            => ValidName(NamingConvention.GetCamelCase(value));
    }
}
