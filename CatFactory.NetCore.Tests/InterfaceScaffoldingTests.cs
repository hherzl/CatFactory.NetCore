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
        var definition = new CSharpInterfaceDefinition
        {
            Namespaces =
            {
                "System"
            },
            Namespace = "Infrastructure.Persistence",
            AccessModifier = AccessModifier.Public,
            Name = "IRepository",
            Implements =
            {
                "IDisposable"
            }
        };

        // Act
        CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, _solutionDirectory, _infrastructureDirectory), _persistenceDirectory, true, definition);
    }

    [Fact]
    public void ScaffoldingRepositoryInterface()
    {
        // Arrange
        var definition = new CSharpInterfaceDefinition
        {
            Namespaces =
            {
                "System",
                "System.Linq",
                "Domain.Entities"
            },
            Namespace = "Infrastructure.Persistence",
            Documentation = new Documentation("Contains all operations related to Northwind database access"),
            AccessModifier = AccessModifier.Public,
            Name = "INorthwindRepository",
            Implements =
            {
                "IRepository"
            },
            Methods =
            {
                new MethodDefinition("IQueryable<Product>", "GetProducts")
                {
                    Documentation = new Documentation("Retrieves all products"),
                    Parameters =
                    {
                        new ParameterDefinition("int?", "supplierID")
                    },
                },
                new MethodDefinition("IQueryable<Shipper>", "GetShippers"),
                new MethodDefinition("IQueryable<Order>", "GetOrders")
            }
        };

        // Act
        CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, _solutionDirectory, _infrastructureDirectory), _persistenceDirectory, true, definition);
    }
}
