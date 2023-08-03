using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public class CSharpInterfaceDefinition : InterfaceDefinition, IDotNetInterfaceDefinition
    {
        public static CSharpInterfaceDefinition Create(AccessModifier accessModifier, string name, string ns = null, bool isPartial = false)
        {
            var definition = new CSharpInterfaceDefinition
            {
                AccessModifier = accessModifier,
                Name = name,
                IsPartial = isPartial
            };

            if (!string.IsNullOrEmpty(ns))
                definition.Namespace = ns;

            return definition;
        }

        public CSharpInterfaceDefinition()
            : base()
        {
        }

        public bool UseRegionsToGroupClassMembers { get; set; }
    }
}
