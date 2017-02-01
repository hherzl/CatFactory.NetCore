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
        }

        public IDotNetInterfaceDefinition ObjectDefinition { get; set; } = new CSharpInterfaceDefinition();

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

        public override String FileExtension => "cs";

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
        }

        protected void AddMethods(Int32 start, StringBuilder output)
        {
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

                    var parameters = method.Parameters.Count == 0 ? String.Empty : String.Join(", ", method.Parameters.Select(item => String.IsNullOrEmpty(item.DefaultValue) ? String.Format("{0} {1}", item.Type, item.Name) : String.Format(String.Format("{0} {1} = {2}", item.Type, item.Name, item.DefaultValue))));

                    output.AppendFormat("{0}{1} {2}({3})", Indent(2), String.IsNullOrEmpty(method.Type) ? "void" : method.Type, method.Name, parameters);

                    if (method.WhereConstraints.Count > 0)
                    {
                        output.AppendFormat(" where {0}", String.Join(", ", method.WhereConstraints));
                    }

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

                this.AddAttributes(output, start);

                var interfaceDeclaration = new List<String>();

                interfaceDeclaration.Add(ObjectDefinition.AccessModifier.ToString().ToLower());

                if (ObjectDefinition.IsPartial)
                {
                    interfaceDeclaration.Add("partial");
                }

                interfaceDeclaration.Add("interface");

                interfaceDeclaration.Add(ObjectDefinition.Name);

                if (ObjectDefinition.HasInheritance)
                {
                    interfaceDeclaration.Add(":");

                    var parents = new List<String>();

                    if (ObjectDefinition.Implements.Count > 0)
                    {
                        parents.AddRange(ObjectDefinition.Implements);
                    }

                    interfaceDeclaration.Add(String.Join(", ", parents));
                }

                output.AppendFormat("{0}{1}", Indent(start), String.Join(" ", interfaceDeclaration));

                output.AppendLine();

                output.AppendFormat("{0}{1}", Indent(start), "{");
                output.AppendLine();

                AddEvents(start, output);

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
