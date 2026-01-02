using System;
using System.Collections.Generic;
using CatFactory.CodeFactory;
using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming;

public static class ClassConstructorDefinitionExtensions
{
    public static ClassConstructorDefinition AddParam(this ClassConstructorDefinition definition, string type, string name, string defaultValue = null, string summary = null)
    {
        definition.Parameters.Add(new ParameterDefinition(type, name, defaultValue)
        {
            Documentation =
            {
                Summary = summary
            }
        });

        return definition;
    }

    public static ClassConstructorDefinition Set(this ClassConstructorDefinition definition, Action<ICollection<ILine>> action)
    {
        action.Invoke(definition.Lines);
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
