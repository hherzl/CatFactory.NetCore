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
            .ImportNs("Microsoft.AspNetCore", "Microsoft.EntityFrameworkCore")
            .AddDefaultCtor()
            ;

        // Assert
        Assert.True(definition.Namespaces.Count == 2);
        Assert.True(definition.Namespace == "ShanghaiCat.API.Controllers");
        Assert.True(definition.Name == "EmployeeController");
        Assert.True(definition.BaseClass == "ControllerBase");
    }

    [Fact]
    public void Create_DbHelper_ReturnsClassDefinition()
    {
        // Arrange and Act
        var definition = CSharpClassDefinition
            .Create(AccessModifier.Public, "DbHelper", "Helpers", isStatic: true)
            .ImportNs("Microsoft.Data.SqlClient")
            ;

        // Assert
        Assert.True(definition.Namespaces.Count == 1);
        Assert.True(definition.Namespace == "Helpers");
        Assert.True(definition.Name == "DbHelper");
    }
}
