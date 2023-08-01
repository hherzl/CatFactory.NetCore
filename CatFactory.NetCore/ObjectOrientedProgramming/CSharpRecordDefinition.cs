using System.Collections.Generic;
using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public class CSharpRecordDefinition : RecordDefinition, IDotNetRecordDefinition
    {
        public static CSharpRecordDefinition Create(AccessModifier accessModifier, string name, string ns = null, string baseRecord = null)
        {
            var definition = new CSharpRecordDefinition
            {
                AccessModifier = accessModifier,
                Name = name
            };

            if (!string.IsNullOrEmpty(ns))
                definition.Namespace = ns;

            if (!string.IsNullOrEmpty(baseRecord))
                definition.BaseRecord = baseRecord;

            return definition;
        }

        public static ClassConstructorDefinition CreateCtor(AccessModifier accessModifier = AccessModifier.Public, string invocation = null)
        {
            var definition = new ClassConstructorDefinition(accessModifier);

            if (!string.IsNullOrEmpty(invocation))
                definition.Invocation = invocation;

            return definition;
        }

        public static PropertyDefinition CreateAutomaticProp(string type, string name, AccessModifier accessModifier = AccessModifier.Public, MetadataAttribute[] attributes = null)
            => new()
            {
                AccessModifier = accessModifier,
                Type = type,
                Name = name,
                IsAutomatic = true,
                Attributes = attributes == null ? new List<MetadataAttribute>() : new List<MetadataAttribute>(attributes)
            };

        public static PropertyDefinition CreateReadonlyProp(string type, string name, AccessModifier accessModifier = AccessModifier.Public)
            => new()
            {
                AccessModifier = accessModifier,
                Type = type,
                Name = name,
                IsAutomatic = true,
                IsReadOnly = true
            };

        public static PropertyDefinition CreatePositionalProp(string type, string name, AccessModifier accessModifier = AccessModifier.Public, MetadataAttribute[] attributes = null)
            => new()
            {
                AccessModifier = accessModifier,
                Type = type,
                Name = name,
                IsAutomatic = false,
                IsPositional = true,
                Attributes = attributes == null ? new List<MetadataAttribute>() : new List<MetadataAttribute>(attributes)
            };

        public CSharpRecordDefinition()
            : base()
        {
        }

        public bool UseRegionsToGroupClassMembers { get; set; }
    }
}
