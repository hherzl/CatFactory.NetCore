using CatFactory.CodeFactory;
using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public interface IDotNetObjectDefinition : IObjectDefinition
    {
        bool UseRegionsToGroupClassMembers { get; set; }

        ICodeNamingConvention NamingConvention { get; }
    }
}
