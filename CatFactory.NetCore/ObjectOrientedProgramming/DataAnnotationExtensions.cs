using System.ComponentModel.DataAnnotations.Schema;
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

        public static PropertyDefinition AddDatabaseGeneratedAnnotation(this PropertyDefinition definition, DatabaseGeneratedOption databaseGeneratedOption = DatabaseGeneratedOption.Identity)
        {
            var arg = "DatabaseGeneratedOption.None";

            if (databaseGeneratedOption == DatabaseGeneratedOption.Identity)
                arg = "DatabaseGeneratedOption.Identity";
            else if (databaseGeneratedOption == DatabaseGeneratedOption.Computed)
                arg = "DatabaseGeneratedOption.Computed";

            var metadataAttrib = new MetadataAttribute("DatabaseGenerated", arg);

            definition.Attributes.Add(metadataAttrib);

            return definition;
        }

        public static PropertyDefinition AddColumnAnnotation(this PropertyDefinition definition)
        {
            var metadataAttrib = new MetadataAttribute("Column");

            definition.Attributes.Add(metadataAttrib);

            return definition;
        }

        public static PropertyDefinition AddRequiredAnnotation(this PropertyDefinition definition, string errorMessage = null, string errorMessageResourceName = null, string errorMessageResourceType = null)
        {
            var metadataAttrib = new MetadataAttribute("Required");

            if (!string.IsNullOrEmpty(errorMessage))
                metadataAttrib.Sets.Add(new MetadataAttributeSet("ErrorMessage", $"\"{errorMessage}\""));

            if (!string.IsNullOrEmpty(errorMessageResourceName))
                metadataAttrib.Sets.Add(new MetadataAttributeSet("ErrorMessageResourceName", $"\"{errorMessageResourceName}\""));

            if (!string.IsNullOrEmpty(errorMessageResourceType))
                metadataAttrib.Sets.Add(new MetadataAttributeSet("ErrorMessageResourceType", $"typeof = {errorMessageResourceType}"));

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
