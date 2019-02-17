using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CatFactory.CodeFactory;
using CatFactory.NetCore.ObjectOrientedProgramming;

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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IDotNetInterfaceDefinition m_objectDefinition;

        public new IDotNetInterfaceDefinition ObjectDefinition
        {
            get
            {
                return m_objectDefinition ?? (m_objectDefinition = new CSharpInterfaceDefinition());
            }
            set
            {
                m_objectDefinition = value;
            }
        }

        public override string FileName
            => ObjectDefinition.Name;

        protected virtual void AddEvents(int start)
        {
            if (ObjectDefinition.Events == null || ObjectDefinition.Events.Count == 0)
                return;

            if (ObjectDefinition.UseRegionsToGroupClassMembers)
            {
                Lines.Add(new CodeLine());
                Lines.Add(new CodeLine("{0}#region {1}", Indent(start + 1), EventsRegionDescription));
                Lines.Add(new CodeLine());
            }

            if (ObjectDefinition.Events.Count > 0)
            {
                foreach (var @event in ObjectDefinition.Events)
                {
                    Lines.Add(new CodeLine("{0}event {1} {2};", Indent(start + 1), @event.Type, @event.Name));
                }

                Lines.Add(new CodeLine());
            }

            if (ObjectDefinition.UseRegionsToGroupClassMembers)
            {
                Lines.Add(new CodeLine());
                Lines.Add(new CodeLine("{0}#endregion", Indent(start + 1)));
                Lines.Add(new CodeLine());
            }
        }

        protected virtual void AddProperties(int start)
        {
            if (ObjectDefinition.Properties == null || ObjectDefinition.Properties.Count == 0)
                return;

            if (ObjectDefinition.UseRegionsToGroupClassMembers)
            {
                Lines.Add(new CodeLine("{0}#region {1}", Indent(start + 1), PropertiesRegionDescription));

                Lines.Add(new CodeLine());
            }

            for (var i = 0; i < ObjectDefinition.Properties.Count; i++)
            {
                var property = ObjectDefinition.Properties[i];

                if (property.Attributes.Count > 0)
                    this.AddAttributes(property, start);

                if (property.IsReadOnly)
                    Lines.Add(new CodeLine("{0}{1} {2} {{ get; }}", Indent(start + 1), property.Type, property.Name));
                else
                    Lines.Add(new CodeLine("{0}{1} {2} {{ get; set; }}", Indent(start + 1), property.Type, property.Name));

                if (i < ObjectDefinition.Properties.Count - 1)
                {
                    Lines.Add(new CodeLine());
                }
            }

            if (ObjectDefinition.UseRegionsToGroupClassMembers)
            {
                Lines.Add(new CodeLine("{0}#endregion", Indent(start + 1)));
                Lines.Add(new CodeLine());
            }
        }

        protected virtual void AddMethods(int start)
        {
            if (ObjectDefinition.Methods == null || ObjectDefinition.Methods.Count == 0)
                return;

            if (ObjectDefinition.UseRegionsToGroupClassMembers)
            {
                Lines.Add(new CodeLine("{0}#region {1}", Indent(2), MethodsRegionDescription));
                Lines.Add(new CodeLine());
            }

            if (ObjectDefinition.Properties != null && ObjectDefinition.Properties.Count > 0)
                Lines.Add(new CodeLine());

            for (var i = 0; i < ObjectDefinition.Methods.Count; i++)
            {
                var method = ObjectDefinition.Methods[i];

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

                if (i < ObjectDefinition.Methods.Count - 1)
                    Lines.Add(new CodeLine());
            }

            if (ObjectDefinition.UseRegionsToGroupClassMembers)
            {
                Lines.Add(new CodeLine("{0}#endregion", Indent(2)));
                Lines.Add(new CodeLine());
            }
        }

        public override void Translating()
        {
            Lines = new List<ILine>();

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

                Lines.Add(new CodeLine("{"));
            }

            AddDocumentation(start, ObjectDefinition);

            this.AddAttributes(start);

            var declaration = new List<string>
            {
                ObjectDefinition.AccessModifier.ToString().ToLower()
            };

            if (ObjectDefinition.IsPartial)
                declaration.Add("partial");

            declaration.Add("interface");

            if (ObjectDefinition.GenericTypes.Count == 0)
                declaration.Add(ObjectDefinition.Name);
            else
                declaration.Add(string.Format("{0}<{1}>", ObjectDefinition.Name, string.Join(", ", ObjectDefinition.GenericTypes.Select(item => item.Name))));

            if (ObjectDefinition.HasInheritance)
            {
                declaration.Add(":");

                var parents = new List<string>();

                if (ObjectDefinition.Implements.Count > 0)
                    parents.AddRange(ObjectDefinition.Implements);

                declaration.Add(string.Join(", ", parents));
            }

            if (ObjectDefinition.GenericTypes.Count > 0)
                declaration.Add(string.Join(", ", ObjectDefinition.GenericTypes.Where(item => !string.IsNullOrEmpty(item.Constraint)).Select(item => string.Format("where {0}", item.Constraint))));

            Lines.Add(new CodeLine("{0}{1}", Indent(start), string.Join(" ", declaration)));

            Lines.Add(new CodeLine("{0}{1}", Indent(start), "{"));

            AddEvents(start);

            AddProperties(start);

            AddMethods(start);

            Lines.Add(new CodeLine("{0}{1}", Indent(start), "}"));

            if (!string.IsNullOrEmpty(ObjectDefinition.Namespace))
                Lines.Add(new CodeLine("}"));
        }
    }
}
