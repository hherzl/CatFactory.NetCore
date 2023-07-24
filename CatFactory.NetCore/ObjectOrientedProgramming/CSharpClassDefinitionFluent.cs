using CatFactory.Collections;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public static class CSharpClassDefinitionFluent
    {
        public static CSharpClassDefinition AddNs(this CSharpClassDefinition definition, string ns)
        {
            definition.Namespaces.AddUnique(ns);

            return definition;
        }
    }
}
