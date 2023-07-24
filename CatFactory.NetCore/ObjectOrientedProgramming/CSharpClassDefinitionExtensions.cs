using CatFactory.Collections;
using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public static class CSharpClassDefinitionExtensions
    {
        public static CSharpClassDefinition IsPartial(this CSharpClassDefinition definition, bool isPartial = true)
        {
            definition.IsPartial = isPartial;

            return definition;
        }

        public static CSharpClassDefinition IsStatic(this CSharpClassDefinition definition, bool isStatic = true)
        {
            definition.IsStatic = isStatic;

            return definition;
        }

        public static CSharpClassDefinition Implement(this CSharpClassDefinition definition, string contract)
        {
            definition.Implements.AddUnique(contract);

            return definition;
        }

        public static CSharpClassDefinition AddDefaultCtor(this CSharpClassDefinition definition)
        {
            definition.Constructors.Add(new ClassConstructorDefinition());

            return definition;
        }

        public static CSharpClassDefinition SetSummary(this CSharpClassDefinition definition, string summary)
        {
            definition.Documentation = new Documentation(summary);

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
