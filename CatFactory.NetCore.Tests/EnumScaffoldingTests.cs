using CatFactory.NetCore.CodeFactory;
using CatFactory.NetCore.ObjectOrientedProgramming;
using CatFactory.ObjectOrientedProgramming;
using Xunit;

namespace CatFactory.NetCore.Tests
{
    public class EnumScaffoldingTests
    {
        [Fact]
        public void TestSimpleEnumGeneration()
        {
            // Arrange
            var definition = new CSharpEnumDefinition
            {
                Namespace = "DesignPatterns",
                Name = "OperationMode",
                Documentation = new Documentation("Represents operation mode for persistance"),
                Sets =
                {
                    new NameValue("First", "0"),
                    new NameValue("Second", "1"),
                    new NameValue("Third", "2")
                }
            };

            // Act
            CSharpEnumBuilder.CreateFiles("C:\\Temp\\CatFactory.NetCore\\DesignPatterns", string.Empty, true, definition);
        }

        [Fact]
        public void TestEnumWithFlagsGeneration()
        {
            // Arrange
            var definition = new CSharpEnumDefinition
            {
                Namespaces =
                {
                    "System"
                },
                Attributes =
                {
                    new MetadataAttribute("Flags")
                },
                Name = "CarOptions",
                Sets =
                {
                    new NameValue("SunRoof", "0x01"),
                    new NameValue("Spoiler", "0x02"),
                    new NameValue("FogLights", "0x04"),
                    new NameValue("TintedWindows", "0x08")
                }
            };

            // Act
            CSharpEnumBuilder.CreateFiles("C:\\Temp\\CatFactory.NetCore\\DesignPatterns", string.Empty, true, definition);
        }
    }
}
