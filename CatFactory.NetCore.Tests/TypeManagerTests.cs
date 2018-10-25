using CatFactory.NetCore.ObjectOrientedProgramming;
using CatFactory.ObjectOrientedProgramming;
using Xunit;

namespace CatFactory.NetCore.Tests
{
    public class TypeManagerTests
    {
        [Fact]
        public void TestGetClass()
        {
            var interfaceDefinition = new CSharpInterfaceDefinition
            {
                Namespace = "Entities",
                Name = "IEntity"
            };

            var classDefinition = new CSharpClassDefinition
            {
                Namespace = "Entities",
                Name = "OrderHeader",
                Implements =
                {
                    "Entities.IEntity"
                }
            };

            var classDef = TypeManager.GetItemByFullName(classDefinition.FullName);

            Assert.True(TypeManager.ObjectDefinitions.Count >0);
        }
    }
}
