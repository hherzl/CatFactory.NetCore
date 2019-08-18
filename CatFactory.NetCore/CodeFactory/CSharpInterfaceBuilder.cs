using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CatFactory.CodeFactory;
using CatFactory.NetCore.ObjectOrientedProgramming;
using Microsoft.Extensions.Logging;

namespace CatFactory.NetCore.CodeFactory
{
    public class CSharpInterfaceBuilder : CSharpCodeBuilder
    {
        public static void CreateFiles(string outputDirectory, string subdirectory, bool forceOverwrite, params CSharpInterfaceDefinition[] definitions)
        {
            foreach (var definition in definitions)
            {
                var codeBuilder = new CSharpInterfaceBuilder
                {
                    OutputDirectory = outputDirectory,
                    ForceOverwrite = forceOverwrite,
                    ObjectDefinition = definition
                };

                codeBuilder.CreateFile(subdirectory);
            }
        }

        public CSharpInterfaceBuilder()
            : base()
        {
        }

        public CSharpInterfaceBuilder(ILogger<CSharpInterfaceBuilder> logger)
            : base(logger)
        {
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IDotNetInterfaceDefinition m_interfaceDefinition;

        public IDotNetInterfaceDefinition InterfaceDefinition
            => m_interfaceDefinition ?? (m_interfaceDefinition = ObjectDefinition as IDotNetInterfaceDefinition);

        public override string FileName
            => InterfaceDefinition.Name;

        protected virtual void AddEvents(int start)
        {
            if (InterfaceDefinition.Events == null || InterfaceDefinition.Events.Count == 0)
                return;

            if (InterfaceDefinition.UseRegionsToGroupClassMembers)
            {
                Lines.Add(new EmptyLine());
                Lines.Add(new CodeLine("{0}#region {1}", Indent(start + 1), EventsRegionDescription));
                Lines.Add(new EmptyLine());
            }

            if (InterfaceDefinition.Events.Count > 0)
            {
                foreach (var @event in InterfaceDefinition.Events)
                {
                    Lines.Add(new CodeLine("{0}event {1} {2};", Indent(start + 1), @event.Type, @event.Name));
                }

                Lines.Add(new EmptyLine());
            }

            if (InterfaceDefinition.UseRegionsToGroupClassMembers)
            {
                Lines.Add(new EmptyLine());
                Lines.Add(new CodeLine("{0}#endregion", Indent(start + 1)));
                Lines.Add(new EmptyLine());
            }
        }

        protected virtual void AddProperties(int start)
        {
            if (InterfaceDefinition.Properties == null || InterfaceDefinition.Properties.Count == 0)
                return;

            if (InterfaceDefinition.UseRegionsToGroupClassMembers)
            {
                Lines.Add(new CodeLine("{0}#region {1}", Indent(start + 1), PropertiesRegionDescription));

                Lines.Add(new EmptyLine());
            }

            for (var i = 0; i < InterfaceDefinition.Properties.Count; i++)
            {
                var property = InterfaceDefinition.Properties[i];

                if (property.Attributes.Count > 0)
                    this.AddAttributes(property, start);

                if (property.IsReadOnly)
                    Lines.Add(new CodeLine("{0}{1} {2} {{ get; }}", Indent(start + 1), property.Type, property.Name));
                else
                    Lines.Add(new CodeLine("{0}{1} {2} {{ get; set; }}", Indent(start + 1), property.Type, property.Name));

                if (i < InterfaceDefinition.Properties.Count - 1)
                {
                    Lines.Add(new EmptyLine());
                }
            }

            if (InterfaceDefinition.UseRegionsToGroupClassMembers)
            {
                Lines.Add(new CodeLine("{0}#endregion", Indent(start + 1)));
                Lines.Add(new EmptyLine());
            }
        }

        protected virtual void AddMethods(int start)
        {
            if (InterfaceDefinition.Methods == null || InterfaceDefinition.Methods.Count == 0)
                return;

            if (InterfaceDefinition.UseRegionsToGroupClassMembers)
            {
                Lines.Add(new CodeLine("{0}#region {1}", Indent(2), MethodsRegionDescription));
                Lines.Add(new EmptyLine());
            }

            if (InterfaceDefinition.Properties != null && InterfaceDefinition.Properties.Count > 0)
                Lines.Add(new EmptyLine());

            for (var i = 0; i < InterfaceDefinition.Methods.Count; i++)
            {
                var method = InterfaceDefinition.Methods[i];

                AddDocumentation(start + 1, method);

                this.AddAttributes(method, start);

                var methodSignature = new List<string>
                {
                    string.IsNullOrEmpty(method.Type) ? "void" : method.Type
                };

                var parameters = new List<string>();

                for (var j = 0; j < method.Parameters.Count; j++)
                {
                    var parameter = method.Parameters[j];

                    var parametersAttributes = this.AddAttributes(parameter);

                    var parameterDef = string.Empty;

                    if (string.IsNullOrEmpty(parameter.DefaultValue))
                    {
                        if (string.IsNullOrEmpty(parametersAttributes))
                            parameterDef = string.Format("{0} {1}", parameter.Type, parameter.Name);
                        else
                            parameterDef = string.Format("{0}{1} {2}", parametersAttributes, parameter.Type, parameter.Name);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(parametersAttributes))
                            parameterDef = string.Format("{0} {1} = {2}", parameter.Type, parameter.Name, parameter.DefaultValue);
                        else
                            parameterDef = string.Format("{0}{1} {2} = {3}", parametersAttributes, parameter.Type, parameter.Name, parameter.DefaultValue);
                    }

                    parameters.Add(method.IsExtension && j == 0 ? string.Format("this {0}", parameterDef) : parameterDef);
                }

                if (method.GenericTypes.Count == 0)
                    methodSignature.Add(string.Format("{0}({1})", method.Name, string.Join(", ", parameters)));
                else
                    methodSignature.Add(string.Format("{0}<{1}>({2})", method.Name, string.Join(", ", method.GenericTypes.Select(item => item.Name)), string.Join(", ", parameters)));

                if (method.GenericTypes.Count > 0)
                    methodSignature.Add(string.Join(", ", method.GenericTypes.Where(item => !string.IsNullOrEmpty(item.Constraint)).Select(item => string.Format("where {0}", item.Constraint))));

                Lines.Add(new CodeLine("{0}{1};", Indent(start + 1), string.Join(" ", methodSignature)));

                if (i < InterfaceDefinition.Methods.Count - 1)
                    Lines.Add(new CodeLine());
            }

            if (InterfaceDefinition.UseRegionsToGroupClassMembers)
            {
                Lines.Add(new CodeLine("{0}#endregion", Indent(2)));
                Lines.Add(new EmptyLine());
            }
        }

