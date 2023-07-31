using CatFactory.NetCore.ObjectOrientedProgramming;
using CatFactory.ObjectOrientedProgramming;
using Xunit;

namespace CatFactory.NetCore.Tests
{
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

            classDefinition.AddViewModelProperty("Guid?", "Id");
            classDefinition.AddViewModelProperty("string", "GivenName");
            classDefinition.AddViewModelProperty("string", "MiddleName");
            classDefinition.AddViewModelProperty("string", "FamilyName");
            classDefinition.AddViewModelProperty("string", "Email");
            classDefinition.AddViewModelProperty("string", "UserName");

            classDefinition.AddPropertyWithField("DateTime?", "BirthDate");

            classDefinition.Methods.Add(new MethodDefinition(AccessModifier.Public, "void", "Foo"));
            classDefinition.Methods.Add(new MethodDefinition(AccessModifier.Private, "void", "Bar"));

            // Act
            var interfaceDefinition = classDefinition.RefactInterface();

            // Assert
            Assert.True(interfaceDefinition.Properties.Count == classDefinition.Properties.Count);
            Assert.True(interfaceDefinition.Methods.Count == classDefinition.Methods.Where(item => item.AccessModifier == AccessModifier.Public).Count());
        }
    }
}
