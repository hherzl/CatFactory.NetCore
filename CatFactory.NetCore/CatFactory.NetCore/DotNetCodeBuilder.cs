using System.Text;
using CatFactory.CodeFactory;
using CatFactory.OOP;

namespace CatFactory.DotNetCore
{
    public class DotNetCodeBuilder : CodeBuilder
    {
        public DotNetCodeBuilder()
        {
        }

        public string ConstantsRegionDescription { get; set; }

        public string EventsRegionDescription { get; set; }

        public string FieldsRegionDescription { get; set; }

        public string ConstructorsRegionDescription { get; set; }

        public string FinalizerRegionDescription { get; set; }

        public string IndexersRegionDescription { get; set; }

        public string PropertiesRegionDescription { get; set; }

        public string MethodsRegionDescription { get; set; }

        protected virtual void AddDocumentation(StringBuilder output, int start, Documentation documentation)
        {
        }

        protected virtual string GetComment(string description)
            => description;

        protected virtual string GetPreprocessorDirective(string description)
            => description;

        protected virtual string GetTodo(string description)
            => description;
    }
}
