using CatFactory.Collections;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public static class CSharpEnumExtensions
    {
        public static CSharpEnumDefinition SetDocumentation(this CSharpEnumDefinition definition, string summary = null, string remarks = null, string returns = null)
        {
            if (!string.IsNullOrEmpty(summary))
                definition.Documentation.Summary = summary;

            if (!string.IsNullOrEmpty(remarks))
                definition.Documentation.Remarks = remarks;

            if (!string.IsNullOrEmpty(returns))
                definition.Documentation.Returns = returns;

            return definition;
        }

        public static CSharpEnumDefinition ImportNs(this CSharpEnumDefinition definition, string ns)
        {
            definition.Namespaces.AddUnique(ns);
            return definition;
        }

        public static CSharpEnumDefinition ImportNs(this CSharpEnumDefinition definition, params string[] ns)
        {
            foreach (var item in ns)
            {
                definition.Namespaces.AddUnique(item);
            }

            return definition;
        }

        public static CSharpEnumDefinition AddAttrib(this CSharpEnumDefinition definition, string name, params string[] arguments)
        {
            definition.Attributes.Add(new(name, arguments));
            return definition;
        }

        public static CSharpEnumDefinition AddSet(this CSharpEnumDefinition definition, string name, int value)
        {
            definition.Sets.Add(new(name, value.ToString()));
            return definition;
        }

        public static CSharpEnumDefinition AddSet(this CSharpEnumDefinition definition, string name, byte value)
        {
            definition.Sets.Add(new(name, value.ToString()));
            return definition;
        }
    }
}
