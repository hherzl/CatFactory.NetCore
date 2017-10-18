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

        protected override void AddDocumentation(StringBuilder output, int start, Documentation documentation)
        {
            output.AppendFormat("{0}/// <summary>", Indent(start));
            output.AppendLine();

            output.AppendFormat("{0}/// {1}", Indent(start), documentation.Summary);
            output.AppendLine();

            output.AppendFormat("{0}/// </summary>", Indent(start));
            output.AppendLine();
        }

        public override string FileExtension
            => "cs";

        protected override string GetComment(string description)
            => string.Format("//{0}", description);

        protected override string GetPreprocessorDirective(string description)
            => string.Format("#{0}", description);

        protected override string GetTodo(string description)
            => string.Format("// todo: {0}", description);

        protected override string GetWarning(string description)
            => string.Format("#warning {0}", description);
    }
}
