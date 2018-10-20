using CatFactory.CodeFactory;
using CatFactory.OOP;

namespace CatFactory.NetCore.CodeFactory
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

        protected override void AddDocumentation(int start, IObjectDefinition definition)
        {
            if (definition.Documentation == null)
                return;

            if (!string.IsNullOrEmpty(definition.Documentation.Summary))
            {
                Lines.Add(new CodeLine("{0}/// <summary>", Indent(start)));
                Lines.Add(new CodeLine("{0}/// {1}", Indent(start), definition.Documentation.Summary));
                Lines.Add(new CodeLine("{0}/// </summary>", Indent(start)));
            }

            if (!string.IsNullOrEmpty(definition.Documentation.Remarks))
                Lines.Add(new CodeLine("{0}/// <remarks>{1}</remarks>", Indent(start), definition.Documentation.Remarks));
        }

        protected override void AddDocumentation(int start, ClassConstructorDefinition definition)
        {
            if (definition.Documentation == null)
                return;

            if (!string.IsNullOrEmpty(definition.Documentation.Summary))
            {
                Lines.Add(new CodeLine("{0}/// <summary>", Indent(start)));
                Lines.Add(new CodeLine("{0}/// {1}", Indent(start), definition.Documentation.Summary));
                Lines.Add(new CodeLine("{0}/// </summary>", Indent(start)));
            }

            foreach (var parameter in definition.Parameters)
            {
                if (parameter.Documentation == null)
                    continue;

                if (!string.IsNullOrEmpty(parameter.Documentation.Summary))
                    Lines.Add(new CodeLine("{0}/// <param name=\"{1}\">{2}</param>", Indent(start), parameter.Name, parameter.Documentation.Summary));
            }

            if (!string.IsNullOrEmpty(definition.Documentation.Remarks))
                Lines.Add(new CodeLine("{0}/// <remarks>{1}</remarks>", Indent(start), definition.Documentation.Remarks));
        }

        protected override void AddDocumentation(int start, PropertyDefinition definition)
        {
            if (definition.Documentation == null)
                return;

            if (!string.IsNullOrEmpty(definition.Documentation.Summary))
            {
                Lines.Add(new CodeLine("{0}/// <summary>", Indent(start)));
                Lines.Add(new CodeLine("{0}/// {1}", Indent(start), definition.Documentation.Summary));
                Lines.Add(new CodeLine("{0}/// </summary>", Indent(start)));
            }

            if (!string.IsNullOrEmpty(definition.Documentation.Remarks))
                Lines.Add(new CodeLine("{0}/// <remarks>{1}</remarks>", Indent(start), definition.Documentation.Remarks));
        }

        protected override void AddDocumentation(int start, MethodDefinition definition)
        {
            if (definition.Documentation == null)
                return;

            if (!string.IsNullOrEmpty(definition.Documentation.Summary))
            {
                Lines.Add(new CodeLine("{0}/// <summary>", Indent(start)));
                Lines.Add(new CodeLine("{0}/// {1}", Indent(start), definition.Documentation.Summary));
                Lines.Add(new CodeLine("{0}/// </summary>", Indent(start)));
            }

            // todo: Add returns tag

            foreach (var parameter in definition.Parameters)
            {
                if (parameter.Documentation == null)
                    continue;

                if (!string.IsNullOrEmpty(parameter.Documentation.Summary))
                    Lines.Add(new CodeLine("{0}/// <param name=\"{1}\">{2}</param>", Indent(start), definition.Name, definition.Documentation.Summary));
            }

            if (!string.IsNullOrEmpty(definition.Documentation.Remarks))
                Lines.Add(new CodeLine("{0}/// <remarks>{1}</remarks>", Indent(start), definition.Documentation.Remarks));
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
