using System;
using System.Text;
using CatFactory.OOP;

namespace CatFactory.DotNetCore
{
    public class CSharpCodeBuilder : DotNetCodeBuilder
    {
        public CSharpCodeBuilder()
        {
            EventsRegionDescription = "[ Constants ]";
            EventsRegionDescription = "[ Events ]";
            FieldsRegionDescription = "[ Fields ]";
            ConstructorsRegionDescription = "[ Constructor ]";
            FinalizerRegionDescription = "[ Finalizer ]";
            PropertiesRegionDescription = "[ Properties ]";
            MethodsRegionDescription = "[ Methods ]";
        }

        protected override void AddDocumentation(StringBuilder output, Int32 start, Documentation documentation)
        {
            output.AppendFormat("{0}/// <summary>", Indent(start));
            output.AppendLine();

            output.AppendFormat("{0}/// {1}", Indent(start), documentation.Summary);
            output.AppendLine();

            output.AppendFormat("{0}/// </summary>", Indent(start));
            output.AppendLine();
        }

        public override String FileExtension
            => "cs";

        protected override String GetComment(String description)
            => String.Format("//{0}", description);

        protected override String GetTodo(String description)
            => String.Format("// todo: {0}", description);

        protected override String GetWarning(String description)
            => String.Format("#warning {0}", description);
    }
}
