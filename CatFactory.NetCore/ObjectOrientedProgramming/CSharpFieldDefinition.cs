using System.Collections.Generic;
using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public static  class CSharpFieldDefinition
    {
        public static FieldDefinition Create(string type, string name, AccessModifier accessModifier = AccessModifier.Private, bool isStatic = false, bool isReadOnly = false, string value = null, MetadataAttribute[] attributes = null)
            => new()
            {
                AccessModifier = accessModifier,
                Type = type,
                Name = name,
                IsStatic = isStatic,
                IsReadOnly = isReadOnly,
                Value = value,
                Attributes = attributes == null ? new List<MetadataAttribute>() : new List<MetadataAttribute>(attributes)
            };
    }
}
