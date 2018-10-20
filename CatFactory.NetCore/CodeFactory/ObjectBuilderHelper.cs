using System.Collections.Generic;
using System.Linq;
using System.Text;
using CatFactory.CodeFactory;
using CatFactory.OOP;

namespace CatFactory.NetCore.CodeFactory
{
    public static class ObjectBuilderHelper
    {
        private static IEnumerable<string> GetAttributes(List<MetadataAttribute> attributes)
        {
            foreach (var attribute in attributes)
            {
                var attributeDefinition = new StringBuilder();

                attributeDefinition.Append("[");

                attributeDefinition.AppendFormat("{0}", attribute.Name);

                if (attribute.HasMembers)
                {
                    attributeDefinition.Append("(");

                    if (attribute.HasArguments)
                        attributeDefinition.Append(string.Join(", ", attribute.Arguments));

                    if (attribute.HasArguments && attribute.HasSets)
                        attributeDefinition.Append(", ");

                    if (attribute.HasSets)
                        attributeDefinition.AppendFormat("{0}", string.Join(", ", attribute.Sets.Select(item => string.Format("{0} = {1}", item.Name, item.Value))));

                    attributeDefinition.Append(")");
                }

                attributeDefinition.Append("]");

                yield return attributeDefinition.ToString();
            }
        }

        private static void AddAttributes(CodeBuilder codeBuilder, List<MetadataAttribute> attributes, int start)
        {
            foreach (var attributeDefinition in GetAttributes(attributes))
            {
                codeBuilder.Lines.Add(new CodeLine("{0}{1}", codeBuilder.Indent(start), attributeDefinition));
            }
        }

        public static void AddAttributes(this CSharpClassBuilder classBuilder, int start)
        {
            AddAttributes(classBuilder, classBuilder.ObjectDefinition.Attributes, start);
        }

        public static void AddAttributes(this CSharpInterfaceBuilder interfaceBuilder, int start)
        {
            AddAttributes(interfaceBuilder, interfaceBuilder.ObjectDefinition.Attributes, start);
        }

        public static void AddAttributes(this CSharpEnumBuilder interfaceBuilder, int start)
        {
            AddAttributes(interfaceBuilder, interfaceBuilder.ObjectDefinition.Attributes, start);
        }

        public static void AddAttributes(this DotNetCodeBuilder codeBuilder, PropertyDefinition propertyDefinition, int start)
        {
            AddAttributes(codeBuilder, propertyDefinition.Attributes, start + 1);
        }

        public static void AddAttributes(this DotNetCodeBuilder codeBuilder, MethodDefinition methodDefinition, int start)
        {
            AddAttributes(codeBuilder, methodDefinition.Attributes, start + 1);
        }

        public static string AddAttributes(this DotNetCodeBuilder codeBuilder, ParameterDefinition parameterDefinition)
            => string.Join("", GetAttributes(parameterDefinition.Attributes));
    }
}
