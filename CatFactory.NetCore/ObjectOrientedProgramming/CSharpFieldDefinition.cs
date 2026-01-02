using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming;

public static class CSharpFieldDefinition
{
    public static FieldDefinition Create(string type, string name, AccessModifier accessModifier = AccessModifier.Private, bool isStatic = false, bool isReadonly = false, string value = null, MetadataAttribute[] attributes = null)
        => new()
        {
            AccessModifier = accessModifier,
            Type = type,
            Name = name,
            IsStatic = isStatic,
            IsReadonly = isReadonly,
            Value = value,
            Attributes = attributes == null ? [] : [.. attributes]
        };
}
