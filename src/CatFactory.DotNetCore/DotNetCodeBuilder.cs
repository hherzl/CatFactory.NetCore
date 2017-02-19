using System;
using System.Text;
using CatFactory.CodeFactory;
using CatFactory.OOP;

namespace CatFactory.DotNetCore
{
    public abstract class DotNetCodeBuilder : CodeBuilder
    {
        public DotNetCodeBuilder()
        {
            EventsRegionDescription = "[ Constants ]";
            EventsRegionDescription = "[ Events ]";
            FieldsRegionDescription = "[ Fields ]";
            ConstructorsRegionDescription = "[ Constructor ]";
            PropertiesRegionDescription = "[ Properties ]";
            MethodsRegionDescription = "[ Methods ]";
        }

        public String ConstantsRegionDescription { get; set; }

        public String EventsRegionDescription { get; set; }

        public String FieldsRegionDescription { get; set; }

        public String ConstructorsRegionDescription { get; set; }

        public String PropertiesRegionDescription { get; set; }

        public String MethodsRegionDescription { get; set; }

        protected void AddDocumentation(StringBuilder output, int start, Documentation documentation)
        {
            output.AppendFormat("{0}/// <summary>", Indent(start));
            output.AppendLine();

            output.AppendFormat("{0}/// {1}", Indent(start), documentation.Summary);
            output.AppendLine();

            output.AppendFormat("{0}/// </summary>", Indent(start));
            output.AppendLine();
        }

        protected String GetToDo(String description)
        {
            return String.Format("// todo: {0}", description);
        }

        protected String GetComment(String description)
        {
            return String.Format("// {0}", description);
        }
    }
}
