using System.Collections.Generic;
using System.Linq;
using CatFactory.CodeFactory;

namespace CatFactory.NetCore.CodeFactory
{
    // ReSharper disable once ClassCanBeSealed.Global
    public class DotNetNamingConvention : ICodeNamingConvention
    {
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public static List<char> invalidChars;

        static DotNetNamingConvention() =>
            DotNetNamingConvention.invalidChars = new List<char>
            {
                ' ', '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '=', '+', ';', ':', '"', ',', '.', '/', '\\', '?', '|', '{', '}'
            };

        public DotNetNamingConvention()
        {
        }

        public string ValidName(string value)
        {

            if ((value == null) || System.String.IsNullOrEmpty(value.Trim()))
                throw new System.ArgumentNullException($"{nameof(value)} may not be null, empty, or white space");

            foreach (var item in DotNetNamingConvention.invalidChars)
            {
                value = value?.Replace(item, '_');
            }


            var provider = new Microsoft.CSharp.CSharpCodeProvider();
            if (provider.IsValidIdentifier(value))
                return value;

            var chars = string.Join("+|", DotNetNamingConvention.invalidChars.Select(c => System.Text.RegularExpressions.Regex.Escape(System.Convert.ToString(c))));

            var pattern = $"{chars}+|\\s+/g";

            if ((value == null) || System.String.IsNullOrEmpty(value.Trim()))
                throw new System.ArgumentNullException($"{nameof(value)} may not be null, empty, or white space");
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
            return provider.IsValidIdentifier(validName) ? validName : $"V{validName}";
        }

        public string GetNamespace(params string[] values)
            => string.Join(".", values);

        public string GetInterfaceName(string value)
            => ValidName($"I{NamingConvention.GetPascalCase(value)}");

        public string GetClassName(string value)
            => ValidName(NamingConvention.GetPascalCase(value));

        public string GetFieldName(string value)
            => ValidName($"m_{NamingConvention.GetCamelCase(value)}");

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
