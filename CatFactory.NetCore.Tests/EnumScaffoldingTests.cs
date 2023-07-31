using CatFactory.NetCore.CodeFactory;
using CatFactory.NetCore.ObjectOrientedProgramming;
using CatFactory.ObjectOrientedProgramming;
using Xunit;

namespace CatFactory.NetCore.Tests
{
    public class EnumScaffoldingTests : ScaffoldingTest
    {
        public EnumScaffoldingTests()
            : base()
        {
        }

        [Fact]
        public void ScafflodOrderStatusEnum()
        {
            // Arrange
            var definition = new CSharpEnumDefinition
            {
                Namespace = "Domain.Enums",
                AccessModifier = AccessModifier.Public,
                Name = "OrderStatus",
                BaseType = "short",
                Documentation = new("Represents status for Orders"),
                Sets =
                {
                    new NameValue("Created", "0"),
                    new NameValue("Processed", "100"),
                    new NameValue("Delivered", "200"),
                    new NameValue("Cancelled", "300")
                }
            };

            // Act
            CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, _solutionDirectory, _domainDirectory), _enumsDirectory, true, definition);
        }

        [Fact]
        public void ScaffoldEnumWithFlags()
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
                AccessModifier = AccessModifier.Public,
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
            // Act
            CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, "DesignPatterns"), string.Empty, true, definition);
        }
    }
}
