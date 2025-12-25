using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming;

public interface IDotNetClassDefinition : IDotNetObjectDefinition, IClassDefinition
{
    bool IsSealed { get; set; }
}
