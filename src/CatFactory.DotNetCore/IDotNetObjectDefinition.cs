using System;
using CatFactory.OOP;

namespace CatFactory.DotNetCore
{
    public interface IDotNetObjectDefinition : IObjectDefinition
    {
        Boolean UseRegionsToGroupClassMembers { get; set; }
    }
}
