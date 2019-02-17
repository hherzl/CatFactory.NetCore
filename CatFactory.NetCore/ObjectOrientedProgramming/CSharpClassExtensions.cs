using System.Collections.Generic;
using System.Linq;
using CatFactory.CodeFactory;
using CatFactory.NetCore.CodeFactory;
using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public static class CSharpClassExtensions
    {
        public static ICodeNamingConvention NamingConvention;

        static CSharpClassExtensions()
        {
            NamingConvention = new DotNetNamingConvention();
        }

        public static void AddReadOnlyProperty(this CSharpClassDefinition classDefinition, string type, string name, params CodeLine[] codeLines)
        {
            var property = new PropertyDefinition(type, name)
            {
                IsReadOnly = true,
                GetBody = new List<ILine>(codeLines)
            };

            classDefinition.Properties.Add(property);
        }

        public static void AddPropertyWithField(this CSharpClassDefinition classDefinition, string type, string name)
        {
            var fieldName = NamingConvention.GetFieldName(name);

            var property = new PropertyDefinition(AccessModifier.Public, type, name)
            {
                IsAutomatic = false,
                GetBody =
                {
                    new CodeLine("return {0};", fieldName)
                },
                SetBody =
                {
                    new CodeLine("{0} = value;", fieldName)
                }
            };

            classDefinition.Fields.Add(new FieldDefinition(AccessModifier.Private, property.Type, fieldName));

            classDefinition.Properties.Add(property);
        }

        public static void AddViewModelProperty(this CSharpClassDefinition classDefinition, string type, string name, bool useNullConditionalOperator = true)
        {
            var fieldName = NamingConvention.GetFieldName(name);

            var property = new PropertyDefinition(AccessModifier.Public, type, name)
            {
                IsAutomatic = false,
                GetBody =
                {
                    new CodeLine("return {0};", fieldName)
                }
            };

            property.SetBody.Add(new CodeLine("if ({0} != value)", fieldName));
            property.SetBody.Add(new CodeLine("{"));
            property.SetBody.Add(new CodeLine(1, "{0} = value;", fieldName));
            property.SetBody.Add(new CodeLine());

            if (useNullConditionalOperator)
            {
                property.SetBody.Add(new CodeLine(1, "PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof({0})));", name));
            }
            else
            {
                property.SetBody.Add(new CodeLine(1, "if (PropertyChanged != null)"));
                property.SetBody.Add(new CodeLine(1, "{"));
                property.SetBody.Add(new CodeLine(2, "PropertyChanged(this, new PropertyChangedEventArgs(nameof({0})));", name));
                property.SetBody.Add(new CodeLine(1, "}"));
            }

            property.SetBody.Add(new CodeLine("}"));

            classDefinition.Fields.Add(new FieldDefinition(AccessModifier.Private, property.Type, fieldName));

            classDefinition.Properties.Add(property);
        }

        public static CSharpInterfaceDefinition RefactInterface(this CSharpClassDefinition classDefinition, params string[] exclusions)
        {
            var interfaceDefinition = new CSharpInterfaceDefinition
            {
                AccessModifier = classDefinition.AccessModifier,
                Namespaces = classDefinition.Namespaces,
                Name = NamingConvention.GetInterfaceName(classDefinition.Name)
            };

            foreach (var @event in classDefinition.Events.Where(item => item.AccessModifier == AccessModifier.Public && !exclusions.Contains(item.Name)))
            {
                interfaceDefinition.Events.Add(new EventDefinition(@event.AccessModifier, @event.Type, @event.Name));
            }

            foreach (var property in classDefinition.Properties.Where(item => item.AccessModifier == AccessModifier.Public && !exclusions.Contains(item.Name)))
            {
                interfaceDefinition.Properties.Add(new PropertyDefinition(property.AccessModifier, property.Type, property.Name)
                {
                    IsAutomatic = property.IsAutomatic,
                    IsReadOnly = property.IsReadOnly
                });
            }

            foreach (var method in classDefinition.Methods.Where(item => item.AccessModifier == AccessModifier.Public && !exclusions.Contains(item.Name)))
            {
                interfaceDefinition.Methods.Add(new MethodDefinition(method.AccessModifier, method.Type, method.Name, method.Parameters.ToArray()));
            }

            return interfaceDefinition;
        }
    }
}