        public override void Translating()
        {
            Lines = new List<ILine>();

            if (AddNamespacesAtStart && InterfaceDefinition.Namespaces.Count > 0)
            {
                foreach (var item in InterfaceDefinition.Namespaces)
                {
                    Lines.Add(new CodeLine("using {0};", item));
                }

                Lines.Add(new EmptyLine());
            }

            var start = 0;

            if (!string.IsNullOrEmpty(InterfaceDefinition.Namespace))
            {
                start = 1;

                Lines.Add(new CodeLine("namespace {0}", InterfaceDefinition.Namespace));

                Lines.Add(new CodeLine("{"));

                if (!AddNamespacesAtStart)
                {
                    foreach (var item in ObjectDefinition.Namespaces)
                    {
                        Lines.Add(new CodeLine("{0}using {1};", Indent(1), item));
                    }

                    Lines.Add(new EmptyLine());
                }
            }

            AddDocumentation(start, InterfaceDefinition);

            this.AddAttributes(start);

            var declaration = new List<string>
            {
                InterfaceDefinition.AccessModifier.ToString().ToLower()
            };

            if (InterfaceDefinition.IsPartial)
                declaration.Add("partial");

            declaration.Add("interface");

            if (InterfaceDefinition.GenericTypes.Count == 0)
                declaration.Add(InterfaceDefinition.Name);
            else
                declaration.Add(string.Format("{0}<{1}>", InterfaceDefinition.Name, string.Join(", ", InterfaceDefinition.GenericTypes.Select(item => item.Name))));

            if (InterfaceDefinition.HasInheritance)
            {
                declaration.Add(":");

                var parents = new List<string>();

                if (InterfaceDefinition.Implements.Count > 0)
                    parents.AddRange(InterfaceDefinition.Implements);

                declaration.Add(string.Join(", ", parents));
            }

            if (InterfaceDefinition.GenericTypes.Count > 0)
                declaration.Add(string.Join(", ", InterfaceDefinition.GenericTypes.Where(item => !string.IsNullOrEmpty(item.Constraint)).Select(item => string.Format("where {0}", item.Constraint))));

            Lines.Add(new CodeLine("{0}{1}", Indent(start), string.Join(" ", declaration)));

            Lines.Add(new CodeLine("{0}{1}", Indent(start), "{"));

            AddEvents(start);

            AddProperties(start);

            AddMethods(start);

            Lines.Add(new CodeLine("{0}{1}", Indent(start), "}"));

            if (!string.IsNullOrEmpty(InterfaceDefinition.Namespace))
                Lines.Add(new CodeLine("}"));
        }
    }
}
