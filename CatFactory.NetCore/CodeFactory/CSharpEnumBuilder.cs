using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using CatFactory.CodeFactory;
using CatFactory.NetCore.ObjectOrientedProgramming;
using Microsoft.Extensions.Logging;

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
            : base()
        {
        }

        public CSharpEnumBuilder(ILogger<CSharpEnumBuilder> logger)
            : base(logger)
        {
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private CSharpEnumDefinition m_enumDefinition;

        public CSharpEnumDefinition EnumDefinition
            => m_enumDefinition ?? (m_enumDefinition = ObjectDefinition as CSharpEnumDefinition);

        public override string FileName
            => EnumDefinition.Name;

        public override void Translating()
        {
            var output = new StringBuilder();

            if (EnumDefinition.Namespaces.Count > 0)
            {
                foreach (var item in EnumDefinition.Namespaces)
                {
                    Lines.Add(new CodeLine("using {0};", item));
                }

                Lines.Add(new CodeLine());
            }

            var start = 0;

            if (!string.IsNullOrEmpty(EnumDefinition.Namespace))
            {
                start = 1;

                Lines.Add(new CodeLine("namespace {0}", EnumDefinition.Namespace));

                Lines.Add(new CodeLine("{0}", "{"));
            }

            AddDocumentation(start, EnumDefinition);

            this.AddAttributes(start);

            var declaration = new List<string>
            {
                EnumDefinition.AccessModifier.ToString().ToLower()
            };

            declaration.Add("enum");

            declaration.Add(EnumDefinition.Name);

            if (!string.IsNullOrEmpty(EnumDefinition.BaseType))
            {
                declaration.Add(":");
                declaration.Add(EnumDefinition.BaseType);
            }

            Lines.Add(new CodeLine("{0}{1}", Indent(start), string.Join(" ", declaration)));

            Lines.Add(new CodeLine("{0}{1}", Indent(start), "{"));

            for (var i = 0; i < EnumDefinition.Sets.Count; i++)
            {
                var set = EnumDefinition.Sets[i];

                // todo: Add attributes for options

                Lines.Add(new CodeLine("{0}{1}", Indent(start + 1), string.Format("{0} = {1}{2}", set.Name, set.Value, i < EnumDefinition.Sets.Count - 1 ? "," : "")));
            }

            Lines.Add(new CodeLine("{0}{1}", Indent(start), "}"));

            if (!string.IsNullOrEmpty(EnumDefinition.Namespace))
                Lines.Add(new CodeLine("}"));
        }
    }
}
