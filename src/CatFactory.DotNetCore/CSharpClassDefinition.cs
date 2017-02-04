using System;
using CatFactory.CodeFactory;
using CatFactory.OOP;

namespace CatFactory.DotNetCore
{
    public class CSharpClassDefinition : ClassDefinition, IDotNetClassDefinition
    {
        public CSharpClassDefinition()
            : base()
        {
        }

        public Boolean UseRegionsToGroupClassMembers { get; set; }

        public INamingConvention NamingConvention { get; } = new DotNetNamingConvention();
    }
}
