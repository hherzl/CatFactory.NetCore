using System;
using CatFactory.OOP;

namespace CatFactory.DotNetCore
{
    public class CSharpClassDefinition : ClassDefinition, IDotNetClassDefinition
    {
        public CSharpClassDefinition()
        {
        }

        public Boolean UseRegionsToGroupClassMembers { get; set; }
    }
}
