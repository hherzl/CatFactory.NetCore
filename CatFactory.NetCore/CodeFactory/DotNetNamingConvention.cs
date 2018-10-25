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
                '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '=', '+', ';', ':', '"', ',', '.', '/', '?'
            };
        }

        public DotNetNamingConvention()
        {
        }

        // todo: add logic to validate name
        public string ValidName(string value)
        {
            foreach (var item in invalidChars)
            {
                value = value.Replace(item, '_');
            }

            return string.Join("", value.Split(' ').Select(item => NamingConvention.GetPascalCase(item)));
        }

        public string GetNamespace(params string[] values)
            => string.Join(".", values);

        public string GetInterfaceName(string value)
            => NamingConvention.GetPascalCase(string.Format("I{0}", ValidName(value)));

        public string GetClassName(string value)
            => NamingConvention.GetPascalCase(ValidName(value));

        public string GetFieldName(string value)
            => string.Format("m_{0}", NamingConvention.GetCamelCase(ValidName(value)));

        public string GetConstantName(string value)
            => string.Format("{0}", NamingConvention.GetCamelCase(ValidName(value)));

        public string GetPropertyName(string value)
            => string.Format("{0}", NamingConvention.GetPascalCase(ValidName(value)));

        public string GetMethodName(string value)
            => NamingConvention.GetPascalCase(ValidName(value));

        public string GetParameterName(string value)
            => NamingConvention.GetCamelCase(ValidName(value));
    }
}
