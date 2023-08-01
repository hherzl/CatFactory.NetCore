using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public class CSharpMethodDefinition
    {
        public static MethodDefinition Create(AccessModifier accessModifier, string type, string name, bool isAsync = false, bool isExtension = false, bool isOverride = false, CSharpClassDefinition target = null)
        {
            var definition = new MethodDefinition(accessModifier, type, name)
            {
                IsAsync = isAsync
            };

            if (isExtension)
            {
                definition.IsStatic = isExtension;
                definition.IsExtension = isExtension;
            }

            definition.IsOverride = isOverride;

            target?.Methods.Add(definition);

            return definition;
        }
    }
}
