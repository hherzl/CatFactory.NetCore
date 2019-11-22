using System;
using System.Collections.Generic;
using System.Diagnostics;
using CatFactory.CodeFactory;
using CatFactory.NetCore.CodeFactory;
using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public class CSharpClassDefinition : ClassDefinition, IDotNetClassDefinition
    {
        public static PropertyDefinition CreateAutomaticProperty(string type, string name, AccessModifier accessModifier = AccessModifier.Public, MetadataAttribute[] attributes = null)
            => new PropertyDefinition
            {
                AccessModifier = accessModifier,
                Type = type,
                Name = name,
                IsAutomatic = true,
                Attributes = attributes == null ? new List<MetadataAttribute>() : new List<MetadataAttribute>(attributes)
            };

        public static PropertyDefinition CreateReadonlyProperty(string type, string name, AccessModifier accessModifier = AccessModifier.Public)
            => new PropertyDefinition
            {
                AccessModifier = accessModifier,
                Type = type,
                Name = name,
                IsAutomatic = true,
                IsReadOnly = true
            };

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ICodeNamingConvention m_namingConvention;

        public CSharpClassDefinition()
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
