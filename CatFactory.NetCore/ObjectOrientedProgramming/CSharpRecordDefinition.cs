using System.Collections.Generic;
using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public class CSharpRecordDefinition : RecordDefinition, IDotNetRecordDefinition
    {
        public static PropertyDefinition CreateAutomaticProperty(string type, string name, AccessModifier accessModifier = AccessModifier.Public, MetadataAttribute[] attributes = null)
            => new()
            {
                AccessModifier = accessModifier,
                Type = type,
                Name = name,
                IsAutomatic = true,
                Attributes = attributes == null ? new List<MetadataAttribute>() : new List<MetadataAttribute>(attributes)
            };

        public static PropertyDefinition CreateReadonlyProperty(string type, string name, AccessModifier accessModifier = AccessModifier.Public)
            => new()
            {
                AccessModifier = accessModifier,
                Type = type,
                Name = name,
                IsAutomatic = true,
                IsReadOnly = true
            };

        public CSharpRecordDefinition()
            : base()
        {
        }

        public bool UseRegionsToGroupClassMembers { get; set; }
    }
}
