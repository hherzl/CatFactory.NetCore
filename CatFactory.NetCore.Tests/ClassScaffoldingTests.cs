using CatFactory.CodeFactory;
using CatFactory.NetCore.CodeFactory;
using CatFactory.NetCore.ObjectOrientedProgramming;
using CatFactory.ObjectOrientedProgramming;
using Xunit;

namespace CatFactory.NetCore.Tests
{
    public class ClassScaffoldingTests
    {
        [Fact]
        public void TestScaffoldingShipperClass()
        {
            // Arrange
            var definition = new CSharpClassDefinition
            {
                Namespaces =
                {
                    "System",
                    "System.ComponentModel.DataAnnotations",
                    "System.ComponentModel.DataAnnotations.Schema"
                },
                Namespace = "DesignPatterns",
                AccessModifier = AccessModifier.Public,
                Name = "Shipper"
            };

            definition.Attributes.Add(new MetadataAttribute("Table", "\"Shippers\"")
            {
                Sets =
                {
                    new MetadataAttributeSet("Schema", "\"dbo\"")
                }
            });

            definition.Properties.Add(
                new PropertyDefinition(AccessModifier.Public, "int?", "ShipperID", new MetadataAttribute("Key"), new MetadataAttribute("DatabaseGenerated", "DatabaseGeneratedOption.Identity"))
                {
                    IsAutomatic = true
                }
            );

            definition.Properties.Add(
                new PropertyDefinition(AccessModifier.Public, "string", "CompanyName", new MetadataAttribute("Required"), new MetadataAttribute("StringLength", "80"))
                {
                    IsAutomatic = true
                }
            );

            definition.Properties.Add(
                new PropertyDefinition(AccessModifier.Public, "string", "Phone", new MetadataAttribute("Required"), new MetadataAttribute("StringLength", "48"))
                {
                    IsAutomatic = true
                }
            );

            // Act
            CSharpCodeBuilder.CreateFiles("C:\\Temp\\CatFactory.NetCore\\DesignPatterns", string.Empty, true, definition);
        }

        [Fact]
        public void TestScaffoldingProductViewModelClass()
        {
            // Arrange
            var definition = new CSharpClassDefinition
            {
                Namespaces =
                {
                    "System",
                    "System.ComponentModel"
                },
                Namespace = "DesignPatterns",
                AccessModifier = AccessModifier.Public,
                Name = "Product",
                Implements =
                {
                    "INotifyPropertyChanged"
                },
                Events =
                {
                    new EventDefinition("PropertyChangedEventHandler", "PropertyChanged")
                    {
                        AccessModifier = AccessModifier.Public
                    }
                }
            };

            definition.AddViewModelProperty("int?", "ProductID");
            definition.AddViewModelProperty("string", "ProductName");
            definition.AddViewModelProperty("int?", "SupplierID");
            definition.AddViewModelProperty("int?", "CategoryID");
            definition.AddViewModelProperty("string", "QuantityPerUnit");
            definition.AddViewModelProperty("decimal?", "UnitPrice");
            definition.AddViewModelProperty("short?", "UnitsInStock");
            definition.AddViewModelProperty("short?", "UnitsOnOrder");
            definition.AddViewModelProperty("short?", "ReorderLevel");
            definition.AddViewModelProperty("bool?", "Discontinued");

            // Act
            CSharpCodeBuilder.CreateFiles("C:\\Temp\\CatFactory.NetCore\\DesignPatterns", string.Empty, true, definition);
        }

        [Fact]
        public void TestScaffoldingOrderClass()
        {
            // Arrange
            var definition = new CSharpClassDefinition
            {
                Namespaces =
                {
                    "System",
                    "System.ComponentModel.DataAnnotations",
                    "System.ComponentModel.DataAnnotations.Schema"
                },
                Namespace = "DesignPatterns",
                AccessModifier = AccessModifier.Public,
                Name = "Order"
            };

            definition.Attributes.Add(new MetadataAttribute("Table", "\"Orders\"")
            {
                Sets =
                {
                    new MetadataAttributeSet("Schema", "\"dbo\"")
                }
            });

            definition.Properties.Add(new PropertyDefinition(AccessModifier.Public, "Int32?", "OrderID", new MetadataAttribute("Key"), new MetadataAttribute("DatabaseGenerated", "DatabaseGeneratedOption.Identity"))
            {
                IsAutomatic = true
            });

            definition.Properties.Add(new PropertyDefinition(AccessModifier.Public, "DateTime?", "OrderDate", new MetadataAttribute("Column"))
            {
                IsAutomatic = true
            });

            definition.Properties.Add(new PropertyDefinition(AccessModifier.Public, "String", "CustomerID", new MetadataAttribute("Column"), new MetadataAttribute("StringLength", "5"))
            {
                IsAutomatic = true
            });

            definition.Properties.Add(new PropertyDefinition(AccessModifier.Public, "Int32?", "ShipperID", new MetadataAttribute("Column"))
            {
                IsAutomatic = true
            });

            definition.SimplifyDataTypes();

            // Act
            CSharpCodeBuilder.CreateFiles("C:\\Temp\\CatFactory.NetCore\\DesignPatterns", string.Empty, true, definition);
        }

