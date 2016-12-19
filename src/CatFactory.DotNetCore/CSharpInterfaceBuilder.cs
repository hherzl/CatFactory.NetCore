using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatFactory.DotNetCore
{
    public class CSharpInterfaceBuilder : DotNetCodeBuilder
    {
        public CSharpInterfaceBuilder()
            : base()
        {
            ObjectDefinition = new CSharpInterfaceDefinition();
        }

        public IDotNetInterfaceDefinition ObjectDefinition { get; set; }

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

                if (ObjectDefinition.Attributes.Count > 0)
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

                if (ObjectDefinition.IsPartial)
                {
                    output.AppendFormat("{0}public partial interface {1}", Indent(1), ObjectDefinition.Name);
                }
                else
                {
                    output.AppendFormat("{0}public interface {1}", Indent(1), ObjectDefinition.Name);
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

                if (ObjectDefinition.Properties != null && ObjectDefinition.Properties.Count > 0)
                {
                    if (ObjectDefinition.UseRegionsToGroupClassMembers)
                    {
                        output.AppendFormat("{0}#region {1}", Indent(2), PropertiesRegionDescription);
                        output.AppendLine();

                        output.AppendLine();
                    }

                    var namingConvention = new DotNetNamingConvention();

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
                            output.AppendFormat("{0}{1} {2} {{ get; }}", Indent(2), property.Type, property.Name);
                        }
                        else
                        {
                            output.AppendFormat("{0}{1} {2} {{ get; set; }}", Indent(2), property.Type, property.Name);
                        }

                        output.AppendLine();

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

                    for (var i = 0; i < ObjectDefinition.Methods.Count; i++)
                    {
                        var method = ObjectDefinition.Methods[i];

                        foreach (var attrib in method.Attributes)
                        {
                            output.AppendFormat("{0}[{1}]", Indent(2), attrib.Name);
                            output.AppendLine();
                        }

                        var parameters = method.Parameters.Count == 0 ? String.Empty : String.Join(", ", method.Parameters.Select(item => String.Format("{0} {1}", item.Type, item.Name)));

                        output.AppendFormat("{0}{1} {2}({3});", Indent(2), String.IsNullOrEmpty(method.Type) ? "void" : method.Type, method.Name, parameters);
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
