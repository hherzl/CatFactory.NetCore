using CatFactory.Collections;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public static class CSharpInterfaceDefinitionExtensions
    {
        public static CSharpInterfaceDefinition Implement(this CSharpInterfaceDefinition definition, string contract)
        {
            definition.Implements.AddUnique(contract);
            return definition;
        }

        public static CSharpInterfaceDefinition ImportNs(this CSharpInterfaceDefinition definition, string ns)
        {
            definition.Namespaces.AddUnique(ns);
            return definition;
        }

        public static CSharpInterfaceDefinition ImportNs(this CSharpInterfaceDefinition definition, params string[] ns)
        {
            foreach (var item in ns)
            {
                definition.Namespaces.AddUnique(item);
            }

            return definition;
        }

        public static CSharpInterfaceDefinition SetDocumentation(this CSharpInterfaceDefinition definition, string summary = null, string remarks = null, string returns = null)
        {
            if (!string.IsNullOrEmpty(summary))
                definition.Documentation.Summary = summary;

            if (!string.IsNullOrEmpty(remarks))
                definition.Documentation.Remarks = remarks;

            if (!string.IsNullOrEmpty(returns))
                definition.Documentation.Returns = returns;

            return definition;
        }
    }
}
