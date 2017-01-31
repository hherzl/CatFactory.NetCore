using System;
using System.Linq;
using System.Text;

namespace CatFactory.DotNetCore
{
    public static class ObjectBuilderExtensions
    {
        public static void AddAttributes(this CSharpClassBuilder classBuilder, StringBuilder output, Int32 start, IDotNetClassDefinition ObjectDefinition)
        {
            foreach (var attrib in ObjectDefinition.Attributes)
            {
                var attributeDefinition = new StringBuilder();

                attributeDefinition.Append("[");

                attributeDefinition.AppendFormat("{0}", attrib.Name);

                if (attrib.HasMembers)
                {
                    attributeDefinition.Append("(");

                    if (attrib.HasArguments)
                    {
                        attributeDefinition.Append(String.Join(", ", attrib.Arguments));
                    }

                    if (attrib.HasSets)
                    {
                        attributeDefinition.AppendFormat(", {0}", String.Join(", ", attrib.Sets.Select(item => String.Format("{0} = {1}", item.Name, item.Value))));
                    }

                    attributeDefinition.Append(")");
                }

                attributeDefinition.Append("]");

                output.AppendFormat("{0}{1}", classBuilder.Indent(1), attributeDefinition.ToString());
                output.AppendLine();
            }
        }
    }
}
