using CatFactory.NetCore.CodeFactory;
using CatFactory.NetCore.ObjectOrientedProgramming;
using CatFactory.ObjectOrientedProgramming;
using Xunit;

namespace CatFactory.NetCore.Tests
{
    public class InterfaceScaffoldingTests
    {
        [Fact]
        public void TestScaffoldingBaseRepositoryInterface()
        {
            // Arrange
            var definition = new CSharpInterfaceDefinition
            {
                Namespaces =
                {
                    "System"
                },
                Namespace = "DesignPatterns",
                Name = "IRepository",
                Implements =
                {
                    "IDisposable"
                }
            };

            // Act
            CSharpCodeBuilder.CreateFiles("C:\\Temp\\CatFactory.NetCore\\DesignPatterns", string.Empty, true, definition);
        }

        [Fact]
        public void TestScaffoldingRepositoryInterface()
        {
            // Arrange
            var definition = new CSharpInterfaceDefinition
            {
                Namespaces =
                {
                    "System",
                    "System.Linq"
                },
                Namespace = "DesignPatterns",
                Documentation = new Documentation
                {
                    Summary = "Contains all operations related to Northwind database access"
                },
                Name = "INorthwindRepository",
                Implements =
                {
                    "IRepository"
                },
                Methods =
                {
                    new MethodDefinition("IQueryable<Product>", "GetProducts")
                    {
                        Documentation = new Documentation
                        {
                            Summary = "Retrieves all products"
                        }
                    },
                    new MethodDefinition("IQueryable<Shipper>", "GetShippers"),
                    new MethodDefinition("IQueryable<Order>", "GetOrders")
                }
            };

            // Act
            CSharpCodeBuilder.CreateFiles("C:\\Temp\\CatFactory.NetCore\\DesignPatterns", string.Empty, true, definition);
        }
    }
}
