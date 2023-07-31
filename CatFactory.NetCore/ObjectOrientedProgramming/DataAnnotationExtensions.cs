using System.ComponentModel.DataAnnotations.Schema;
using CatFactory.Collections;
using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public static class DataAnnotationExtensions
    {
        private const string COLUMN = "Column";
        private const string DATABASE_GENERATED = "DatabaseGenerated";
        private const string DATABASE_GENERATED_OPTION = "DatabaseGeneratedOption";
        private const string ERROR_MESSAGE = "ErrorMessage";
        private const string ERROR_MESSAGE_RESOURCE_NAME = "ErrorMessageResourceName";
        private const string ERROR_MESSAGE_RESOURCE_TYPE = "ErrorMessageResourceType";
        private const string KEY = "Key";
        private const string REQUIRED = "Required";
        private const string STRING_LENGTH = "StringLength";
        private const string TABLE = "Table";

        public static CSharpClassDefinition AddTableAttrib(this CSharpClassDefinition definition, string name, string schema = null)
        {
            var metadataAttrib = new MetadataAttribute(TABLE, $"\"{name}\"");

            if (!string.IsNullOrEmpty(schema))
                metadataAttrib.Sets.Add(new MetadataAttributeSet("Schema", $"\"{schema}\""));

            definition.Attributes.Add(metadataAttrib);
            definition.Namespaces.AddUnique("System.ComponentModel.DataAnnotations.Schema");

            return definition;
        }

        public static PropertyDefinition AddKeyAnnotation(this PropertyDefinition definition)
        {
            var metadataAttrib = new MetadataAttribute(KEY);

            definition.Attributes.Add(metadataAttrib);

            return definition;
        }

        public static PropertyDefinition AddDatabaseGeneratedAttrib(this PropertyDefinition definition, DatabaseGeneratedOption databaseGeneratedOption = DatabaseGeneratedOption.Identity)
        {
            var metadataAttrib = new MetadataAttribute(DATABASE_GENERATED, $"{DATABASE_GENERATED_OPTION}.{databaseGeneratedOption}");

            definition.Attributes.Add(metadataAttrib);

            return definition;
        }

        public static PropertyDefinition AddColumnAttrib(this PropertyDefinition definition)
        {
            var metadataAttrib = new MetadataAttribute(COLUMN);

            definition.Attributes.Add(metadataAttrib);

            return definition;
        }

        public static PropertyDefinition AddRequiredAttrib(this PropertyDefinition definition, string errorMessage = null, string errorMessageResourceName = null, string errorMessageResourceType = null)
        {
            var metadataAttrib = new MetadataAttribute(REQUIRED);

            if (!string.IsNullOrEmpty(errorMessage))
                metadataAttrib.Sets.Add(new MetadataAttributeSet(ERROR_MESSAGE, $"\"{errorMessage}\""));

            if (!string.IsNullOrEmpty(errorMessageResourceName))
                metadataAttrib.Sets.Add(new MetadataAttributeSet(ERROR_MESSAGE_RESOURCE_NAME, $"\"{errorMessageResourceName}\""));

            if (!string.IsNullOrEmpty(errorMessageResourceType))
                metadataAttrib.Sets.Add(new MetadataAttributeSet(ERROR_MESSAGE_RESOURCE_TYPE, $"typeof = {errorMessageResourceType}"));

            definition.Attributes.Add(metadataAttrib);

            return definition;
        }

        public static PropertyDefinition AddStringLengthAttrib(this PropertyDefinition definition, int length)
        {
            var metadataAttrib = new MetadataAttribute(STRING_LENGTH, length.ToString());

            definition.Attributes.Add(metadataAt