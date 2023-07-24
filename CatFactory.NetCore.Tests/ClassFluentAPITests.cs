using CatFactory.NetCore.ObjectOrientedProgramming;
using CatFactory.ObjectOrientedProgramming;
using Xunit;

namespace CatFactory.NetCore.Tests;

public class ClassFluentAPITests
{
    [Fact]
    public void Test_CreateClass()
    {
        // Arrange and Act
        var definition = CSharpClassDefinition
            .New(AccessModifier.Public, "EmployeeController", "ShanghaiCat.API.Controllers", baseClass: "ControllerBase")
            .AddNs("Microsoft.AspNetCore")
            ;

        // Assert
        Assert.True(definition.Namespaces.Count == 1);
        Assert.True(definition.Namespace == "ShanghaiCat.API.Controllers");
        Assert.True(definition.Name == "EmployeeController");
        Assert.True(definition.BaseClass == "ControllerBase");
    }
}
