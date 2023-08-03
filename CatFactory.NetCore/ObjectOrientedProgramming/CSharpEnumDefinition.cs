using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public class CSharpEnumDefinition : EnumDefinition, IDotNetEnumDefinition
    {
        public static CSharpEnumDefinition Create(AccessModifier accessModifier, string name, bool isPartial = false, string baseType = null, string ns = null, MetadataAttribute[] attributes = null)
            => new()
            {
                AccessModifier = accessModifier,
                Name = name,
                IsPartial = isPartial,
                BaseType = baseType,
                Namespace = ns,
                Attributes = attributes == null ? new() : new(attributes)
            };

        public CSharpEnumDefinition()
            : base()
        {
        }

        public bool UseRegionsToGroupClassMembers { get; set; }
    }
}
