using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatFactory.DotNetCore
{
    public class CSharpClassBuilder : DotNetCodeBuilder
    {
        public CSharpClassBuilder()
            : base()
        {
            ObjectDefinition = new CSharpClassDefinition();
        }

        public IDotNetClassDefinition ObjectDefinition { get; set; }

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

        public override String FileExtension
        {
            get
            {
                return "cs";
            }
        }

        protected void AddAttributes (Int32 start, StringBuilder output)
        {
            foreach (var attrib in ObjectDefinition.Attributes)
            {
                var attributeDefinition = new StringBuilder();

                attributeDefinition.Append("[");

                attributeDefinition.AppendFormat("{0}", attrib.Name);

                if (attrib.HasMembers)
                {
                    attributeDefinition.Append("(");

                    if (attrib.HasArguments)
                    {
                        attributeDefinition.Append(String.Join(", ", attrib.Arguments));
                    }

                    if (attrib.HasSets)
                    {
                        attributeDefinition.Append(String.Join(", ", attrib.Sets));
                    }

                    attributeDefinition.Append(")");
                }

                attributeDefinition.Append("]");

                output.AppendFormat("{0}{1}", Indent(1), attributeDefinition.ToString());
                output.AppendLine();
            }
        }

        protected void AddEvents(Int32 start, StringBuilder output)
        {
            if (ObjectDefinition.Events != null && ObjectDefinition.Events.Count > 0)
            {
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
        }

        protected void AddFields (Int32 start, StringBuilder output)
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
                        output.AppendFormat("{0}{1} {2} {3};", Indent(start + 1), field.AccessModifier.ToString().ToLower(), field.Type, field.Name);
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

        protected void AddConstructors(Int32 start, StringBuilder output)
        {
            if (ObjectDefinition.Constructors != null && ObjectDefinition.Constructors.Count > 0)
            {
                if (ObjectDefinition.UseRegionsToGroupClassMembers)
                {
                    output.AppendFormat("{0}#region {1}", Indent(start + 1), ConstructorsRegionDescription);
                    output.AppendLine();
                }

                foreach (var constructor in ObjectDefinition.Constructors)
                {
                    output.AppendFormat("{0}public {1}({2})", Indent(start + 1), ObjectDefinition.Name, constructor.Parameters.Count == 0 ? String.Empty : String.Join(", ", constructor.Parameters.Select(item => String.Format("{0} {1}", item.Type, item.Name))));
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
                        if (line.IsNullOrEmpty)
                        {
                            output.AppendLine();
                        }
                        else
                        {
                            output.AppendFormat("{0}{1}", Indent(start + 2 + line.Indent), line);
                            output.AppendLine();
                        }
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
        }

        protected void AddProperties(Int32 start, StringBuilder output)
        {
            if (ObjectDefinition.Properties != null && ObjectDefinition.Properties.Count > 0)
            {
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
                        foreach (var attrib in property.Attributes)
                        {
                            var attributeDefinition = new StringBuilder();

                            attributeDefinition.Append("[");

                            attributeDefinition.AppendFormat("{0}", attrib.Name);

                            if (attrib.HasMembers)
                            {
                                attributeDefinition.Append("(");

                                if (attrib.HasArguments)
                                {
                                    attributeDefinition.Append(String.Join(", ", attrib.Arguments));
                                }

                                if (attrib.HasSets)
                                {
                                    attributeDefinition.Append(String.Join(", ", attrib.Sets));
                                }

                                attributeDefinition.Append(")");
                            }

                            attributeDefinition.Append("]");

                            output.AppendFormat("{0}{1}", Indent(start + 1), attributeDefinition.ToString());
                            output.AppendLine();
                        }
                    }

                    if (property.IsReadOnly)
                    {
                        output.AppendFormat("{0}{1} {2} {3}", Indent(start + 1), property.AccessModifier.ToString().ToLower(), property.Type, property.Name);
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
                                output.AppendFormat("{0}{1}", Indent(start + 3 + line.Indent), line);
                                output.AppendLine();
                            }

                            output.AppendFormat("{0}{1}", Indent(start + 2), "}");
                            output.AppendLine();

                            output.AppendFormat("{0}{1}", Indent(start + 1), "}");
                            output.AppendLine();
                        }
                    }
                    else if (property.IsAutomatic)
                    {
                        output.AppendFormat("{0}{1} {2} {3} {{ get; set; }}", Indent(start + 1), property.AccessModifier.ToString().ToLower(), property.Type, property.Name);
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
                            output.AppendFormat("{0}{1}", Indent(start + 3 + line.Indent), line);
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
                            output.AppendFormat("{0}{1}", Indent(start + 3 + line.Indent), line);
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
        }

        protected void AddMethods (Int32 start, StringBuilder output)
        {
            if (ObjectDefinition.Methods != null && ObjectDefinition.Methods.Count > 0)
            {
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

                    foreach (var attrib in method.Attributes)
                    {
                        output.AppendFormat("{0}[{1}]", Indent(start + 2), attrib.Name);
                        output.AppendLine();
                    }

                    var parameters = method.Parameters.Count == 0 ? String.Empty : String.Join(", ", method.Parameters.Select(item => String.IsNullOrEmpty(item.DefaultValue) ? String.Format("{0} {1}", item.Type, item.Name) : String.Format(String.Format("{0} {1} = {2}", item.Type, item.Name, item.DefaultValue))));

                    output.AppendFormat("{0}{1} {2}{3} {4}({5})", Indent(start + 1), method.AccessModifier.ToString().ToLower(), String.IsNullOrEmpty(method.Prefix) ? String.Empty : String.Format("{0} ", method.Prefix), String.IsNullOrEmpty(method.Type) ? "void" : method.Type, method.Name, parameters);
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
                    else if (method.Lines.Count > 0)
                    {
                        output.AppendFormat("{0}{1}", Indent(start + 1), "{");
                        output.AppendLine();

                        foreach (var line in method.Lines)
                        {
                            output.AppendFormat("{0}{1}", Indent(3 + line.Indent), line);
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

                AddAttributes(start, output);

                var classDeclaration = new List<String>();

                classDeclaration.Add(ObjectDefinition.AccessModifier.ToString().ToLower());

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

                AddEvents(start, output);

                AddFields(start, output);

                AddConstructors(start, output);

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
    }
}
