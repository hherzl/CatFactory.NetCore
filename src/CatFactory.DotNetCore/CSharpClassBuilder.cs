using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatFactory.DotNetCore
{
    public class CSharpClassBuilder : CSharpCodeBuilder
    {
        public static void CreateFiles(string outputDirectory, string subdirectory, bool forceOverwrite, params CSharpClassDefinition[] definitions)
        {
            foreach (var definition in definitions)
            {
                var codeBuilder = new CSharpClassBuilder
                {
                    OutputDirectory = outputDirectory,
                    ForceOverwrite = forceOverwrite,
                    ObjectDefinition = definition
                };

                codeBuilder.CreateFile(subdirectory);
            }
        }

        public CSharpClassBuilder()
            : base()
        {
        }

        public IDotNetClassDefinition ObjectDefinition { get; set; } = new CSharpClassDefinition();

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

                if (!string.IsNullOrEmpty(ObjectDefinition.Documentation.Summary))
                {
                    AddDocumentation(output, start, ObjectDefinition.Documentation);
                }

                this.AddAttributes(output, start);

                var declaration = new List<string>();

                declaration.Add(ObjectDefinition.AccessModifier.ToString().ToLower());

                if (ObjectDefinition.IsStatic)
                {
                    declaration.Add("static");
                }

                if (ObjectDefinition.IsPartial)
                {
                    declaration.Add("partial");
                }

                declaration.Add("class");

                declaration.Add(ObjectDefinition.Name);

                if (!string.IsNullOrEmpty(ObjectDefinition.GenericType))
                {
                    declaration.Add(string.Format("<{0}>", ObjectDefinition.GenericType));
                }

                if (ObjectDefinition.HasInheritance)
                {
                    declaration.Add(":");

                    var parents = new List<string>();

                    if (!string.IsNullOrEmpty(ObjectDefinition.BaseClass))
                    {
                        parents.Add(ObjectDefinition.BaseClass);
                    }

                    if (ObjectDefinition.Implements.Count > 0)
                    {
                        parents.AddRange(ObjectDefinition.Implements);
                    }

                    declaration.Add(string.Join(", ", parents));
                }

                output.AppendFormat("{0}{1}", Indent(start), string.Join(" ", declaration));

                output.AppendLine();

                output.AppendFormat("{0}{{", Indent(start));
                output.AppendLine();

                AddConstants(start, output);

                AddEvents(start, output);

                AddFields(start, output);

                AddStaticConstructor(start, output);

                AddConstructors(start, output);

                AddFinalizer(start, output);

                AddIndexers(start, output);

                AddProperties(start, output);

                AddMethods(start, output);

                output.AppendFormat("{0}}}", Indent(start));
                output.AppendLine();

                if (!string.IsNullOrEmpty(ObjectDefinition.Namespace))
                {
                    output.Append("}");
                    output.AppendLine();
                }

                return output.ToString();
            }
        }

        protected virtual void AddConstants(int start, StringBuilder output)
        {
            if (ObjectDefinition.Constants == null || ObjectDefinition.Constants.Count == 0)
            {
                return;
            }

            if (ObjectDefinition.UseRegionsToGroupClassMembers)
            {
                output.AppendLine();

                output.AppendFormat("{0}#region {1}", Indent(start + 1), ConstantsRegionDescription);
                output.AppendLine();

                output.AppendLine();
            }

            foreach (var constant in ObjectDefinition.Constants)
            {
                output.AppendFormat("{0}{1} const {2} {3} = {4};", Indent(start + 1), constant.AccessModifier.ToString().ToLower(), constant.Type, constant.Name, constant.Value);
                output.AppendLine();
            }

            output.AppendLine();

            if (ObjectDefinition.UseRegionsToGroupClassMembers)
            {
                output.AppendLine();

                output.AppendFormat("{0}#endregion", Indent(start + 1));
                output.AppendLine();

                output.AppendLine();
            }
        }

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
                    output.AppendFormat("{0}{1} event {2} {3};", Indent(start + 1), @event.AccessModifier.ToString().ToLower(), @event.Type, @event.Name);
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

        protected virtual void AddFields(int start, StringBuilder output)
        {
            if (ObjectDefinition.Fields == null || ObjectDefinition.Fields.Count == 0)
            {
                return;
            }

            if (ObjectDefinition.UseRegionsToGroupClassMembers)
            {
                output.AppendLine();

                output.AppendFormat("{0}#region {1}", Indent(start + 1), FieldsRegionDescription);
                output.AppendLine();

                output.AppendLine();
            }

            if (ObjectDefinition.Fields.Count > 0)
            {
                foreach (var field in ObjectDefinition.Fields)
                {
                    var fieldSignature = new List<string>();

                    fieldSignature.Add(field.AccessModifier.ToString().ToLower());

                    if (field.IsStatic)
                    {
                        fieldSignature.Add("static");
                    }

                    if (field.IsReadOnly)
                    {
                        fieldSignature.Add("readonly");
                    }

                    fieldSignature.Add(field.Type);

                    fieldSignature.Add(field.Name);

                    output.AppendFormat("{0}{1};", Indent(start + 1), string.Join(" ", fieldSignature));
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

        protected virtual void AddStaticConstructor(int start, StringBuilder output)
        {
            if (ObjectDefinition.StaticConstructor == null)
            {
                return;
            }

            output.AppendFormat("{0}static {1}()", Indent(start + 1), ObjectDefinition.Name);
            output.AppendLine();

            output.AppendFormat("{0}{1}", Indent(start + 1), "{");
            output.AppendLine();

            foreach (var line in ObjectDefinition.StaticConstructor.Lines)
            {
                if (line.IsComment())
                {
                    output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), GetComment(line.Content));
                }
                else if (line.IsPreprocessorDirective())
                {
                    output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), GetPreprocessorDirective(line.Content));
                }
                else if (line.IsTodo())
                {
                    output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), GetTodo(line.Content));
                }
                else
                {
                    output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), line.Content);
                }

                output.AppendLine();
            }

            output.AppendFormat("{0}{1}", Indent(start + 1), "}");
            output.AppendLine();

            output.AppendLine();
        }

        protected virtual void AddConstructors(int start, StringBuilder output)
        {
            if (ObjectDefinition.Constructors == null || ObjectDefinition.Constructors.Count == 0)
            {
                return;
            }

            if (ObjectDefinition.UseRegionsToGroupClassMembers)
            {
                output.AppendFormat("{0}#region {1}", Indent(start + 1), ConstructorsRegionDescription);
                output.AppendLine();
            }

            for (var i = 0; i < ObjectDefinition.Constructors.Count; i++)
            {
                var constructor = ObjectDefinition.Constructors[i];

                output.AppendFormat("{0}{1} {2}({3})", Indent(start + 1), constructor.AccessModifier.ToString().ToLower(), ObjectDefinition.Name, constructor.Parameters.Count == 0 ? string.Empty : string.Join(", ", constructor.Parameters.Select(item => string.Format("{0} {1}", item.Type, item.Name))));
                output.AppendLine();

                if (!string.IsNullOrEmpty(constructor.Invocation))
                {
                    output.AppendFormat("{0}: {1}", Indent(start + 2), constructor.Invocation);
                    output.AppendLine();
                }

                output.AppendFormat("{0}{1}", Indent(start + 1), "{");
                output.AppendLine();

                foreach (var line in constructor.Lines)
                {
                    if (line.IsComment())
                    {
                        output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), GetComment(line.Content));
                    }
                    else if (line.IsPreprocessorDirective())
                    {
                        output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), GetPreprocessorDirective(line.Content));
                    }
                    else if (line.IsTodo())
                    {
                        output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), GetTodo(line.Content));
                    }
                    else
                    {
                        output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), line.Content);
                    }

                    output.AppendLine();
                }

                output.AppendFormat("{0}{1}", Indent(start + 1), "}");
                output.AppendLine();

                if (i < ObjectDefinition.Constructors.Count - 1)
                {
                    output.AppendLine();
                }
            }

            if (ObjectDefinition.UseRegionsToGroupClassMembers)
            {
                output.AppendLine();

                output.AppendFormat("{0}#endregion", Indent(start + 1));
                output.AppendLine();
            }

            output.AppendLine();
        }

        protected virtual void AddFinalizer(int start, StringBuilder output)
        {
            if (ObjectDefinition.Finalizer == null || ObjectDefinition.Finalizer.Lines == null || ObjectDefinition.Finalizer.Lines.Count == 0)
            {
                return;
            }

            output.AppendFormat("{0}~{1}()", Indent(start + 1), ObjectDefinition.Name);
            output.AppendLine();

            output.AppendFormat("{0}{1}", Indent(start + 1), "{");
            output.AppendLine();

            foreach (var line in ObjectDefinition.Finalizer.Lines)
            {
                if (line.IsComment())
                {
                    output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), GetComment(line.Content));
                }
                else if (line.IsPreprocessorDirective())
                {
                    output.AppendFormat("{0}", GetPreprocessorDirective(line.Content));
                }
                else if (line.IsTodo())
                {
                    output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), GetTodo(line.Content));
                }
                else
                {
                    output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), line.Content);
                }

                output.AppendLine();
            }

            output.AppendFormat("{0}{1}", Indent(start + 1), "}");
            output.AppendLine();
        }

        protected virtual void AddIndexers(int start, StringBuilder output)
        {
            if (ObjectDefinition.Indexers == null || ObjectDefinition.Indexers.Count == 0)
            {
                return;
            }

            if (ObjectDefinition.UseRegionsToGroupClassMembers)
            {
                output.AppendFormat("{0}#region {1}", Indent(start + 1), IndexersRegionDescription);
                output.AppendLine();

                output.AppendLine();
            }

            for (var i = 0; i < ObjectDefinition.Indexers.Count; i++)
            {
                var indexer = ObjectDefinition.Indexers[i];

                var parameters = string.Join(", ", indexer.Parameters.Select(item => string.Format("{0} {1}", item.Type, item.Name)));

                output.AppendFormat("{0}{1} {2} {3}[{4}]", Indent(start + 1), indexer.AccessModifier.ToString().ToLower(), indexer.Type, "this", parameters);
                output.AppendLine();

                output.AppendFormat("{0}{1}", Indent(start + 1), "{");
                output.AppendLine();

                if (indexer.GetBody.Count > 0)
                {
                    output.AppendFormat("{0}get", Indent(start + 2));
                    output.AppendLine();

                    output.AppendFormat("{0}{1}", Indent(start + 2), "{");
                    output.AppendLine();

                    foreach (var line in indexer.GetBody)
                    {
                        if (line.IsComment())
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 3 + line.Indent), GetComment(line.Content));
                        }
                        else if (line.IsTodo())
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 3 + line.Indent), GetTodo(line.Content));
                        }
                        else if (line.IsPreprocessorDirective())
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 3 + line.Indent), GetPreprocessorDirective(line.Content));
                        }
                        else
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 3 + line.Indent), line.Content);
                        }

                        output.AppendLine();
                    }

                    output.AppendFormat("{0}{1}", Indent(start + 2), "}");
                    output.AppendLine();
                }

                if (indexer.SetBody.Count > 0)
                {
                    output.AppendFormat("{0}set", Indent(start + 2));
                    output.AppendLine();

                    output.AppendFormat("{0}{1}", Indent(start + 2), "{");
                    output.AppendLine();

                    foreach (var line in indexer.SetBody)
                    {
                        if (line.IsComment())
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 3 + line.Indent), GetComment(line.Content));
                        }
                        else if (line.IsPreprocessorDirective())
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 3 + line.Indent), GetPreprocessorDirective(line.Content));
                        }
                        else if (line.IsTodo())
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 3 + line.Indent), GetTodo(line.Content));
                        }
                        else
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 3 + line.Indent), line.Content);
                        }

                        output.AppendLine();
                    }

                    output.AppendFormat("{0}{1}", Indent(start + 2), "}");
                    output.AppendLine();
                }

                output.AppendFormat("{0}{1}", Indent(start + 1), "}");
                output.AppendLine();

                if (i < ObjectDefinition.Indexers.Count - 1)
                {
                    output.AppendLine();
                }
            }

            if (ObjectDefinition.UseRegionsToGroupClassMembers)
            {
                output.AppendLine();

                output.AppendFormat("{0}#endregion", Indent(start + 1));
                output.AppendLine();
            }

            output.AppendLine();
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
                    if (property.GetBody.Count == 0)
                    {
                        output.AppendFormat("{0}{1} {2} {3} {{ get; }}", Indent(start + 1), property.AccessModifier.ToString().ToLower(), property.Type, property.Name);
                        output.AppendLine();
                    }
                    else
                    {
                        var propertySignature = new List<string>();

                        propertySignature.Add(property.AccessModifier.ToString().ToLower());

                        if (property.IsOverride)
                        {
                            // todo: add logic for override property
                        }
                        else if (property.IsVirtual)
                        {
                            propertySignature.Add("virtual");
                        }

                        propertySignature.Add(property.Type);

                        propertySignature.Add(property.Name);

                        output.AppendFormat("{0}{1}", Indent(start + 1), string.Join(" ", propertySignature));
                        output.AppendLine();

                        if (property.GetBody.Count == 1)
                        {
                            output.AppendFormat("{0}=> {1}", Indent(start + 2), property.GetBody[0].Content.Replace("return ", string.Empty));
                            output.AppendLine();
                        }
                        else
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 1), "{");
                            output.AppendLine();

                            output.AppendFormat("{0}get", Indent(start + 2));
                            output.AppendLine();

                            output.AppendFormat("{0}{1}", Indent(start + 2), "{");
                            output.AppendLine();

                            foreach (var line in property.GetBody)
                            {
                                if (line.IsComment())
                                {
                                    output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), GetComment(line.Content));
                                }
                                else if (line.IsTodo())
                                {
                                    output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), GetTodo(line.Content));
                                }
                                else
                                {
                                    output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), line.Content);
                                }

                                output.AppendLine();
                            }

                            output.AppendFormat("{0}{1}", Indent(start + 2), "}");
                            output.AppendLine();

                            output.AppendFormat("{0}{1}", Indent(start + 1), "}");
                            output.AppendLine();
                        }
                    }
                }
                else if (property.IsAutomatic)
                {
                    var propertySignature = new List<string>();

                    propertySignature.Add(property.AccessModifier.ToString().ToLower());

                    if (property.IsOverride)
                    {
                        // todo: add logic for override property
                    }
                    else if (property.IsVirtual)
                    {
                        propertySignature.Add("virtual");
                    }

                    propertySignature.Add(property.Type);

                    propertySignature.Add(property.Name);

                    output.AppendFormat("{0}{1} {{ get; set; }}", Indent(start + 1), string.Join(" ", propertySignature));
                    output.AppendLine();
                }
                else
                {
                    output.AppendFormat("{0}{1} {2} {3}", Indent(start + 1), property.AccessModifier.ToString().ToLower(), property.Type, property.Name);
                    output.AppendLine();

                    output.AppendFormat("{0}{1}", Indent(start + 1), "{");
                    output.AppendLine();

                    output.AppendFormat("{0}get", Indent(start + 2));
                    output.AppendLine();

                    output.AppendFormat("{0}{1}", Indent(start + 2), "{");
                    output.AppendLine();

                    foreach (var line in property.GetBody)
                    {
                        if (line.IsComment())
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 3 + line.Indent), GetComment(line.Content));
                        }
                        else if (line.IsPreprocessorDirective())
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 3 + line.Indent), GetPreprocessorDirective(line.Content));
                        }
                        else if (line.IsTodo())
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 3 + line.Indent), GetTodo(line.Content));
                        }
                        else
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 3 + line.Indent), line.Content);
                        }

                        output.AppendLine();
                    }

                    output.AppendFormat("{0}{1}", Indent(start + 2), "}");
                    output.AppendLine();

                    output.AppendFormat("{0}set", Indent(start + 2));
                    output.AppendLine();

                    output.AppendFormat("{0}{1}", Indent(start + 2), "{");
                    output.AppendLine();

                    foreach (var line in property.SetBody)
                    {
                        if (line.IsComment())
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 3 + line.Indent), GetComment(line.Content));
                        }
                        else if (line.IsPreprocessorDirective())
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 3 + line.Indent), GetPreprocessorDirective(line.Content));
                        }
                        else if (line.IsTodo())
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 3 + line.Indent), GetTodo(line.Content));
                        }
                        else
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 3 + line.Indent), line.Content);
                        }

                        output.AppendLine();
                    }

                    output.AppendFormat("{0}{1}", Indent(start + 2), "}");
                    output.AppendLine();

                    output.AppendFormat("{0}{1}", Indent(start + 1), "}");
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
                output.AppendFormat("{0}#region {1}", Indent(start + 2), MethodsRegionDescription);
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

                methodSignature.Add(method.AccessModifier.ToString().ToLower());

                if (method.IsAsync)
                {
                    methodSignature.Add("async");
                }

                if (method.IsStatic)
                {
                    methodSignature.Add("static");
                }
                else if (method.IsOverride)
                {
                    methodSignature.Add("override");
                }
                else if (method.IsVirtual)
                {
                    methodSignature.Add("virtual");
                }
                else if (method.IsAbstract)
                {
                    methodSignature.Add("abstract");
                }

                methodSignature.Add(string.IsNullOrEmpty(method.Type) ? "void" : method.Type);

                if (string.IsNullOrEmpty(method.GenericType))
                {
                    methodSignature.Add(method.Name);
                }
                else
                {
                    methodSignature.Add(string.Format("{0}<{1}>", method.Name, method.GenericType));
                }

                output.AppendFormat("{0}{1}", Indent(start + 1), string.Join(" ", methodSignature));

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

                output.AppendFormat("({0})", string.Join(", ", parameters));

                if (method.WhereConstraints.Count > 0)
                {
                    output.AppendFormat(" where {0}", string.Join(", ", method.WhereConstraints));
                }

                output.AppendLine();

                if (method.Lines.Count == 0)
                {
                    output.AppendFormat("{0}{1}", Indent(start + 1), "{");
                    output.AppendLine();

                    output.AppendFormat("{0}{1}", Indent(start + 1), "}");
                    output.AppendLine();
                }
                else if (method.Lines.Count == 1)
                {
                    output.AppendFormat("{0}=> {1}", Indent(start + 2), method.Lines[0].Content.Replace("return ", string.Empty));
                    output.AppendLine();
                }
                else if (method.Lines.Count > 1)
                {
                    output.AppendFormat("{0}{1}", Indent(start + 1), "{");
                    output.AppendLine();

                    foreach (var line in method.Lines)
                    {
                        if (line.IsComment())
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), GetComment(line.Content));
                        }
                        else if (line.IsPreprocessorDirective())
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), GetPreprocessorDirective(line.Content));
                        }
                        else if (line.IsTodo())
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), GetTodo(line.Content));
                        }
                        else
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), line.Content);
                        }

                        output.AppendLine();
                    }

                    output.AppendFormat("{0}{1}", Indent(start + 1), "}");
                    output.AppendLine();
                }

                if (i < ObjectDefinition.Methods.Count - 1)
                {
                    output.AppendLine();
                }
            }

            if (ObjectDefinition.UseRegionsToGroupClassMembers)
            {
                output.AppendFormat("{0}#endregion", Indent(start + 2));
                output.AppendLine();

                output.AppendLine();
            }
        }
    }
}
