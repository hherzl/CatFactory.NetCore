using System;
using System.Collections.Generic;
using CatFactory.CodeFactory;
using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming;

public static class CSharpMethodDefinitionExtensions
{
    public static MethodDefinition SetDocumentation(this MethodDefinition definition, string summary = null, string remarks = null, string returns = null)
    {
        if (!string.IsNullOrEmpty(summary))
            definition.Documentation.Summary = summary;

        if (!string.IsNullOrEmpty(remarks))
            definition.Documentation.Remarks = remarks;

        if (!string.IsNullOrEmpty(returns))
            definition.Documentation.Returns = returns;

        return definition;
    }

    public static MethodDefinition IsStatic(this MethodDefinition definition, bool flag = true)
    {
        definition.IsStatic = flag;

        if (definition.IsStatic)
            definition.IsVirtual = definition.IsOverride = false;

        return definition;
    }

    public static MethodDefinition IsExtension(this MethodDefinition definition, bool flag = true)
    {
        definition.IsExtension = flag;
        definition.IsStatic = definition.IsExtension;

        if (definition.IsExtension)
            definition.IsVirtual = definition.IsOverride = false;

        return definition;
    }

    public static MethodDefinition IsVirtual(this MethodDefinition definition, bool flag = true)
    {
        definition.IsVirtual = flag;

        if (definition.IsVirtual)
            definition.IsStatic = definition.IsExtension = false;

        return definition;
    }

    public static MethodDefinition IsOverride(this MethodDefinition definition, bool flag = true)
    {
        definition.IsOverride = flag;

        if (definition.IsOverride)
            definition.IsStatic = definition.IsExtension = false;

        return definition;
    }

    public static MethodDefinition AddGenericType(this MethodDefinition definition, string name, params string[] constraints)
    {
        definition.GenericTypes.Add(new GenericTypeDefinition(name, constraints));
        return definition;
    }

    public static MethodDefinition AddParam(this MethodDefinition definition, string type, string name)
    {
        definition.Parameters.Add(new(type, name));
        return definition;
    }

    public static MethodDefinition AddParam(this MethodDefinition definition, string type, string name, string defaultValue)
    {
        definition.Parameters.Add(new(type, name, defaultValue));
        return definition;
    }

    public static MethodDefinition Set(this MethodDefinition definition, Action<ICollection<ILine>> action)
    {
        action.Invoke(definition.Lines);
        return definition;
    }

    public static ICollection<ILine> Line(this ICollection<ILine> lines, string line)
    {
        lines.Add(new CodeLine(line));
        return lines;
    }

    public static ICollection<ILine> Line(this ICollection<ILine> lines, int tabs, string line)
    {
        lines.Add(new CodeLine(tabs, line));
        return lines;
    }

    public static ICollection<ILine> Comment(this ICollection<ILine> lines, string comment)
    {
        lines.Add(new CommentLine(comment));
        return lines;
    }

    public static ICollection<ILine> Comment(this ICollection<ILine> lines, int tabs, string comment)
    {
        lines.Add(new CommentLine(tabs, comment));
        return lines;
    }

    public static ICollection<ILine> Empty(this ICollection<ILine> lines)
    {
        lines.Add(new EmptyLine());
        return lines;
    }

    public static ICollection<ILine> Return(this ICollection<ILine> lines, string ret)
    {
        lines.Add(new ReturnLine(ret));
        return lines;
    }

    public static ICollection<ILine> Todo(this ICollection<ILine> lines, string todo)
    {
        lines.Add(new TodoLine(todo));
        return lines;
    }
}
