using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatFactory.DotNetCore
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

        public IDotNetInterfaceDefinition ObjectDefinition { get; set; } = new CSharpInterfaceDefinition();

        public override string FileName
            => ObjectDefinition.Name;

        protected virtual void AddEvents(int start, StringBuilder output)
        {
            if (ObjectDefinition.Events == null || ObjectDefinition.Events.Count == 0)
            {
                return;
            }

            if (ObjectDefinition.UseRegionsToGroupClassMembers)
            {
                output.AppendLine();

                output.AppendFormat("{0}#region {1}", Indent(start + 1), EventsRegionDescription);
                output.AppendLine();

                output.AppendLine();
            }

            if (ObjectDefinition.Events.Count > 0)
            {
                foreach (var @event in ObjectDefinition.Events)
                {
                    output.AppendFormat("{0}event {1} {2};", Indent(start + 1), @event.Type, @event.Name);
                    output.AppendLine();
                }

                output.AppendLine();
            }

            if (ObjectDefinition.UseRegionsToGroupClassMembers)
            {
                output.AppendLine();

                output.AppendFormat("{0}#endregion", Indent(start + 1));
                output.AppendLine();

                output.AppendLine();
            }
        }

        protected virtual void AddProperties(int start, StringBuilder output)
        {
            if (ObjectDefinition.Properties == null || ObjectDefinition.Properties.Count == 0)
            {
                return;
            }

            if (ObjectDefinition.UseRegionsToGroupClassMembers)
            {
                output.AppendFormat("{0}#region {1}", Indent(start + 1), PropertiesRegionDescription);
                output.AppendLine();

                output.AppendLine();
            }

            for (var i = 0; i < ObjectDefinition.Properties.Count; i++)
            {
                var property = ObjectDefinition.Properties[i];

                if (property.Attributes.Count > 0)
                {
                    this.AddAttributes(property, output, start);
                }

                if (property.IsReadOnly)
                {
                    output.AppendFormat("{0}{1} {2} {{ get; }}", Indent(start + 1), property.Type, property.Name);
                    output.AppendLine();
                }
                else
                {
                    output.AppendFormat("{0}{1} {2} {{ get; set; }}", Indent(start + 1), property.Type, property.Name);
                    output.AppendLine();
                }

                if (i < ObjectDefinition.Properties.Count - 1)
                {
                    output.AppendLine();
                }
            }

            if (ObjectDefinition.UseRegionsToGroupClassMembers)
            {
                output.AppendFormat("{0}#endregion", Indent(start + 1));
                output.AppendLine();

                output.AppendLine();
            }
        }

        protected virtual void AddMethods(int start, StringBuilder output)
        {
            if (ObjectDefinition.Methods == null || ObjectDefinition.Methods.Count == 0)
            {
                return;
            }

            if (ObjectDefinition.UseRegionsToGroupClassMembers)
            {
                output.AppendFormat("{0}#region {1}", Indent(2), MethodsRegionDescription);
                output.AppendLine();

                output.AppendLine();
            }

            if (ObjectDefinition.Properties != null && ObjectDefinition.Properties.Count > 0)
            {
                output.AppendLine();
            }

            for (var i = 0; i < ObjectDefinition.Methods.Count; i++)
            {
                var method = ObjectDefinition.Methods[i];

                this.AddAttributes(method, output, start);

                var methodSignature = new List<string>();

                methodSignature.Add(string.IsNullOrEmpty(method.Type) ? "void" : method.Type);
                
                var parameters = new List<string>();

                for (var j = 0; j < method.Parameters.Count; j++)
                {
                    var parameter = method.Parameters[j];

                    var parametersAttributes = this.AddAttributes(parameter);

                    var parameterDef = string.Empty;

                    if (string.IsNullOrEmpty(parameter.DefaultValue))
                    {
                        if (string.IsNullOrEmpty(parametersAttributes))
                        {
                            parameterDef = string.Format("{0} {1}", parameter.Type, parameter.Name);
                        }
                        else
                        {
                            parameterDef = string.Format("{0}{1} {2}", parametersAttributes, parameter.Type, parameter.Name);
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(parametersAttributes))
                        {
                            parameterDef = string.Format("{0} {1} = {2}", parameter.Type, parameter.Name, parameter.DefaultValue);
                        }
                        else
                        {
                            parameterDef = string.Format("{0}{1} {2} = {3}", parametersAttributes, parameter.Type, parameter.Name, parameter.DefaultValue);
                        }
                    }

                    parameters.Add(method.IsExtension && j == 0 ? string.Format("this {0}", parameterDef) : parameterDef);
                }

                if (method.GenericTypes.Count == 0)
                {
                    methodSignature.Add(string.Format("{0}({1})", method.Name, string.Join(", ", parameters)));
                }
                else
                {
                    methodSignature.Add(string.Format("{0}<{1}>({2})", method.Name, string.Join(", ", method.GenericTypes.Select(item => item.Name)), string.Join(", ", parameters)));
                }

                if (method.GenericTypes.Count > 0)
                {
                    methodSignature.Add(string.Join(", ", method.GenericTypes.Where(item => !string.IsNullOrEmpty(item.Constraint)).Select(item => string.Format("where {0}", item.Constraint))));
                }

                output.AppendFormat("{0}{1}", Indent(start + 1), string.Join(" ", methodSignature));
                output.Append(";");
                output.AppendLine();

                if (i < ObjectDefinition.Methods.Count - 1)
                {
                    output.AppendLine();
                }
            }

            if (ObjectDefinition.UseRegionsToGroupClassMembers)
            {
                output.AppendFormat("{0}#endregion", Indent(2));
                output.AppendLine();

                output.AppendLine();
            }
        }

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

                if (ObjectDefinition.Documentation != null)
                {
                    AddDocumentation(output, start, ObjectDefinition.Documentation);
                }

                this.AddAttributes(output, start);

                var declaration = new List<string>();

                declaration.Add(ObjectDefinition.AccessModifier.ToString().ToLower());

                if (ObjectDefinition.IsPartial)
                {
                    declaration.Add("partial");
                }

                declaration.Add("interface");

                if (ObjectDefinition.GenericTypes.Count == 0)
                {
                    declaration.Add(ObjectDefinition.Name);
                }
                else
                {
                    declaration.Add(string.Format("{0}<{1}>", ObjectDefinition.Name, string.Join(", ", ObjectDefinition.GenericTypes.Select(item => item.Name))));
                }

                if (ObjectDefinition.HasInheritance)
                {
                    declaration.Add(":");

                    var parents = new List<string>();

                    if (ObjectDefinition.Implements.Count > 0)
                    {
                        parents.AddRange(ObjectDefinition.Implements);
                    }

                    declaration.Add(string.Join(", ", parents));
                }

                if (ObjectDefinition.GenericTypes.Count > 0)
                {
                    declaration.Add(string.Join(", ", ObjectDefinition.GenericTypes.Where(item => !string.IsNullOrEmpty(item.Constraint)).Select(item => string.Format("where {0}", item.Constraint))));
                }

                output.AppendFormat("{0}{1}", Indent(start), string.Join(" ", declaration));

                output.AppendLine();

                output.AppendFormat("{0}{1}", Indent(start), "{");
                output.AppendLine();

                AddEvents(start, output);

                AddProperties(start, output);

                AddMethods(start, output);

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
