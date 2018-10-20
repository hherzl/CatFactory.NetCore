using System.Collections.Generic;
using System.Text;
using CatFactory.CodeFactory;

namespace CatFactory.NetCore.CodeFactory
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

        public override void Translating()
        {
            var output = new StringBuilder();

            if (ObjectDefinition.Namespaces.Count > 0)
            {
                foreach (var item in ObjectDefinition.Namespaces)
                {
                    Lines.Add(new CodeLine("using {0};", item));
                }

                Lines.Add(new CodeLine());
            }

            var start = 0;

            if (!string.IsNullOrEmpty(ObjectDefinition.Namespace))
            {
                start = 1;

                Lines.Add(new CodeLine("namespace {0}", ObjectDefinition.Namespace));

                Lines.Add(new CodeLine("{0}", "{"));
            }

            AddDocumentation(start, ObjectDefinition);

            this.AddAttributes(start);

            var declaration = new List<string>
            {
                ObjectDefinition.AccessModifier.ToString().ToLower()
            };

            declaration.Add("enum");

            declaration.Add(ObjectDefinition.Name);

            if (!string.IsNullOrEmpty(ObjectDefinition.BaseType))
            {
                declaration.Add(":");
                declaration.Add(ObjectDefinition.BaseType);
            }

            Lines.Add(new CodeLine("{0}{1}", Indent(start), string.Join(" ", declaration)));

            Lines.Add(new CodeLine("{0}{1}", Indent(start), "{"));

            for (var i = 0; i < ObjectDefinition.Sets.Count; i++)
            {
                var set = ObjectDefinition.Sets[i];

                // todo: Add attributes for options

                Lines.Add(new CodeLine("{0}{1}", Indent(start + 1), string.Format("{0} = {1}{2}", set.Name, set.Value, i < ObjectDefinition.Sets.Count - 1 ? "," : "")));
            }

            Lines.Add(new CodeLine("{0}{1}", Indent(start), "}"));

            if (!string.IsNullOrEmpty(ObjectDefinition.Namespace))
                Lines.Add(new CodeLine("}"));
        }
    }
}
