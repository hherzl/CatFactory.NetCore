using System;
using System.Linq;
using CatFactory.CodeFactory;

namespace CatFactory.DotNetCore
{
    public class DotNetNamingConvention : ICodeNamingConvention
    {
        public DotNetNamingConvention()
        {
        }

        // todo: add logic to validate name
        public String ValidName(String name)
            => String.Join("", name.Split(' ').Select(item => CodeNamingConvention.GetPascalCase(item)));

        public String GetInterfaceName(String value)
            => CodeNamingConvention.GetPascalCase(String.Format("I{0}", ValidName(value)));

        public String GetClassName(String value)
            => CodeNamingConvention.GetPascalCase(ValidName(value));

        public String GetFieldName(String value)
            => String.Format("m_{0}", CodeNamingConvention.GetCamelCase(ValidName(value)));

        public String GetConstantName(String value)
            => String.Format("{0}", CodeNamingConvention.GetCamelCase(ValidName(value)));

        public String GetPropertyName(String value)
            => CodeNamingConvention.GetPascalCase(ValidName(value));

        public String GetMethodName(String value)
            => CodeNamingConvention.GetPascalCase(ValidName(value));

        public String GetParameterName(String value)
            => CodeNamingConvention.GetCamelCase(ValidName(value));
    }
}
