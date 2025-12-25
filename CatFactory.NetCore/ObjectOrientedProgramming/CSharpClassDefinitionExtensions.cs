using System.Linq;
using CatFactory.CodeFactory;
using CatFactory.Collections;
using CatFactory.NetCore.CodeFactory;
using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming;

public static class CSharpClassDefinitionExtensions
{
    public static CSharpClassDefinition UsingNs(this CSharpClassDefinition definition, string ns)
    {
        definition.Namespaces.AddUnique(ns);
        return definition;
    }

    public static CSharpClassDefinition UsingNs(this CSharpClassDefinition definition, params string[] ns)
    {
        foreach (var item in ns)
        {
            definition.Namespaces.AddUnique(item);
        }

        return definition;
    }

    public static CSharpClassDefinition Implement(this CSharpClassDefinition definition, string contract)
    {
        definition.Implements.AddUnique(contract);
        return definition;
    }

    public static CSharpClassDefinition AddDefaultCtor(this CSharpClassDefinition definition, AccessModifier accessModifier = AccessModifier.Public)
    {
        definition.Constructors.Add(new(accessModifier));
        return definition;
    }

    public static CSharpClassDefinition AddCtor(this CSharpClassDefinition definition, ClassConstructorDefinition ctor)
    {
        definition.Constructors.Add(ctor);
        return definition;
    }

    public static void AddPropWithField(this CSharpClassDefinition classDefinition, string type, string name, string fieldName = null, ICodeNamingConvention namingConvention = null)
    {
        namingConvention ??= new DotNetNamingConvention();

        if (string.IsNullOrEmpty(fieldName))
            fieldName = namingConvention.GetFieldName(name);

        var prop = new PropertyDefinition(AccessModifier.Public, type, name)
        {
            IsAutomatic = false,
            GetBody =
            {
                new ReturnLine("{0};", fieldName)
            },
            SetBody =
            {
                new CodeLine("{0} = value;", fieldName)
            }
        };

        classDefinition.Fields.Add(new(AccessModifier.Private, prop.Type, fieldName));

        classDefinition.Properties.Add(prop);
    }

    public static void AddViewModelProp(this CSharpClassDefinition classDefinition, string type, string name, string fieldName = null, ICodeNamingConvention namingConvention = null, bool useNullConditionalOperator = true)
    {
        namingConvention ??= new DotNetNamingConvention();

        if (string.IsNullOrEmpty(fieldName))
            fieldName = namingConvention.GetFieldName(name);

        var prop = new PropertyDefinition(AccessModifier.Public, type, name)
        {
            IsAutomatic = false,
            GetBody =
            {
                new ReturnLine("{0};", fieldName)
            }
        };

        prop.SetBody.Add(new CodeLine("if ({0} != value)", fieldName));
        prop.SetBody.Add(new CodeLine("{"));
        prop.SetBody.Add(new CodeLine(1, "{0} = value;", fieldName));
        prop.SetBody.Add(new CodeLine());

        if (useNullConditionalOperator)
        {
            prop.SetBody.Add(new CodeLine(1, "PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof({0})));", name));
        }
        else
        {
            prop.SetBody.Add(new CodeLine(1, "if (PropertyChanged != null)"));
            prop.SetBody.Add(new CodeLine(1, "{"));
            prop.SetBody.Add(new CodeLine(2, "PropertyChanged(this, new PropertyChangedEventArgs(nameof({0})));", name));
            prop.SetBody.Add(new CodeLine(1, "}"));
        }

        prop.SetBody.Add(new CodeLine("}"));

        classDefinition.Fields.Add(new(AccessModifier.Private, prop.Type, fieldName));

        classDefinition.Properties.Add(prop);
    }

    public static CSharpClassDefinition SetDocumentation(this CSharpClassDefinition definition, string summary = null, string remarks = null, string returns = null)
    {
        if (!string.IsNullOrEmpty(summary))
            definition.Documentation.Summary = summary;

        if (!string.IsNullOrEmpty(remarks))
            definition.Documentation.Remarks = remarks;

        if (!string.IsNullOrEmpty(returns))
            definition.Documentation.Returns = returns;

        return definition;
    }

    public static CSharpInterfaceDefinition RefactInterface(this CSharpClassDefinition classDefinition, ICodeNamingConvention namingConvention = null, params string[] exclusions)
    {
        namingConvention ??= new DotNetNamingConvention();

        var interfaceDefinition = new CSharpInterfaceDefinition
        {
            AccessModifier = classDefinition.AccessModifier,
            Namespaces = classDefinition.Namespaces,
            Name = namingConvention.GetInterfaceName(classDefinition.Name)
        };

        foreach (var @event in classDefinition.Events.Where(item => item.AccessModifier == AccessModifier.Public && !exclusions.Contains(item.Name)))
        {
            interfaceDefinition.Events.Add(new(@event.AccessModifier, @event.Type, @event.Name));
        }

        foreach (var prop in classDefinition.Properties.Where(item => item.AccessModifier == AccessModifier.Public && !exclusions.Contains(item.Name)))
        {
            interfaceDefinition.Properties.Add(new(prop.AccessModifier, prop.Type, prop.Name)
            {
                IsAutomatic = prop.IsAutomatic,
                IsReadOnly = prop.IsReadOnly
            });
        }

        foreach (var method in classDefinition.Methods.Where(item => item.AccessModifier == AccessModifier.Public && !exclusions.Contains(item.Name)))
        {
            interfaceDefinition.Methods.Add(new(method.AccessModifier, method.Type, method.Name, [.. method.Parameters]));
        }

        return interfaceDefinition;
    }
}
