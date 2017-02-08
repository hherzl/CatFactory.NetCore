using System;
using CatFactory.CodeFactory;
using CatFactory.OOP;

namespace CatFactory.DotNetCore
{
    public interface IDotNetObjectDefinition : IObjectDefinition
    {
        Boolean UseRegionsToGroupClassMembers { get; set; }

        ICodeNamingConvention NamingConvention { get; }
    }
}
