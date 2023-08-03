using CatFactory.Collections;
using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public static class CSharpClassDefinitionExtensions
    {
        public static CSharpClassDefinition Implement(this CSharpClassDefinition definition, string contract)
        {
            definition.Implements.AddUnique(contract);
            return definition;
        }

        public static CSharpClassDefinition AddDefaultCtor(this CSharpClassDefinition definition, AccessModifier accessModifier = AccessModifier.Public)
        {
            definition.Constructors.Add(new(accessModifier));
            return definition;
        }

        public static CSharpClassDefinition AddCtor(this CSharpClassDefinition definition, ClassConstructorDefinition ctor)
        {
            definition.Constructors.Add(ctor);
            return definition;
        }

        public static CSharpClassDefinition SetDocumentation(this CSharpClassDefinition definition, string summary = null, string remarks = null, string returns = null)
        {
            if (!string.IsNullOrEmpty(summary))
                definition.Documentation.Summary = summary;

            if (!string.IsNullOrEmpty(remarks))
                definition.Documentation.Remarks = remarks;

            if (!string.IsNullOrEmpty(returns))
                definition.Documentation.Returns = returns;

            return definition;
        }

        public static CSharpClassDefinition ImportNs(this CSharpClassDefinition definition, string ns)
        {
            definition.Namespaces.AddUnique(ns);
            return definition;
        }

        public static CSharpClassDefinition ImportNs(this CSharpClassDefinition definition, params string[] ns)
        {
            foreach (var item in ns)
            {
                definition.Namespaces.AddUnique(item);
            }

            return definition;
        }
    }
}
