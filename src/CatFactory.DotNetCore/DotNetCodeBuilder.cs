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
        }

        public String ConstantsRegionDescription { get; set; }

        public String EventsRegionDescription { get; set; }

        public String FieldsRegionDescription { get; set; }

        public String ConstructorsRegionDescription { get; set; }

        public String FinalizerRegionDescription { get; set; }

        public String PropertiesRegionDescription { get; set; }

        public String MethodsRegionDescription { get; set; }

        protected virtual void AddDocumentation(StringBuilder output, Int32 start, Documentation documentation)
        {
        }

        protected virtual String GetTodo(String description)
            => description;

        protected virtual String GetWarning(String description)
            => description;
    }
}
