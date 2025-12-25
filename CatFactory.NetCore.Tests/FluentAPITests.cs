using CatFactory.NetCore.ObjectOrientedProgramming;
using CatFactory.ObjectOrientedProgramming;
using Xunit;

namespace CatFactory.NetCore.Tests;

public class FluentAPITests
{
    [Fact]
    public void Create_Controller_ReturnsClassDefinition()
    {
        // Arrange and Act
        var definition = CSharpClassDefinition
            .Create(AccessModifier.Public, "EmployeeController", "ShanghaiCat.API.Controllers", baseClass: "ControllerBase")
            .UsingNs("Microsoft.AspNetCore", "Microsoft.EntityFrameworkCore")
            .AddDefaultCtor()
            ;

        // Assert
        Assert.Equal(2, definition.Namespaces.Count);
        Assert.Equal("ShanghaiCat.API.Controllers", definition.Namespace);
        Assert.Equal("EmployeeController", definition.Name);
        Assert.Equal("ControllerBase", definition.BaseClass);
    }

    [Fact]
    public void Create_DbHelper_ReturnsClassDefinition()
    {
        // Arrange and Act
        var definition = CSharpClassDefinition
            .Create(AccessModifier.Public, "DbHelper", "Helpers", isStatic: true)
            .UsingNs("Microsoft.Data.SqlClient")
            ;

        // Assert
        Assert.Single(definition.Namespaces);
        Assert.Equal("Helpers", definition.Namespace);
        Assert.Equal("DbHelper", definition.Name);
    }
}
