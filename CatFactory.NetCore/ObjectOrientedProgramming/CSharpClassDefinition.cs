using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming;

public class CSharpClassDefinition : ClassDefinition, IDotNetClassDefinition
{
    public static CSharpClassDefinition Create(AccessModifier accessModifier, string name, string ns = null, string baseClass = null, bool isStatic = false, bool isSealed = false, bool isPartial = false)
    {
        var definition = new CSharpClassDefinition
        {
            AccessModifier = accessModifier,
            Name = name,
            IsStatic = isStatic,
            IsSealed = isSealed,
            IsPartial = isPartial
        };

        if (!string.IsNullOrEmpty(ns))
            definition.Namespace = ns;

        if (!string.IsNullOrEmpty(baseClass))
            definition.BaseClass = baseClass;

        return definition;
    }

    public static ClassConstructorDefinition CreateCtor(AccessModifier accessModifier = AccessModifier.Public, string invocation = null, string summary = null)
    {
        var definition = new ClassConstructorDefinition(accessModifier);

        if (definition.Documentation.HasSummary)
            definition.Documentation.Summary = summary;

        if (!string.IsNullOrEmpty(invocation))
            definition.Invocation = invocation;

        return definition;
    }

    public static PropertyDefinition CreateAutomaticProp(string type, string name, AccessModifier accessModifier = AccessModifier.Public, string initValue = null, MetadataAttribute[] attributes = null)
        => new()
        {
            AccessModifier = accessModifier,
            Type = type,
            Name = name,
            IsAutomatic = true,
            InitializationValue = initValue,
            Attributes = attributes == null ? [] : [.. attributes]
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

    public CSharpClassDefinition()
        : base()
    {
    }

    public bool IsSealed { get; set; }
    public bool UseRegionsToGroupClassMembers { get; set; }
}
