using System;
using CatFactory.OOP;

namespace CatFactory.DotNetCore
{
    public interface IDotNetClassDefinition : IClassDefinition
    {
        Boolean UseRegionsToGroupClassMembers { get; set; }
    }
}
