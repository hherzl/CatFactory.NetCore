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
            var property = new PropertyDefinition(type, name)
            {
                IsReadOnly = true,
                GetBody = new List<CodeLine>(codeLines)
            };
            
            classDefinition.Properties.Add(property);
        }

        public static void AddPropertyWithField(this CSharpClassDefinition classDefinition, String type, String name)
        {
            var namingConvention = new DotNetNamingConvention();

            var fieldName = namingConvention.GetFieldName(name);

            var property = new PropertyDefinition(type, name)
            {
                IsAutomatic = false,
                GetBody = new List<CodeLine>()
                {
                    new CodeLine("return {0};", fieldName)
                },
                SetBody = new List<CodeLine>()
                {
                    new CodeLine("{0} = value;", fieldName)
                }
            };

            classDefinition.Fields.Add(new FieldDefinition(AccessModifier.Private, property.Type, fieldName));

            classDefinition.Properties.Add(property);
        }

        public static void AddViewModelProperty(this CSharpClassDefinition classDefinition, String type, String name, Boolean useNullConditionalOperatorAndUseNameOfOperator = true)
        {
            var namingConvention = new DotNetNamingConvention();

            var fieldName = namingConvention.GetFieldName(name);

            var property = new PropertyDefinition(type, name)
            {
                IsAutomatic = false,
                GetBody = new List<CodeLine>()
                {
                    new CodeLine("return {0};", fieldName)
                }
            };

            property.SetBody.Add(new CodeLine("if ({0} != value)", fieldName));
            property.SetBody.Add(new CodeLine("{{"));
            property.SetBody.Add(new CodeLine(1, "{0} = value;", fieldName));
            property.SetBody.Add(new CodeLine());

            if (useNullConditionalOperatorAndUseNameOfOperator)
            {
                property.SetBody.Add(new CodeLine(1, "PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof({0})));", name));
            }
            else
            {
                property.SetBody.Add(new CodeLine(1, "if (PropertyChanged != null)"));
                property.SetBody.Add(new CodeLine(1, "{{"));
                property.SetBody.Add(new CodeLine(2, "PropertyChanged(this, new PropertyChangedEventArgs(\"{0}\"));", name));
                property.SetBody.Add(new CodeLine(1, "}}"));
            }

            property.SetBody.Add(new CodeLine("}}"));

            classDefinition.Fields.Add(new FieldDefinition(AccessModifier.Private, property.Type, fieldName));

            classDefinition.Properties.Add(property);
        }

        public static CSharpInterfaceDefinition RefactInterface(this CSharpClassDefinition classDefinition, params String[] exclusions)
        {
            var interfaceDefinition = new CSharpInterfaceDefinition();
            var namingConvention = new DotNetNamingConvention();

            interfaceDefinition.Name = namingConvention.GetInterfaceName(classDefinition.Name);

            interfaceDefinition.Namespaces = classDefinition.Namespaces;

            foreach (var @event in classDefinition.Events.Where(item => item.AccessModifier == AccessModifier.Public && !exclusions.Contains(item.Name)))
            {
                interfaceDefinition.Events.Add(new EventDefinition(@event.Type, @event.Name));
            }

            foreach (var property in classDefinition.Properties.Where(item => item.AccessModifier == AccessModifier.Public && !exclusions.Contains(item.Name)))
            {
                interfaceDefinition.Properties.Add(new PropertyDefinition(property.Type, property.Name)
                {
                    IsAutomatic = property.IsAutomatic,
                    IsReadOnly = property.IsReadOnly
                });
            }

            foreach (var method in classDefinition.Methods.Where(item => item.AccessModifier == AccessModifier.Public && !exclusions.Contains(item.Name)))
            {
                interfaceDefinition.Methods.Add(new MethodDefinition(method.Type, method.Name, method.Parameters.ToArray()));
            }

            return interfaceDefinition;
        }
    }
}
