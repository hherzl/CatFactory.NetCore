using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CatFactory.CodeFactory;

namespace CatFactory.NetCore.CodeFactory
{
    // ReSharper disable once ClassCanBeSealed.Global
    public class DotNetNamingConvention : ICodeNamingConvention
    {
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public static List<char> invalidChars;

        static DotNetNamingConvention() =>
            invalidChars = new List<char>
            {
                ' ', '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '=', '+', ';', ':', '"', ',', '.', '/', '\\', '?', '|', '{', '}'
            };

        public DotNetNamingConvention()
        {
        }

        public string ValidName(string value)
        {
            if ((value == null) || string.IsNullOrEmpty(value.Trim()))
                throw new ArgumentNullException($"{nameof(value)} may not be null, empty, or white space");

            foreach (var item in invalidChars)
            {
                value = value?.Replace(item, '_');
            }

            var provider = new Microsoft.CSharp.CSharpCodeProvider();
            if (provider.IsValidIdentifier(value))
                return value;

            var chars = string.Join("+|", invalidChars.Select(item => Regex.Escape(Convert.ToString(item))));

            var pattern = $"{chars}+|\\s+/g";

            if ((value == null) || string.IsNullOrEmpty(value.Trim()))
                throw new ArgumentNullException($"{nameof(value)} may not be null, empty, or white space");

            var validName = string.Join("", Regex.Replace(value, pattern, "_", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(0.5)).Split(' ').Select(item => item));

            validName = Regex.Replace(validName, @"__", "_", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(0.5));

            if (provider.IsValidIdentifier(validName))
                return validName;

            // remove all whitespace, digit
            validName = Regex.Replace(validName, @"\s+\d+\p{P}+/g", "_", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(0.5));

            validName = Regex.Replace(validName, @"__", "_", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(0.5));
            if (provider.IsValidIdentifier(validName))
                return validName;

            // remove all non-alphanumeric
            validName = Regex.Replace(validName, @"\W+/g", "_", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(0.5));
            validName = Regex.Replace(validName, @"__", "_", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(0.5));
            if (provider.IsValidIdentifier(validName))
                return validName;

            // remove all non-alphabetic
            validName = Regex.Replace(validName, @"[^a-zA-Z_]+/g", "_", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(0.5));
            validName = Regex.Replace(validName, @"__", "_", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(0.5));

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
