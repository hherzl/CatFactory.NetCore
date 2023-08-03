using System;
using System.Linq;
using CatFactory.CodeFactory;
using CatFactory.Collections;
using CatFactory.NetCore.CodeFactory;
using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public static  class CSharpRecordExtensions
    {
        public static CSharpRecordDefinition ImportNs(this CSharpRecordDefinition definition, string ns)
        {
            definition.Namespaces.AddUnique(ns);
            return definition;
        }

        public static CSharpRecordDefinition ImportNs(this CSharpRecordDefinition definition, params string[] ns)
        {
            foreach (var item in ns)
            {
                definition.Namespaces.AddUnique(item);
            }

            return definition;
        }

        public static CSharpRecordDefinition Implement(this CSharpRecordDefinition definition, string contract)
        {
            definition.Implements.AddUnique(contract);
            return definition;
        }

        public static void AddViewModelProp(this CSharpRecordDefinition recordDefinition, string type, string name, string fieldName = null, ICodeNamingConvention namingConvention = null, bool useNullConditionalOperator = true)
        {
            namingConvention ??= new DotNetNamingConvention();

            if (string.IsNullOrEmpty(fieldName))
                fieldName = namingConvention.GetFieldName(name);

            var property = new PropertyDefinition(AccessModifier.Public, type, name)
            {
                IsAutomatic = false,
                GetBody =
                {
                    new ReturnLine("{0};", fieldName)
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

            recordDefinition.Fields.Add(new FieldDefinition(AccessModifier.Private, property.Type, fieldName));

            recordDefinition.Properties.Add(property);
        }

        public static CSharpInterfaceDefinition RefactInterface(this CSharpRecordDefinition recordDefinition, ICodeNamingConvention namingConvention = null, params string[] exclusions)
        {
            namingConvention ??= new DotNetNamingConvention();

            var interfaceDefinition = new CSharpInterfaceDefinition
            {
                AccessModifier = recordDefinition.AccessModifier,
                Namespaces = recordDefinition.Namespaces,
                Name = namingConvention.GetInterfaceName(recordDefinition.Name)
            };

            foreach (var @event in recordDefinition.Events.Where(item => item.AccessModifier == AccessModifier.Public && !exclusions.Contains(item.Name)))
            {
                interfaceDefinition.Events.Add(new EventDefinition(@event.AccessModifier, @event.Type, @event.Name));
            }

            foreach (var property in recordDefinition.Properties.Where(item => item.AccessModifier == AccessModifier.Public && !exclusions.Contains(item.Name)))
            {
                interfaceDefinition.Properties.Add(new PropertyDefinition(property.AccessModifier, property.Type, property.Name)
                {
                    IsAutomatic = property.IsAutomatic,
                    IsReadOnly = property.IsReadOnly
                });
            }

            foreach (var method in recordDefinition.Methods.Where(item => item.AccessModifier == AccessModifier.Public && !exclusions.Contains(item.Name)))
            {
                interfaceDefinition.Methods.Add(new MethodDefinition(method.AccessModifier, method.Type, method.Name, method.Parameters.ToArray()));
            }

            return interfaceDefinition;
        }
    }
}
