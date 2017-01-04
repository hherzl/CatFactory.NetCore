using System;
using System.Collections.Generic;
using CatFactory.CodeFactory;
using CatFactory.OOP;

namespace CatFactory.DotNetCore
{
    public static class CSharpClassExtensions
    {
        public static void AddViewModelProperty(this CSharpClassDefinition classDefinition, String type, String name)
        {
            var namingConvention = new DotNetNamingConvention();

            var propertyName = namingConvention.GetPropertyName(name);
            var fieldName = namingConvention.GetFieldName(name);

            var property = new PropertyDefinition(type, propertyName) { IsAutomatic = false };

            property.GetBody = new List<CodeLine>()
            {
                new CodeLine("return {0};", fieldName)
            };

            property.SetBody = new List<CodeLine>()
            {
                new CodeLine("if ({0} != value)", fieldName),
                new CodeLine("{{"),
                new CodeLine(1, "{0} = value;", fieldName),
                new CodeLine(),
                new CodeLine(1, "PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(\"{0}\"));", fieldName),
                new CodeLine("}}")
            };

            classDefinition.Fields.Add(new FieldDefinition(property.Type, fieldName) { AccessModifier = AccessModifier.Private });
            classDefinition.Properties.Add(property);
        }
    }
}
