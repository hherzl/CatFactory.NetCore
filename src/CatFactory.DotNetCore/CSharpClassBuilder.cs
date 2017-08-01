using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatFactory.DotNetCore
{
    public class CSharpClassBuilder : CSharpCodeBuilder
    {
        public CSharpClassBuilder()
            : base()
        {
        }

        public IDotNetClassDefinition ObjectDefinition { get; set; } = new CSharpClassDefinition();

        public override String FileName
        {
            get
            {
                if (ObjectDefinition != null && ObjectDefinition.Name != null && ObjectDefinition.Name.Contains("<"))
                {
                    return ObjectDefinition.Name.Substring(0, ObjectDefinition.Name.IndexOf("<"));
                }

                return ObjectDefinition.Name;
            }
        }

        public override void CreateFile(string subdirectory = "", string fileName = "")
        {
            ObjectDefinition.Init();

            base.CreateFile(subdirectory, fileName);
        }

        public override String Code
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

                if (!String.IsNullOrEmpty(ObjectDefinition.Namespace))
                {
                    start = 1;

                    output.AppendFormat("namespace {0}", ObjectDefinition.Namespace);
                    output.AppendLine();

                    output.AppendFormat("{0}", "{");
                    output.AppendLine();
                }

                if (!String.IsNullOrEmpty(ObjectDefinition.Documentation.Summary))
                {
                    AddDocumentation(output, start, ObjectDefinition.Documentation);
                }

                this.AddAttributes(output, start);

                var classDeclaration = new List<String>();

                classDeclaration.Add(ObjectDefinition.AccessModifier.ToString().ToLower());

                if (ObjectDefinition.IsStatic)
                {
                    classDeclaration.Add("static");
                }

                if (ObjectDefinition.IsPartial)
                {
                    classDeclaration.Add("partial");
                }

                classDeclaration.Add("class");

                classDeclaration.Add(ObjectDefinition.Name);

                if (ObjectDefinition.HasInheritance)
                {
                    classDeclaration.Add(":");

                    var parents = new List<String>();

                    if (!String.IsNullOrEmpty(ObjectDefinition.BaseClass))
                    {
                        parents.Add(ObjectDefinition.BaseClass);
                    }

                    if (ObjectDefinition.Implements.Count > 0)
                    {
                        parents.AddRange(ObjectDefinition.Implements);
                    }

                    classDeclaration.Add(String.Join(", ", parents));
                }

                output.AppendFormat("{0}{1}", Indent(start), String.Join(" ", classDeclaration));

                output.AppendLine();

                output.AppendFormat("{0}{1}", Indent(start), "{");
                output.AppendLine();

                AddConstants(start, output);

                AddEvents(start, output);

                AddFields(start, output);

                AddStaticConstructor(start, output);

                AddConstructors(start, output);

                AddFinalizer(start, output);

                AddProperties(start, output);

                AddMethods(start, output);

                output.AppendFormat("{0}{1}", Indent(start), "}");
                output.AppendLine();

                if (!String.IsNullOrEmpty(ObjectDefinition.Namespace))
                {
                    output.Append("}");
                    output.AppendLine();
                }

                return output.ToString();
            }
        }

        protected void AddConstants(Int32 start, StringBuilder output)
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

        protected void AddEvents(Int32 start, StringBuilder output)
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

        protected void AddFields(Int32 start, StringBuilder output)
        {
            if (ObjectDefinition.Fields != null && ObjectDefinition.Fields.Count > 0)
            {
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
                        var fieldSignature = new List<String>();

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

                        output.AppendFormat("{0}{1};", Indent(start + 1), String.Join(" ", fieldSignature));
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
        }

        protected void AddStaticConstructor(Int32 start, StringBuilder output)
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
                else if (line.IsTodo())
                {
                    output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), GetTodo(line.Content));
                }
                else if (line.IsWarning())
                {
                    output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), GetWarning(line.Content));
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

        protected void AddConstructors(Int32 start, StringBuilder output)
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

            foreach (var constructor in ObjectDefinition.Constructors)
            {
                output.AppendFormat("{0}{1} {2}({3})", Indent(start + 1), constructor.AccessModifier.ToString().ToLower(), ObjectDefinition.Name, constructor.Parameters.Count == 0 ? String.Empty : String.Join(", ", constructor.Parameters.Select(item => String.Format("{0} {1}", item.Type, item.Name))));
                output.AppendLine();

                if (!String.IsNullOrEmpty(constructor.ParentInvoke))
                {
                    output.AppendFormat("{0}: {1}", Indent(start + 2), constructor.ParentInvoke);
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
                    else if (line.IsTodo())
                    {
                        output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), GetTodo(line.Content));
                    }
                    else if (line.IsWarning())
                    {
                        output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), GetWarning(line.Content));
                    }
                    else
                    {
                        output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), line.Content);
                    }

                    output.AppendLine();
                }

                output.AppendFormat("{0}{1}", Indent(start + 1), "}");
                output.AppendLine();

                if (ObjectDefinition.Constructors.Count > 1)
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

        protected void AddFinalizer(Int32 start, StringBuilder output)
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
                else if (line.IsTodo())
                {
                    output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), GetTodo(line.Content));
                }
                else if (line.IsWarning())
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

        protected void AddProperties(Int32 start, StringBuilder output)
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
                        var propertySignature = new List<String>();

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

                        output.AppendFormat("{0}{1}", Indent(start + 1), String.Join(" ", propertySignature));
                        output.AppendLine();

                        if (property.GetBody.Count == 1)
                        {
                            output.AppendFormat("{0}=> {1}", Indent(start + 2), property.GetBody[0].Content.Replace("return ", String.Empty));
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
                                else if (line.IsWarning())
                                {
                                    output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), GetWarning(line.Content));
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
                    var propertySignature = new List<String>();

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

                    output.AppendFormat("{0}{1} {{ get; set; }}", Indent(start + 1), String.Join(" ", propertySignature));
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
                        else if (line.IsTodo())
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 3 + line.Indent), GetTodo(line.Content));
                        }
                        else if (line.IsWarning())
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 3 + line.Indent), GetWarning(line.Content));
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
                        else if (line.IsTodo())
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 3 + line.Indent), GetTodo(line.Content));
                        }
                        else if (line.IsWarning())
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

        protected void AddMethods(Int32 start, StringBuilder output)
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

                var methodSignature = new List<String>();

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

                methodSignature.Add(String.IsNullOrEmpty(method.Type) ? "void" : method.Type);

                methodSignature.Add(method.Name);

                output.AppendFormat("{0}{1}", Indent(start + 1), String.Join(" ", methodSignature));

                var parameters = new List<String>();

                for (var j = 0; j < method.Parameters.Count; j++)
                {
                    var parameter = method.Parameters[j];

                    var parametersAttributes = this.AddAttributes(parameter);

                    var parameterDef = String.Empty;

                    if (String.IsNullOrEmpty(parameter.DefaultValue))
                    {
                        if (String.IsNullOrEmpty(parametersAttributes))
                        {
                            parameterDef = String.Format("{0} {1}", parameter.Type, parameter.Name);
                        }
                        else
                        {
                            parameterDef = String.Format("{0}{1} {2}", parametersAttributes, parameter.Type, parameter.Name);
                        }
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(parametersAttributes))
                        {
                            parameterDef = String.Format("{0} {1} = {2}", parameter.Type, parameter.Name, parameter.DefaultValue);
                        }
                        else
                        {
                            parameterDef = String.Format("{0}{1} {2} = {3}", parametersAttributes, parameter.Type, parameter.Name, parameter.DefaultValue);
                        }
                    }

                    parameters.Add(method.IsExtension && j == 0 ? String.Format("this {0}", parameterDef) : parameterDef);
                }

                if (!String.IsNullOrEmpty(method.GenericType))
                {
                    output.AppendFormat("<{0}>", method.GenericType);
                }

                output.AppendFormat("({0})", String.Join(", ", parameters));

                if (method.WhereConstraints.Count > 0)
                {
                    output.AppendFormat(" where {0}", String.Join(", ", method.WhereConstraints));
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
                    output.AppendFormat("{0}=> {1}", Indent(start + 2), method.Lines[0].Content.Replace("return ", String.Empty));
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
                        else if (line.IsTodo())
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), GetTodo(line.Content));
                        }
                        else if (line.IsWarning())
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), GetWarning(line.Content));
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
