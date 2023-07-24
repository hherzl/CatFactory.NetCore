using CatFactory.Collections;
using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public static class DataAnnotationExtensions
    {
        public static CSharpClassDefinition AddTableAnnotation(this CSharpClassDefinition definition, string name, string schema = null)
        {
            var metadataAttrib = new MetadataAttribute("Table", $"\"{name}\"");

            if (!string.IsNullOrEmpty(schema))
                metadataAttrib.Sets.Add(new MetadataAttributeSet("Schema", $"\"{schema}\""));

            definition.Attributes.Add(metadataAttrib);
            definition.Namespaces.AddUnique("System.ComponentModel.DataAnnotations.Schema");

            return definition;
        }

        public static PropertyDefinition AddKeyAnnotation(this PropertyDefinition definition)
        {
            var metadataAttrib = new MetadataAttribute("Key");

            definition.Attributes.Add(metadataAttrib);

            return definition;
        }

        public static PropertyDefinition AddDatabaseGeneratedAnnotation(this PropertyDefinition definition)
        {
            var metadataAttrib = new MetadataAttribute("DatabaseGenerated", "DatabaseGeneratedOption.Identity");

            definition.Attributes.Add(metadataAttrib);

            return definition;
        }

        public static PropertyDefinition AddColumnAnnotation(this PropertyDefinition definition)
        {
            var metadataAttrib = new MetadataAttribute("Column");

            definition.Attributes.Add(metadataAttrib);

            return definition;
        }

        public static PropertyDefinition AddRequiredAnnotation(this PropertyDefinition definition)
        {
            var metadataAttrib = new MetadataAttribute("Required");

            definition.Attributes.Add(metadataAttrib);

            return definition;
        }

        public static PropertyDefinition AddStringLengthAnnotation(this PropertyDefinition definition, int length)
        {
            var metadataAttrib = new MetadataAttribute("StringLength", length.ToString());

            definition.Attributes.Add(metadataAttrib);

            return definition;
        }
    }
}
