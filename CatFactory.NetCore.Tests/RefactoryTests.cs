using CatFactory.NetCore.ObjectOrientedProgramming;
using CatFactory.ObjectOrientedProgramming;
using Xunit;

namespace CatFactory.NetCore.Tests;

public class RefactoryTests
{
    [Fact]
    public void RefactInterfaceFromClass()
    {
        // Arrange
        var classDefinition = CSharpClassDefinition
            .Create(AccessModifier.Public, "UserViewModel", ns: "DesignPatterns")
            .ImportNs("System")
            .ImportNs("System.ComponentModel")
            ;

        classDefinition.AddViewModelProp("Guid?", "Id");
        classDefinition.AddViewModelProp("string", "GivenName");
        classDefinition.AddViewModelProp("string", "MiddleName");
        classDefinition.AddViewModelProp("string", "FamilyName");
        classDefinition.AddViewModelProp("string", "Email");
        classDefinition.AddViewModelProp("string", "UserName");

        classDefinition.AddPropWithField("DateTime?", "BirthDate");

        classDefinition.Methods.Add(new MethodDefinition(AccessModifier.Public, "void", "Foo"));
        classDefinition.Methods.Add(new MethodDefinition(AccessModifier.Private, "void", "Bar"));

        // Act
        var interfaceDefinition = classDefinition.RefactInterface();

        // Assert
        Assert.True(interfaceDefinition.Properties.Count == classDefinition.Properties.Count);
        Assert.True(interfaceDefinition.Methods.Count == classDefinition.Methods.Where(item => item.AccessModifier == AccessModifier.Public).Count());
    }
}
