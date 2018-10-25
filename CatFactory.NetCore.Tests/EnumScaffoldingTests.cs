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
                Documentation = new Documentation
                {
                    Summary = "Represents operation mode for persistance"
                },
                Sets =
                {
                    new NameValue { Name = "First", Value = "0" },
                    new NameValue { Name = "Second", Value = "1" },
                    new NameValue { Name = "Third", Value = "2" }
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
                    new NameValue { Name = "SunRoof", Value = "0x01" },
                    new NameValue { Name = "Spoiler", Value = "0x02" },
                    new NameValue { Name = "FogLights", Value = "0x04" },
                    new NameValue { Name = "TintedWindows", Value = "0x08" }
                }
            };

            // Act
            CSharpEnumBuilder.CreateFiles("C:\\Temp\\CatFactory.NetCore\\DesignPatterns", string.Empty, true, definition);
        }
    }
}
