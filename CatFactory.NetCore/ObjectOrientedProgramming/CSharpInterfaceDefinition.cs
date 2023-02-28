using System.Diagnostics;
using CatFactory.CodeFactory;
using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public class CSharpInterfaceDefinition : InterfaceDefinition, IDotNetInterfaceDefinition
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ICodeNamingConvention m_namingConvention;

        public CSharpInterfaceDefinition()
            : base()
        {
        }

        public bool UseRegionsToGroupClassMembers { get; set; }
    }
}
