using System.Linq;
using CatFactory.CodeFactory;
using CatFactory.NetCore.CodeFactory;
using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public static class CSharpClassExtensions
    {
        public static void AddPropWithField(this CSharpClassDefinition classDefinition, string type, string name, string fieldName = null, ICodeNamingConvention namingConvention = null)
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
                },
                SetBody =
                {
                    new CodeLine("{0} = value;", fieldName)
                }
            };

            classDefinition.Fields.Add(new(AccessModifier.Private, property.Type, fieldName));

            classDefinition.Properties.Add(property);
        }

        public static void AddViewModelProp(this CSharpClassDefinition classDefinition, string type, string name, string fieldName = null, ICodeNamingConvention namingConvention = null, bool useNullConditionalOperator = true)
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

            classDefinition.Fields.Add(new(AccessModifier.Private, property.Type, fieldName));

            classDefinition.Properties.Add(property);
        }

        public static CSharpInterfaceDefinition RefactInterface(this CSharpClassDefinition classDefinition, ICodeNamingConvention namingConvention = null, params string[] exclusions)
        {
            namingConvention ??= new DotNetNamingConvention();

            var interfaceDefinition = new CSharpInterfaceDefinition
            {
                AccessModifier = classDefinition.AccessModifier,
                Namespaces = classDefinition.Namespaces,
                Name = namingConvention.GetInterfaceName(classDefinition.Name)
            };

            foreach (var @event in classDefinition.Events.Where(item => item.AccessModifier == AccessModifier.Public && !exclusions.Contains(item.Name)))
            {
                interfaceDefinition.Events.Add(new(@event.AccessModifier, @event.Type, @event.Name));
            }

            foreach (var property in classDefinition.Properties.Where(item => item.AccessModifier == AccessModifier.Public && !exclusions.Contains(item.Name)))
            {
                interfaceDefinition.Properties.Add(new(property.AccessModifier, property.Type, property.Name)
                {
                    IsAutomatic = property.IsAutomatic,
                    IsReadOnly = property.IsReadOnly
                });
            }

            foreach (var method in classDefinition.Methods.Where(item => item.AccessModifier == AccessModifier.Public && !exclusions.Contains(item.Name)))
            {
                interfaceDefinition.Methods.Add(new(method.AccessModifier, method.Type, method.Name, method.Parameters.ToArray()));
            }

            return interfaceDefinition;
        }
    }
}
