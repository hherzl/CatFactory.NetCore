using System;
using System.Linq;
using CatFactory.CodeFactory;

namespace CatFactory.DotNetCore
{
    public class DotNetNamingConvention : INamingConvention
    {
        public DotNetNamingConvention()
        {
        }

        // todo: add logic to validate name
        public String ValidName(String name)
            => String.Join("", name.Split(' ').Select(item => NamingConvention.GetPascalCase(item)));

        public String GetInterfaceName(String value)
            => NamingConvention.GetPascalCase(String.Format("I{0}", ValidName(value)));

        public String GetClassName(String value)
            => NamingConvention.GetPascalCase(ValidName(value));

        public String GetFieldName(String value)
            => String.Format("m_{0}", NamingConvention.GetCamelCase(ValidName(value)));

        public String GetPropertyName(String value)
            => NamingConvention.GetPascalCase(ValidName(value));

        public String GetMethodName(String value)
            => NamingConvention.GetPascalCase(ValidName(value));

        public String GetParameterName(String value)
            => NamingConvention.GetCamelCase(ValidName(value));
    }
}
