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

                if (!String.IsNullOrEmpty(ObjectDefinition.Namespace))
                {
                    output.AppendFormat("namespace {0}", ObjectDefinition.Namespace);
                    output.AppendLine();
                    output.AppendFormat("{0}", "{");
                    output.AppendLine();
                }

                if (ObjectDefinition.IsPartial)
                {
                    output.AppendFormat("{0}public partial class {1}", Indent(1), ObjectDefinition.Name);
                }
                else
                {
                    output.AppendFormat("{0}public class {1}", Indent(1), ObjectDefinition.Name);
                }

                if (ObjectDefinition.HasInheritance)
                {
                    output.AppendFormat(" : ");

                    var parents = new List<String>();

                    if (!String.IsNullOrEmpty(ObjectDefinition.BaseClass))
                    {
                        parents.Add(ObjectDefinition.BaseClass);
                    }

                    if (ObjectDefinition.Implements.Count > 0)
                    {
                        parents.AddRange(ObjectDefinition.Implements);
                    }

                    output.Append(String.Join(", ", parents));
                }

                output.AppendLine();

                output.AppendFormat("{0}{1}", Indent(1), "{");
                output.AppendLine();

                if (ObjectDefinition.Fields != null && ObjectDefinition.Fields.Count > 0)
                {
                    if (ObjectDefinition.UseRegionsToGroupClassMembers)
                    {
                        output.AppendLine();

                        output.AppendFormat("{0}#region {1}", Indent(2), FieldsRegionDescription);
                        output.AppendLine();

                        output.AppendLine();
                    }

                    if (ObjectDefinition.Fields.Count > 0)
                    {
                        foreach (var field in ObjectDefinition.Fields)
                        {
                            output.AppendFormat("{0}{1} {2} {3};", Indent(2), field.ModifierAccess.ToString().ToLower(), field.Type, field.Name);
                            output.AppendLine();
                        }

                        output.AppendLine();
                    }

                    if (ObjectDefinition.UseRegionsToGroupClassMembers)
                    {
                        output.AppendLine();

                        output.AppendFormat("{0}#endregion", Indent(2));
                        output.AppendLine();

                        output.AppendLine();
                    }
                }

                if (ObjectDefinition.Constructors != null && ObjectDefinition.Constructors.Count > 0)
                {
                    if (ObjectDefinition.UseRegionsToGroupClassMembers)
                    {
                        output.AppendFormat("{0}#region {1}", Indent(2), ConstructorsRegionDescription);
                        output.AppendLine();
                    }

                    foreach (var constructor in ObjectDefinition.Constructors)
                    {
                        output.AppendFormat("{0}public {1}({2})", Indent(2), ObjectDefinition.Name, constructor.Parameters.Count == 0 ? String.Empty : String.Join(", ", constructor.Parameters.Select(item => String.Format("{0} {1}", item.Type, item.Name))));
                        output.AppendLine();

                        if (!String.IsNullOrEmpty(constructor.ParentInvoke))
                        {
                            output.AppendFormat("{0}: {1}", Indent(3), constructor.ParentInvoke);
                            output.AppendLine();
                        }

                        output.AppendFormat("{0}{1}", Indent(2), "{");
                        output.AppendLine();

                        foreach (var line in constructor.Lines)
                        {
                            if (line.IsNullOrEmpty)
                            {
                                output.AppendLine();
                            }
                            else
                            {
                                output.AppendFormat("{0}{1}", Indent(3 + line.Indent), line);
                                output.AppendLine();
                            }
                        }

                        output.AppendFormat("{0}{1}", Indent(2), "}");
                        output.AppendLine();

                        if (ObjectDefinition.Constructors.Count > 1)
                        {
                            output.AppendLine();
                        }
                    }

                    if (ObjectDefinition.UseRegionsToGroupClassMembers)
                    {
                        output.AppendLine();

                        output.AppendFormat("{0}#endregion", Indent(2));
                        output.AppendLine();
                    }

                    output.AppendLine();
                }

                if (ObjectDefinition.Properties != null && ObjectDefinition.Properties.Count > 0)
                {
                    if (ObjectDefinition.UseRegionsToGroupClassMembers)
                    {
                        output.AppendFormat("{0}#region {1}", Indent(2), PropertiesRegionDescription);
                        output.AppendLine();

                        output.AppendLine();
                    }

                    for (var i = 0; i < ObjectDefinition.Properties.Count; i++)
                    {
                        var property = ObjectDefinition.Properties[i];

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

                            output.AppendFormat("{0}{1}", Indent(2), attributeDefinition.ToString());
                            output.AppendLine();
                        }

                        if (property.IsReadOnly)
                        {
                            output.AppendFormat("{0}public {1} {2}", Indent(2), property.Type, property.Name);
                            output.AppendLine();

                            output.AppendFormat("{0}{1}", Indent(2), "{");
                            output.AppendLine();

                            output.AppendFormat("{0}get", Indent(3));
                            output.AppendLine();

                            output.AppendFormat("{0}{1}", Indent(3), "{");
                            output.AppendLine();

                            foreach (var line in property.GetBody)
                            {
                                output.AppendFormat("{0}{1}", Indent(4 + line.Indent), line);
                                output.AppendLine();
                            }

                            output.AppendFormat("{0}{1}", Indent(3), "}");
                            output.AppendLine();

                            output.AppendFormat("{0}{1}", Indent(2), "}");
                            output.AppendLine();
                        }
                        else if (property.IsAutomatic)
                        {
                            output.AppendFormat("{0}{1} {2} {3} {{ get; set; }}", Indent(2), property.ModifierAccess.ToString().ToLower(), property.Type, property.Name);
                            output.AppendLine();
                        }

                        if (i < ObjectDefinition.Properties.Count - 1)
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

                if (ObjectDefinition.Methods != null && ObjectDefinition.Methods.Count > 0)
                {
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

                        foreach (var attrib in method.Attributes)
                        {
                            output.AppendFormat("{0}[{1}]", Indent(2), attrib.Name);
                            output.AppendLine();
                        }

                        var parameters = method.Parameters.Count == 0 ? String.Empty : String.Join(", ", method.Parameters.Select(item => String.Format("{0} {1}", item.Type, item.Name)));

                        output.AppendFormat("{0}{1} {2}{3} {4}({5})", Indent(2), method.ModifierAccess.ToString().ToLower(), String.IsNullOrEmpty(method.Prefix) ? String.Empty : String.Format("{0} ", method.Prefix), String.IsNullOrEmpty(method.Type) ? "void" : method.Type, method.Name, parameters);
                        output.AppendLine();

                        output.AppendFormat("{0}{1}", Indent(2), "{");
                        output.AppendLine();

                        if (method.Lines.Count > 0)
                        {
                            foreach (var line in method.Lines)
                            {
                                output.AppendFormat("{0}{1}", Indent(3 + line.Indent), line);
                                output.AppendLine();
                            }
                        }

                        output.AppendFormat("{0}{1}", Indent(2), "}");
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

                output.AppendFormat("{0}{1}", Indent(1), "}");
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
