using System.Text;
using CatFactory.OOP;

namespace CatFactory.DotNetCore
{
    public class CSharpCodeBuilder : DotNetCodeBuilder
    {
        public static void CreateFiles(string outputDirectory, string subdirectory, bool forceOverwrite, params IDotNetObjectDefinition[] definitions)
        {
            foreach (var definition in definitions)
            {
                if (definition is CSharpClassDefinition)
                {
                    var codeBuilder = new CSharpClassBuilder
                    {
                        OutputDirectory = outputDirectory,
                        ForceOverwrite = forceOverwrite,
                        ObjectDefinition = (CSharpClassDefinition)definition
                    };

                    codeBuilder.CreateFile(subdirectory);
                }
                else if (definition is CSharpInterfaceDefinition)
                {
                    var codeBuilder = new CSharpInterfaceBuilder
                    {
                        OutputDirectory = outputDirectory,
                        ForceOverwrite = forceOverwrite,
                        ObjectDefinition = (CSharpInterfaceDefinition)definition
                    };

                    codeBuilder.CreateFile(subdirectory);
                }
            }
        }

        public CSharpCodeBuilder()
        {
            ConstantsRegionDescription = "[ Constants ]";
            EventsRegionDescription = "[ Events ]";
            FieldsRegionDescription = "[ Fields ]";
            ConstructorsRegionDescription = "[ Constructor ]";
            FinalizerRegionDescription = "[ Finalizer ]";
            IndexersRegionDescription = "[ Indexers ]";
            PropertiesRegionDescription = "[ Properties ]";
            MethodsRegionDescription = "[ Methods ]";
        }

        protected override void AddDocumentation(StringBuilder output, int start, Documentation documentation)
        {
            if (!string.IsNullOrEmpty(documentation.Summary))
            {
                output.AppendFormat("{0}/// <summary>", Indent(start));
                output.AppendLine();

                output.AppendFormat("{0}/// {1}", Indent(start), documentation.Summary);
                output.AppendLine();

                output.AppendFormat("{0}/// </summary>", Indent(start));
                output.AppendLine();
            }

            if (!string.IsNullOrEmpty(documentation.Remarks))
            {
                output.AppendFormat("{0}/// <remarks>{1}</remarks>", Indent(start), documentation.Remarks);
                output.AppendLine();
            }
        }

        public override string FileExtension
            => "cs";

        protected override string GetComment(string description)
            => string.Format("//{0}", description);

        protected override string GetPreprocessorDirective(string name)
            => string.Format("#{0}", name);

        protected override string GetTodo(string description)
            => string.Format("// todo: {0}", description);
    }
}
