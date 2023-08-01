using System;
using System.Collections.Generic;
using CatFactory.CodeFactory;
using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public static class CSharpMethodDefinitionExtensions
    {
        public static MethodDefinition AddParam(this MethodDefinition definition, string type, string name)
        {
            definition.Parameters.Add(new ParameterDefinition(type, name));
            return definition;
        }

        public static MethodDefinition AddParam(this MethodDefinition definition, string type, string name, string defaultValue)
        {
            definition.Parameters.Add(new ParameterDefinition(type, name, defaultValue));
            return definition;
        }

        public static MethodDefinition Set(this MethodDefinition definition, Action<List<ILine>> action)
        {
            action.Invoke(definition.Lines);
            return definition;
        }

        public static List<ILine> Line(this List<ILine> lines, string line)
        {
            lines.Add(new CodeLine(line));
            return lines;
        }

        public static List<ILine> Line(this List<ILine> lines, int tabs, string line)
        {
            lines.Add(new CodeLine(tabs, line));
            return lines;
        }

        public static List<ILine> Comment(this List<ILine> lines, string comment)
        {
            lines.Add(new CommentLine(comment));
            return lines;
        }

        public static List<ILine> Comment(this List<ILine> lines, int tabs, string comment)
        {
            lines.Add(new CommentLine(tabs, comment));
            return lines;
        }

        public static List<ILine> Empty(this List<ILine> lines)
        {
            lines.Add(new EmptyLine());
            return lines;
        }

        public static List<ILine> Return(this List<ILine> lines, string ret)
        {
            lines.Add(new ReturnLine(ret));
            return lines;
        }

        public static List<ILine> Todo(this List<ILine> lines, string todo)
        {
            lines.Add(new TodoLine(todo));
            return lines;
        }
    }
}
