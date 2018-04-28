using CatFactory.CodeFactory;
using CatFactory.OOP;

namespace CatFactory.DotNetCore
{
    public class CSharpInterfaceDefinition : InterfaceDefinition, IDotNetInterfaceDefinition
    {
        public CSharpInterfaceDefinition()
            : base()
        {
        }

        public bool UseRegionsToGroupClassMembers { get; set; }

        public ICodeNamingConvention NamingConvention { get; } = new DotNetNamingConvention();
    }
}
