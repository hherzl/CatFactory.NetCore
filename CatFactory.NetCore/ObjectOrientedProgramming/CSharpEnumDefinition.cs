using System;
using System.Diagnostics;
using CatFactory.CodeFactory;
using CatFactory.NetCore.CodeFactory;
using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public class CSharpEnumDefinition : EnumDefinition, IDotNetEnumDefinition
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private ICodeNamingConvention m_namingConvention;

        public CSharpEnumDefinition()
            : base()
        {
        }

        public bool UseRegionsToGroupClassMembers { get; set; }

        [Obsolete("Set instance for ICodeNamingConvention in CodeBuilder instance")]
        public ICodeNamingConvention NamingConvention
        {
            get => m_namingConvention ?? (m_namingConvention = new DotNetNamingConvention());
            set => m_namingConvention = value;
        }
    }
}
