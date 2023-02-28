using System.Diagnostics;
using CatFactory.CodeFactory;
using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public class CSharpEnumDefinition : EnumDefinition, IDotNetEnumDefinition
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ICodeNamingConvention m_namingConvention;

        public CSharpEnumDefinition()
            : base()
        {
        }

        public bool UseRegionsToGroupClassMembers { get; set; }
    }
}
