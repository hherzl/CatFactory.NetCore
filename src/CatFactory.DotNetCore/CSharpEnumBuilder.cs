using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatFactory.DotNetCore
{
    public class CSharpEnumBuilder : CSharpCodeBuilder
    {
        public static void CreateFiles(string outputDirectory, string subdirectory, bool forceOverwrite, params CSharpEnumDefinition[] definitions)
        {
            foreach (var definition in definitions)
            {
                var codeBuilder = new CSharpEnumBuilder
                {
                    OutputDirectory = outputDirectory,
                    ForceOverwrite = forceOverwrite,
                    ObjectDefinition = definition
                };

                codeBuilder.CreateFile(subdirectory);
            }
        }

        public CSharpEnumBuilder()
        {
        }

        public new CSharpEnumDefinition ObjectDefinition { get; set; }

        public override string FileName
            => ObjectDefinition.Name;

        public override string Code
        {
            get
            {
                var output = new StringBuilder();

                if (ObjectDefinition.Namespaces.Count > 0)
                {
                    foreach (var item in ObjectDefinition.Namespaces)
                    {
                        output.AppendFormat("using {0};", item);
                        output.AppendLine();
                    }

                    output.AppendLine();
                }

                var start = 0;

                if (!string.IsNullOrEmpty(ObjectDefinition.Namespace))
                {
                    start = 1;

                    output.AppendFormat("namespace {0}", ObjectDefinition.Namespace);
                    output.AppendLine();

                    output.AppendFormat("{0}", "{");
                    output.AppendLine();
                }

                this.AddAttributes(output, start);

                var declaration = new List<string>();

                declaration.Add(ObjectDefinition.AccessModifier.ToString().ToLower());

                declaration.Add("enum");

                declaration.Add(ObjectDefinition.Name);

                if (!string.IsNullOrEmpty(ObjectDefinition.BaseType))
                {
                    declaration.Add(":");
                    declaration.Add(ObjectDefinition.BaseType);
                }

                output.AppendFormat("{0}{1}", Indent(start), string.Join(" ", declaration));

                output.AppendLine();

                output.AppendFormat("{0}{1}", Indent(start), "{");
                output.AppendLine();

                output.AppendFormat("{0}{1}", Indent(start + 1), string.Join(", ", ObjectDefinition.Sets.Select(item => string.Format("{0} = {1}", item.Name, item.Value))));
                output.AppendLine();

                output.AppendFormat("{0}{1}", Indent(start), "}");
                output.AppendLine();

                if (!string.IsNullOrEmpty(ObjectDefinition.Namespace))
                {
                    output.Append("}");
                    output.AppendLine();
                }

                return output.ToString();
            }
        }
    }
}
