using System;
using CatFactory.CodeFactory;
using CatFactory.NetCore.ObjectOrientedProgramming;
using CatFactory.ObjectOrientedProgramming;
using Microsoft.Extensions.Logging;

namespace CatFactory.NetCore.CodeFactory
{
    public class CSharpCodeBuilder : DotNetCodeBuilder
    {
        public static void CreateFiles(string outputDirectory, bool forceOverwrite, params IDotNetObjectDefinition[] definitions)
        {
            foreach (var definition in definitions)
            {
                if (definition is CSharpClassDefinition)
                {
                    var codeBuilder = new CSharpClassBuilder
                    {
                        OutputDirectory = outputDirectory,
                        ForceOverwrite = forceOverwrite,
                        ObjectDefinition = definition
                    };
                }
                else if (definition is CSharpRecordDefinition)
                {
                    var codeBuilder = new CSharpRecordBuilder
                    {
                        OutputDirectory = outputDirectory,
                        ForceOverwrite = forceOverwrite,
                        ObjectDefinition = definition
                    };
                }
                else if (definition is CSharpInterfaceDefinition)
                {
                    var codeBuilder = new CSharpInterfaceBuilder
                    {
                        OutputDirectory = outputDirectory,
                        ForceOverwrite = forceOverwrite,
                        ObjectDefinition = definition
                    };
                }
                else if (definition is CSharpEnumDefinition)
                {
                    var codeBuilder = new CSharpEnumBuilder
                    {
                        OutputDirectory = outputDirectory,
                        ForceOverwrite = forceOverwrite,
                        ObjectDefinition = definition
                    };
                }
                else
                    throw new NotImplementedException();
            }
        }

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
                        ObjectDefinition = definition
                    };

                    codeBuilder.CreateFile(subdirectory);
                }
                else if (definition is CSharpRecordDefinition)
                {
                    var codeBuilder = new CSharpRecordBuilder
                    {
                        OutputDirectory = outputDirectory,
                        ForceOverwrite = forceOverwrite,
                        ObjectDefinition = definition
                    };

                    codeBuilder.CreateFile(subdirectory);
                }
                else if (definition is CSharpInterfaceDefinition)
                {
                    var codeBuilder = new CSharpInterfaceBuilder
                    {
                        OutputDirectory = outputDirectory,
                        ForceOverwrite = forceOverwrite,
                        ObjectDefinition = definition
                    };

                    codeBuilder.CreateFile(subdirectory);
                }
                else if (definition is CSharpEnumDefinition)
                {
                    var codeBuilder = new CSharpEnumBuilder
                    {
                        OutputDirectory = outputDirectory,
                        ForceOverwrite = forceOverwrite,
                        ObjectDefinition = definition
                    };

                    codeBuilder.CreateFile(subdirectory);
                }
                else
                    throw new NotImplementedException();
            }
        }

        public CSharpCodeBuilder()
            : base()
        {
            Init();
        }

        public CSharpCodeBuilder(ILogger<CSharpCodeBuilder> logger)
            : base(logger)
        {
            Init();
        }

        public bool AddNamespacesAtStart { get; set; }

        protected virtual void Init()
        {
            AddNamespacesAtStart = true;
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

            if (definition.Documentation.HasSummary)
            {
                Lines.Add(new CodeLine("{0}/// <summary>", Indent(start)));
                Lines.Add(new CodeLine("{0}/// {1}", Indent(start), definition.Documentation.Summary));
                Lines.Add(new CodeLine("{0}/// </summary>", Indent(start)));
            }

            if (definition.Documentation.HasRemarks)
                Lines.Add(new CodeLine("{0}/// <remarks>{1}</remarks>", Indent(start), definition.Documentation.Remarks));
        }

        protected override void AddDocumentation(int start, ClassConstructorDefinition definition)
        {
            if (definition.Documentation == null)
                return;

            if (definition.Documentation.HasSummary)
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

            if (definition.Documentation.HasRemarks)
                Lines.Add(new CodeLine("{0}/// <remarks>{1}</remarks>", Indent(start), definition.Documentation.Remarks));
        }

        protected override void AddDocumentation(int start, PropertyDefinition definition)
        {
            if (definition.Documentation == null)
                return;

            if (definition.Documentation.HasSummary)
            {
                Lines.Add(new CodeLine("{0}/// <summary>", Indent(start)));
                Lines.Add(new CodeLine("{0}/// {1}", Indent(start), definition.Documentation.Summary));
                Lines.Add(new CodeLine("{0}/// </summary>", Indent(start)));
            }

            if (definition.Documentation.HasRemarks)
                Lines.Add(new CodeLine("{0}/// <remarks>{1}</remarks>", Indent(start), definition.Documentation.Remarks));
        }

        protected override void AddDocumentation(int start, MethodDefinition definition)
        {
            if (definition.Documentation == null)
                return;

            if (definition.Documentation.HasSummary)
            {
                Lines.Add(new CodeLine("{0}/// <summary>", Indent(start)));
                Lines.Add(new CodeLine("{0}/// {1}", Indent(start), definition.Documentation.Summary));
                Lines.Add(new CodeLine("{0}/// </summary>", Indent(start)));
            }

            if (definition.Documentation.HasReturns)
                Lines.Add(new CodeLine("{0}/// <returns>{1}</returns>", Indent(start), definition.Documentation.Returns));

            foreach (var parameter in definition.Parameters)
            {
                if (parameter.Documentation == null)
                    continue;

                if (parameter.Documentation.HasSummary)
                    Lines.Add(new CodeLine("{0}/// <param name=\"{1}\">{2}</param>", Indent(start), definition.Name, definition.Documentation.Summary));
            }

            if (definition.Documentation.HasRemarks)
                Lines.Add(new CodeLine("{0}/// <remarks>{1}</remarks>", Indent(start), definition.Documentation.Remarks));
        }

        public override string FileExtension
            => "cs";

        protected override string GetComment(string description)
            => string.Format("//{0}", description);

        protected override string GetPreprocessorDirective(string name)
            => string.Format("#{0}", name);

        protected override string GetReturn(string content)
            => string.Format("return {0}", content);

        protected override string GetTodo(string description)
            => string.Format("// todo: {0}", description);
    }
}
