using CatFactory.NetCore.CodeFactory;
using CatFactory.NetCore.ObjectOrientedProgramming;
using CatFactory.ObjectOrientedProgramming;
using Xunit;

namespace CatFactory.NetCore.Tests;

public class InterfaceScaffoldingTests : ScaffoldingTest
{
    public InterfaceScaffoldingTests()
        : base()
    {
    }

    [Fact]
    public void ScaffoldingBaseRepositoryInterface()
    {
        // Arrange
        var definition = CSharpInterfaceDefinition.Create(AccessModifier.Public, "IRepository", ns: "Infrastructure.Persistence")
            .Implement("IDisposable")
            .UsingNs("System")
            ;

        // Act
        CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, _solutionDirectory, _infrastructureDirectory), _persistenceDirectory, true, definition);
    }

    [Fact]
    public void ScaffoldingRepositoryInterface()
    {
        // Arrange
        var definition = CSharpInterfaceDefinition.Create(AccessModifier.Public, "INorthwindRepository", ns: "Infrastructure.Persistence")
            .UsingNs("System")
            .UsingNs("System.Linq")
            .UsingNs("Domain.Entities")
            .Implement("IRepository")
            .SetDocumentation(summary: "Contains all operations related to Northwind database access")
            ;

        definition.Methods.Add(CSharpMethodDefinition.Create("IQueryable<Product>", "GetProducts").SetDocumentation(summary: "Retrieves all products").AddParam("int?", "supplierID"));
        definition.Methods.Add(CSharpMethodDefinition.Create("IQueryable<Shipper>", "GetShippers"));
        definition.Methods.Add(CSharpMethodDefinition.Create("IQueryable<Order>", "GetOrders"));

        // Act
        CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, _solutionDirectory, _infrastructureDirectory), _persistenceDirectory, true, definition);
    }
}
