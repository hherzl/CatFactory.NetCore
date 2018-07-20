using CatFactory.CodeFactory;
using CatFactory.OOP;

namespace CatFactory.NetCore
{
    public interface IDotNetObjectDefinition : IObjectDefinition
    {
        bool UseRegionsToGroupClassMembers { get; set; }

        ICodeNamingConvention NamingConvention { get; }
    }
}