        [Fact]
        public void TestScaffoldingDbContextClass()
        {
            // Arrange
            var definition = new CSharpClassDefinition
            {
                Namespaces =
                {
                    "System",
                    "Microsoft.EntityFrameworkCore"
                },
                Documentation = new Documentation("Represents Northwind database in EF Core Model"),
                Namespace = "DesignPatterns",
                AccessModifier = AccessModifier.Public,
                IsPartial = true,
                Name = "NorthwindDbContext",
                BaseClass = "DbContext",
                Constructors =
                {
                    new ClassConstructorDefinition
                    {
                        AccessModifier = AccessModifier.Public,
                        Invocation = "base(options)",
                        Documentation = new Documentation("Initializes a new instance of NorthwindDbContext class"),
                        Parameters =
                        {
                            new ParameterDefinition("DbContextOptions<NorthwindDbContext>", "options")
                            {
                                Documentation = new Documentation("Instance of DbContext options")
                            }
                        }
                    }
                },
                Properties =
                {
                    new PropertyDefinition(AccessModifier.Public, "DbSet<Product>", "Products") { IsAutomatic = true },
                    new PropertyDefinition(AccessModifier.Public, "DbSet<Shipper>", "Shippers") { IsAutomatic = true },
                    new PropertyDefinition(AccessModifier.Public, "DbSet<Order>", "Orders") { IsAutomatic = true }
                },
                Methods =
                {
                    new MethodDefinition(AccessModifier.Protected, "void", "OnModelCreating", new ParameterDefinition("ModelBuilder", "modelBuilder"))
                    {
                        IsOverride = true,
                        Lines =
                        {
                            new CodeLine("modelBuilder.Entity<Product>(builder =>"),
                            new CodeLine("{"),
                            new CodeLine(1, "builder.ToTable(\"Products\", \"dbo\");"),
                            new CodeLine(),
                            new CodeLine(1, "builder.Property(p => p.ProductID).UseSqlServerIdentityColumn();"),
                            new CodeLine(),
                            new CodeLine(1, "builder.HasKey(p => p.ProductID);"),
                            new CodeLine("});")
                        }
                    }
                }
            };

            // Act
            CSharpCodeBuilder.CreateFiles("C:\\Temp\\CatFactory.NetCore\\DesignPatterns", string.Empty, true, definition);
        }

        [Fact]
        public void TestScaffoldingBaseRepositoryClass()
        {
            // Arrange
            var definition = new CSharpClassDefinition
            {
                Namespaces =
                {
                    "System"
                },
                Namespace = "DesignPatterns",
                AccessModifier = AccessModifier.Public,
                IsPartial = true,
                Name = "Repository",
                Implements =
                {
                    "IRepository"
                },
                Constructors =
                {
                    new ClassConstructorDefinition(AccessModifier.Public, new ParameterDefinition("NorthwindDbContext", "dbContext"))
                    {
                        Lines =
                        {
                            new CodeLine("DbContext = dbContext;")
                        }
                    }
                },
                Properties =
                {
                    new PropertyDefinition(AccessModifier.Protected, "NorthwindDbContext", "DbContext")
                    {
                        IsAutomatic = true,
                        IsReadOnly = true
                    }
                },
                Methods =
                {
                    new MethodDefinition(AccessModifier.Public, "void", "Dispose")
                    {
                        Lines =
                        {
                            new TodoLine("Implement dispose for DbContext")
                        }
                    }
                }
            };

            // Act
            CSharpCodeBuilder.CreateFiles("C:\\Temp\\CatFactory.NetCore\\DesignPatterns", string.Empty, true, definition);
        }

        [Fact]
        public void TestScaffoldingRepositoryClass()
        {
            // Arrange
            var definition = new CSharpClassDefinition
            {
                Namespaces =
                {
                    "System",
                    "System.Linq",
                    "Microsoft.EntityFrameworkCore"
                },
                Namespace = "DesignPatterns",
                AccessModifier = AccessModifier.Public,
                Name = "NorthwindRepository",
                BaseClass = "Repository",
                Implements =
                {
                    "INorthwindRepository"
                },
                Constructors =
                {
                    new ClassConstructorDefinition(AccessModifier.Public, new ParameterDefinition("NorthwindDbContext", "dbContext"))
                    {
                        Invocation = "base(dbContext)"
                    }
                }
            };

            definition.Methods.Add(new MethodDefinition(AccessModifier.Public, "IQueryable<Product>", "GetProducts")
            {
                Lines =
                {
                    new CodeLine("return DbContext.Products;")
                }
            });

            definition.Methods.Add(new MethodDefinition(AccessModifier.Public, "IQueryable<Shipper>", "GetShippers")
            {
                Lines =
                {
                    new CodeLine("return DbContext.Shippers;")
                }
            });

            definition.Methods.Add(new MethodDefinition(AccessModifier.Public, "IQueryable<Order>", "GetOrders")
            {
                Lines =
                {
                    new CodeLine("return DbContext.Orders;")
                }
            });

            // Act
            CSharpCodeBuilder.CreateFiles("C:\\Temp\\CatFactory.NetCore\\DesignPatterns", string.Empty, true, definition);
        }
    }
}
