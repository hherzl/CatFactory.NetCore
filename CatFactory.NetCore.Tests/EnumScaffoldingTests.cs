using CatFactory.NetCore.CodeFactory;
using CatFactory.NetCore.ObjectOrientedProgramming;
using CatFactory.ObjectOrientedProgramming;
using Xunit;

namespace CatFactory.NetCore.Tests;

public class EnumScaffoldingTests : ScaffoldingTest
{
    public EnumScaffoldingTests()
        : base()
    {
    }

    [Fact]
    public void Scafflod_OrderStatus_ReturnsEnum()
    {
        // Arrange
        var definition = CSharpEnumDefinition.Create(AccessModifier.Public, "OrderStatus", baseType: "short", ns: "Domain.Enums")
            .SetDocumentation(summary: "Represents status for Orders")
            .AddSet("Created", 0)
            .AddSet("Processed", 100)
            .AddSet("Delivered", 200)
            .AddSet("Cancelled", 300)
            ;

        // Act
        CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, _solutionDirectory, _domainDirectory, _enumsDirectory), true, definition);
    }

    [Fact]
    public void Scaffold_CarOptions_ReturnsEnumWithFlags()
    {
        // Arrange
        var definition = CSharpEnumDefinition.Create(AccessModifier.Public, "CarOptions", baseType: "short", ns: "Domain.Enums")
            .AddAttrib("Flags")
            .SetDocumentation(summary: "Represents status for Orders")
            .AddSet("SunRoof", 0x01)
            .AddSet("Spoiler", 0x02)
            .AddSet("FogLights", 0x04)
            .AddSet("TintedWindows", 0x08)
            ;

        // Act
        CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, "DesignPatterns"), true, definition);
    }
}
