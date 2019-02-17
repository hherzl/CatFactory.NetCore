using System;
using CatFactory.CodeFactory;
using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public interface IDotNetObjectDefinition : IObjectDefinition
    {
        bool UseRegionsToGroupClassMembers { get; set; }

        [Obsolete("Set instance for ICodeNamingConvention in CodeBuilder instance")]
        ICodeNamingConvention NamingConvention { get; }
    }
}
