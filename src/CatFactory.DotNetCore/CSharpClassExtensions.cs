using System;
using System.Collections.Generic;
using System.Linq;
using CatFactory.CodeFactory;
using CatFactory.OOP;

namespace CatFactory.DotNetCore
{
    public static class CSharpClassExtensions
    {
        public static void AddReadOnlyProperty(this CSharpClassDefinition classDefinition, String type, String name, params CodeLine[] codeLines)
        {
            var property = new PropertyDefinition(type, name) { IsReadOnly = true };

            property.GetBody = new List<CodeLine>(codeLines);

            classDefinition.Properties.Add(property);
        }

        public static void AddViewModelProperty(this CSharpClassDefinition classDefinition, String type, String name)
        {
            var namingConvention = new DotNetNamingConvention();

            var fieldName = namingConvention.GetFieldName(name);

            var property = new PropertyDefinition(type, name) { IsAutomatic = false };

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
                new CodeLine(1, "PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof({0})));", name),
                new CodeLine("}}")
            };

            classDefinition.Fields.Add(new FieldDefinition(property.Type, fieldName) { AccessModifier = AccessModifier.Private });

            classDefinition.Properties.Add(property);
        }

        public static CSharpInterfaceDefinition RefactInterface(this CSharpClassDefinition classDefinition, params String[] exclusions)
        {
            var interfaceDefinition = new CSharpInterfaceDefinition();
            var namingConvention = new DotNetNamingConvention();

            interfaceDefinition.Name = namingConvention.GetInterfaceName(classDefinition.Name);

            interfaceDefinition.Namespaces = classDefinition.Namespaces;

            foreach (var @event in classDefinition.Events)
            {
                if (exclusions.Contains(@event.Name))
                {
                    continue;
                }

                if (@event.AccessModifier == AccessModifier.Public)
                {
                    interfaceDefinition.Events.Add(new EventDefinition(@event.Type, @event.Name));
                }
            }

            foreach (var property in classDefinition.Properties)
            {
                if (exclusions.Contains(property.Name))
                {
                    continue;
                }

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
                if (exclusions.Contains(method.Name))
                {
                    continue;
                }

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
