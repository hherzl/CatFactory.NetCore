using CatFactory.NetCore.ObjectOrientedProgramming;
using CatFactory.NetCore.Tests.Models;
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
            .AddDefaultCtor();

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
            .UsingNs("Microsoft.Data.SqlClient");

        // Assert
        Assert.Single(definition.Namespaces);
        Assert.Equal("Helpers", definition.Namespace);
        Assert.Equal("DbHelper", definition.Name);
    }

    [Fact]
    public void CreateWithGeneric_QueryModel_ReturnsClassDefinition()
    {
        // Arrange and Act
        var definition = CSharpClassDefinition
            .Create<FooClassDefinition>(AccessModifier.Public, "PersonQueryModel", "QueryModels");

        definition.AddAutomaticProperty("int?", "Id");
        definition.AddAutomaticProperty("string", "GivenName");
        definition.AddAutomaticProperty("string", "MiddleName");
        definition.AddAutomaticProperty("string", "FamilyName");

        // Assert
        Assert.Equal("PersonQueryModel", definition.Name);
        Assert.Equal("QueryModels", definition.Namespace);
    }

    [Fact]
    public void CreateWithGeneric_CommandModel_ReturnsClassDefinition()
    {
        // Arrange and Act
        var definition = CSharpClassDefinition
            .Create<FooClassDefinition>(AccessModifier.Public, "CreateOrderCommand", "Commands");

        definition.AddAutomaticProperty("string", "CustomerId");
        definition.AddAutomaticProperty("string", "OrderNumber");
        definition.AddAutomaticProperty("DateTime?", "OrderDate");
        definition.AddAutomaticProperty("decimal?", "OrderTotal");

        // Assert
        Assert.Equal("CreateOrderCommand", definition.Name);
        Assert.Equal("Commands", definition.Namespace);
    }

    [Fact]
    public void CreateWithGeneric_ServiceClass_ReturnsClassDefinition()
    {
        // Arrange and Act
        var definition = CSharpClassDefinition
            .Create<FooClassDefinition>(AccessModifier.Public, "InvoiceHandler", "Services")
            .AddDefaultCtor()
            .Add(new MethodDefinition(AccessModifier.Public, "Task", "HandleAsync")
                .AddParam("CreateInvoiceRequest", "request")
                .AddParam("CancellationToken", "ct")
                .Set(body => body.Line("return Task.CompletedTask;"))
            );

        // Assert
        Assert.Equal("InvoiceHandler", definition.Name);
        Assert.Equal("Services", definition.Namespace);
        Assert.Single(definition.Methods);
    }
}
