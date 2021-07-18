using CatFactory.NetCore.ObjectOrientedProgramming;
using CatFactory.ObjectOrientedProgramming;
using Xunit;

namespace CatFactory.NetCore.Tests
{
    public class TypeManagerTests
    {
        [Fact]
        public void GetClassFromTypeManager()
        {
            // Act
            var interfaceDefinition = new CSharpInterfaceDefinition
            {
                Namespace = "Entities",
                AccessModifier = AccessModifier.Public,
                Name = "IEntity"
            };

            var classDefinition = new CSharpClassDefinition
            {
                Namespace = "Entities",
                AccessModifier = AccessModifier.Public,
                Name = "OrderHeader",
                Implements =
                {
                    "Entities.IEntity"
                }
            };

            // Arrange
            var classDef = TypeManager.GetItemByFullName(classDefinition.FullName);

            // Assert
            Assert.True(TypeManager.ObjectDefinitions.Count > 0);
            Assert.True(classDef != null);
        }

        [Fact]
        public void GetInterfaceFromTypeManager()
        {
            // Act
            var interfaceDefinition = new CSharpInterfaceDefinition
            {
                Namespace = "Entities",
                AccessModifier = AccessModifier.Public,
                Name = "IEntity"
            };

            var classDefinition = new CSharpClassDefinition
            {
                Namespace = "Entities",
                AccessModifier = AccessModifier.Public,
                Name = "OrderHeader",
                Implements =
                {
                    "Entities.IEntity"
                }
            };

            // Arrange
            var interfaceDef = TypeManager.GetItemByFullName(classDefinition.FullName);

            // Assert
            Assert.True(TypeManager.ObjectDefinitions.Count > 0);
            Assert.True(interfaceDef != null);
        }
    }
}
