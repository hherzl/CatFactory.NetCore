﻿using CatFactory.NetCore.CodeFactory;
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
                AccessModifier = AccessModifier.Public,
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
            CSharpCodeBuilder.CreateFiles("C:\\Temp\\CatFactory.NetCore\\DesignPatterns", string.Empty, true, definition);
        }
    }
}
