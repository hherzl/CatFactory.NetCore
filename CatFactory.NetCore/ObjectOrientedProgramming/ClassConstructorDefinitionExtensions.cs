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

        public static ClassConstructorDefinition AddLine(this ClassConstructorDefinition definition, string code)
        {
            definition.Lines.Add(new CodeLine(code));

            return definition;
        }
    }
}
