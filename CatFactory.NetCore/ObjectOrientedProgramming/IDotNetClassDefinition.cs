using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public interface IDotNetClassDefinition : IDotNetObjectDefinition, IClassDefinition
    {
        // todo: Remove this flag in next core package update
        bool IsAbstract { get; set; }
    }
}
