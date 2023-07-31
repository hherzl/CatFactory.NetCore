using CatFactory.CodeFactory;
using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public static class ClassConstructorDefinitionExtensions
    {
        public static ClassConstructorDefinition AddParam(this ClassConstructorDefinition definition, string type, string name)
        {
            definition.Parameters.Add(new ParameterDefinition(type, name));

            return definition;
        }

        public static ClassConstructorDefinition AddParam(this ClassConstructorDefinition definition, string type, string name, string defaultValue)
        {
            definition.Parameters.Add(new ParameterDefinition(type, name, defaultValue));

            return definition;
        }

        public static ClassConstructorDefinition AddLine(this ClassConstructorDefinition definition, string line)
        {
            definition.Lines.Add(new CodeLine(line));

            return definition;
        }

        public static ClassConstructorDefinition AddEmpty(this ClassConstructorDefinition definition)
        {
            definition.Lines.Add(new EmptyLine());

            return definition;
        }

        public static ClassConstructorDefinition AddComment(this ClassConstructorDefinition definition, string comment)
        {
            definition.Lines.Add(new CommentLine(comment));

            return definition;
        }

        public static ClassConstructorDefinition AddTodo(this ClassConstructorDefinition definition, string todo)
        {
            definition.Lines.Add(new TodoLine(todo));

            return definition;
        }
    }
}
