using CatFactory.CodeFactory;
using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.CodeFactory
{
    public abstract class DotNetCodeBuilder : CodeBuilder
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

        protected abstract void AddDocumentation(int start, IObjectDefinition definition);

        protected abstract void AddDocumentation(int start, ClassConstructorDefinition definition);

        protected abstract void AddDocumentation(int start, PropertyDefinition definition);

        protected abstract void AddDocumentation(int start, MethodDefinition definition);

        protected virtual string GetComment(string description)
            => description;

        protected virtual string GetPreprocessorDirective(string description)
            => description;

        protected virtual string GetTodo(string description)
            => description;
    }
}
