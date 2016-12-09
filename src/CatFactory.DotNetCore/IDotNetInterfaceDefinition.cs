using System;
using CatFactory.OOP;

namespace CatFactory.DotNetCore
{
    public interface IDotNetInterfaceDefinition : IInterfaceDefinition
    {
        Boolean UseRegionsToGroupClassMembers { get; set; }
    }
}
