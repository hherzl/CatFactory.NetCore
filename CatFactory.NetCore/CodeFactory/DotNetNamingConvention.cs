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
                ' ', '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '=', '+', ';', ':', '"', ',', '.', '/', '\\', '?', '|', '{', '}'
            };
        }

        public DotNetNamingConvention()
        {
        }

        public string ValidName(string value)
        {
/*
            foreach (var item in invalidChars)
                value = value?.Replace(item, '_');

*/
             var provider = new Microsoft.CSharp.CSharpCodeProvider();
            if (provider.IsValidIdentifier(value))
                return value;

            var invalidChars = string.Join("+|", DotNetNamingConvention.invalidChars.Select(c => System.Text.RegularExpressions.Regex.Escape(System.Convert.ToString(c))));

            var pattern = $"{invalidChars}+|\\s+/g";

            var validName = string.Join("",
                System.Text.RegularExpressions.Regex.Replace(value, pattern, "_", System.Text.RegularExpressions.RegexOptions.IgnoreCase, System.TimeSpan.FromSeconds(0.5))
                    .Split(' ').Select(item => item));

            validName = System.Text.RegularExpressions.Regex.Replace(validName, @"__", "_", System.Text.RegularExpressions.RegexOptions.IgnoreCase, System.TimeSpan.FromSeconds(0.5));

            if (provider.IsValidIdentifier(validName))
                return validName;

            // remove all whitespace, digit
            validName = System.Text.RegularExpressions.Regex.Replace(validName, @"\s+\d+\p{P}+/g", "_", System.Text.RegularExpressions.RegexOptions.IgnoreCase,
                System.TimeSpan.FromSeconds(0.5));
            validName = System.Text.RegularExpressions.Regex.Replace(validName, @"__", "_", System.Text.RegularExpressions.RegexOptions.IgnoreCase, System.TimeSpan.FromSeconds(0.5));
            if (provider.IsValidIdentifier(validName))
                return validName;

            // remove all non-alphanumeric
            validName = System.Text.RegularExpressions.Regex.Replace(validName, @"\W+/g", "_", System.Text.RegularExpressions.RegexOptions.IgnoreCase, System.TimeSpan.FromSeconds(0.5));
            validName = System.Text.RegularExpressions.Regex.Replace(validName, @"__", "_", System.Text.RegularExpressions.RegexOptions.IgnoreCase, System.TimeSpan.FromSeconds(0.5));
            if (provider.IsValidIdentifier(validName))
                return validName;

            // remove all non-alphabetic
            validName = System.Text.RegularExpressions.Regex.Replace(validName, @"[^a-zA-Z_]+/g", "_", System.Text.RegularExpressions.RegexOptions.IgnoreCase,
                System.TimeSpan.FromSeconds(0.5));
            validName = System.Text.RegularExpressions.Regex.Replace(validName, @"__", "_", System.Text.RegularExpressions.RegexOptions.IgnoreCase, System.TimeSpan.FromSeconds(0.5));
            return provider.IsValidIdentifier(validName) ? validName : $"@{validName}";
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
