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
        public string ValidName(string name)
            => string.Join("", name.Split(' ').Select(item => NamingConvention.GetPascalCase(item)));

        public string GetNamespace(string value)
            => NamingConvention.GetPascalCase(string.Format("{0}", ValidName(value)));

        public string GetInterfaceName(string value)
            => NamingConvention.GetPascalCase(string.Format("I{0}", ValidName(value)));

        public string GetClassName(string value)
            => NamingConvention.GetPascalCase(ValidName(value));

        public string GetFieldName(string value)
            => string.Format("m_{0}", NamingConvention.GetCamelCase(ValidName(value)));

        public string GetConstantName(string value)
            => string.Format("{0}", NamingConvention.GetCamelCase(ValidName(value)));

        public string GetPropertyName(string value)
            => NamingConvention.GetPascalCase(ValidName(value));

        public string GetMethodName(string value)
            => NamingConvention.GetPascalCase(ValidName(value));

        public string GetParameterName(string value)
            => NamingConvention.GetCamelCase(ValidName(value));
    }
}
