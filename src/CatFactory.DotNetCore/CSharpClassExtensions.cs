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
                new CodeLine(1, "PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(\"{0}\"));", propertyName),
                new CodeLine("}}")
            };

            classDefinition.Fields.Add(new FieldDefinition(property.Type, fieldName) { AccessModifier = AccessModifier.Private });

            classDefinition.Properties.Add(property);
        }

        public static CSharpInterfaceDefinition RefactInterface(this CSharpClassDefinition classDefinition)
        {
            var interfaceDefinition = new CSharpInterfaceDefinition();

            foreach (var @event in classDefinition.Events)
            {
                if (@event.AccessModifier == AccessModifier.Public)
                {
                    interfaceDefinition.Events.Add(new EventDefinition(@event.Type, @event.Name));
                }
            }

            foreach (var property in classDefinition.Properties)
            {
                if (property.AccessModifier == AccessModifier.Public)
                {
                    interfaceDefinition.Properties.Add(new PropertyDefinition(property.Type, property.Name)
                    {
                        IsAutomatic = property.IsAutomatic,
                        IsReadOnly = property.IsReadOnly
                    });
                }
            }

            foreach (var method in classDefinition.Methods)
            {
                if (method.AccessModifier == AccessModifier.Public)
                {
                    interfaceDefinition.Methods.Add(new MethodDefinition(method.Type, method.Name)
                    {
                        Parameters = method.Parameters
                    });
                }
            }

            return interfaceDefinition;
        }
    }
}
