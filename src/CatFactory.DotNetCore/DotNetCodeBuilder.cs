using System;
using System.Text;
using CatFactory.CodeFactory;
using CatFactory.OOP;

namespace CatFactory.DotNetCore
{
    public class DotNetCodeBuilder : CodeBuilder
    {
        public DotNetCodeBuilder()
        {
            EventsRegionDescription = "[ Events ]";
            FieldsRegionDescription = "[ Fields ]";
            ConstructorsRegionDescription = "[ Constructor ]";
            PropertiesRegionDescription = "[ Properties ]";
            MethodsRegionDescription = "[ Methods ]";
        }

        public String EventsRegionDescription { get; set; }

        public String FieldsRegionDescription { get; set; }

        public String ConstructorsRegionDescription { get; set; }

        public String PropertiesRegionDescription { get; set; }

        public String MethodsRegionDescription { get; set; }

        public virtual void AddSummary(StringBuilder output, Int32 start, Documentation documentation)
        {
            if (documentation == null)
            {
                return;
            }

            output.AppendFormat("{0}/// <summary>", Indent(start));
            output.AppendLine();

            output.AppendFormat("{0}/// {1}", Indent(start), documentation.Summary);
            output.AppendLine();

            output.AppendFormat("{0}/// </summary>", Indent(start));
            output.AppendLine();
        }
    }
}
