using System;
using CatFactory.OOP;

namespace CatFactory.DotNetCore
{
    public class CSharpInterfaceDefinition : InterfaceDefinition, IDotNetInterfaceDefinition
    {
        public CSharpInterfaceDefinition()
        {
            NamingConvention = new DotNetNamingConvention();
        }

        public Boolean UseRegionsToGroupClassMembers { get; set; }
    }
}
