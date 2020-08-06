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
                CSharpClassDefinition.CreateAutomaticProperty("Int32?", "ShipperID", attributes: new[]
                {
                    new MetadataAttribute("Key"),
                    new MetadataAttribute("DatabaseGenerated", "DatabaseGeneratedOption.Identity")
                })
            );

            definition.Properties.Add(
                CSharpClassDefinition.CreateAutomaticProperty("String", "CompanyName", attributes: new[]
                {
                    new MetadataAttribute("Required"),
                    new MetadataAttribute("StringLength", "80")
                })
            );

            definition.Properties.Add(
                CSharpClassDefinition.CreateAutomaticProperty("String", "Phone", attributes: new[]
                {
                    new MetadataAttribute("Required"),
                    new MetadataAttribute("StringLength", "48")
                })
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
                IsPartial = true,
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
        public void ScaffoldingOrderClass()
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

            definition.Properties.Add(
                CSharpClassDefinition.CreateAutomaticProperty("Int32?", "OrderID", attributes: new[]
                {
                    new MetadataAttribute("Key"),
                    new MetadataAttribute("DatabaseGenerated", "DatabaseGeneratedOption.Identity")
                })
            );

            definition.Properties.Add(
                CSharpClassDefinition.CreateAutomaticProperty("DateTime?", "OrderDate", attributes: new[] { new MetadataAttribute("Column") })
            );

            definition.Properties.Add(
                CSharpClassDefinition.CreateAutomaticProperty("String", "CustomerID", attributes: new[]
                {
                    new MetadataAttribute("Column"),
                    new MetadataAttribute("StringLength", "5")
                })
            );

            definition.Properties.Add(
                CSharpClassDefinition.CreateAutomaticProperty("Int32?", "ShipperID", attributes: new[] { new MetadataAttribute("Column") })
            );

            definition.SimplifyDataTypes();

            // Act
            CSharpCodeBuilder.CreateFiles("C:\\Temp\\CatFactory.NetCore\\DesignPatterns", string.Empty, true, definition);
        }

        [Fact]
        public void ScaffoldingEntityExtensionsClass()
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
                IsStatic = true,
                Name = "EntityExtensions"
            };

            definition.Methods.Add(new MethodDefinition
            {
                AccessModifier = AccessModifier.Public,
                IsStatic = true,
                IsExtension = true,
                Type = "string",
                Name = "ToJson",
                Parameters =
                {
                    new ParameterDefinition("Order", "entity")
                },
                Lines =
                {
                    new CodeLine("return string.Empty;")
                }
            });

            definition.SimplifyDataTypes();

            // Act
            CSharpCodeBuilder.CreateFiles("C:\\Temp\\CatFactory.NetCore\\DesignPatterns", string.Empty, true, definition);
        }

        [Fact]
        public void TestScaffoldingCustOrderHistResultClass()
        {
            // Arrange
            var definition = new CSharpClassDefinition
            {
                Namespace = "DesignPatterns",
                AccessModifier = AccessModifier.Public,
                Name = "CustOrderHist"
            };

            definition.Properties.Add(new PropertyDefinition(AccessModifier.Public, "string", "ProductName")
            {
                IsAutomatic = true
            });

            definition.Properties.Add(new PropertyDefinition(AccessModifier.Public, "int", "Total")
            {
                IsAutomatic = true,
                InitializationValue = "0"
            });

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
                    CSharpClassDefinition.CreateAutomaticProperty("DbSet<Product>", "Products"),
                    CSharpClassDefinition.CreateAutomaticProperty("DbSet<Shipper>", "Shippers"),
                    CSharpClassDefinition.CreateAutomaticProperty("DbSet<Order>", "Orders")
                },
                Methods =
                {
                    new MethodDefinition(AccessModifier.Protected, "", "OnModelCreating", new ParameterDefinition("ModelBuilder", "modelBuilder"))
                    {
                        IsOverride = true,
                        Lines =
                        {
                            new CodeLine("modelBuilder.Entity<Product>(builder =>"),
                            new CodeLine("{"),
                            new CommentLine(1, " Add configuration for 'dbo.Products' table"),
                            new CodeLine(),
                            new CodeLine(1, "builder.ToTable(\"Products\", \"dbo\");"),
                            new CodeLine(),
                            new CodeLine(1, "builder.Property(p => p.ProductID).UseSqlServerIdentityColumn();"),
                            new CodeLine(),
                            new CodeLine(1, "builder.HasKey(p => p.ProductID);"),
                            new CodeLine("});"),
                            new CodeLine(),
                            new CommentLine(" Register results for stored procedures"),
                            new CodeLine(),
                            new CommentLine(" 'dbo.CustOrderHist'"),
                            new CodeLine("modelBuilder.Query<CustOrderHist>();")
                        }
                    }
                }
            };

            // Act
            CSharpCodeBuilder.CreateFiles("C:\\Temp\\CatFactory.NetCore\\DesignPatterns", string.Empty, true, definition);
        }

        [Fact]
        public void TestScaffoldingDbContextExtensionClass()
        {
            // Arrange
            var definition = new CSharpClassDefinition
            {
                Namespaces =
                {
                    "System",
                    "System.Collections.Generic",
                    "System.Data.SqlClient",
                    "System.Linq",
                    "System.Threading.Tasks",
                    "Microsoft.EntityFrameworkCore"
                },
                Namespace = "DesignPatterns",
                AccessModifier = AccessModifier.Public,
                IsPartial = true,
                IsStatic = true,
                Name = "NorthwindDbContextExtensions",
                Methods =
                {
                    new MethodDefinition(AccessModifier.Public, "IQueryable<Product>", "GetProducts")
                    {
                        IsStatic = true,
                        IsExtension = true,
                        Parameters =
                        {
                            new ParameterDefinition("NorthwindDbContext", "dbContext"),
                            new ParameterDefinition("int?", "supplierID")
                        },
                        Lines =
                        {
                            new CodeLine("var query = dbContext.Products.AsQueryable();"),
                            new CodeLine(),
                            new CodeLine("if (supplierID.HasValue)"),
                            new CodeLine(1, "query = query.Where(item => item.SupplierID == supplierID);"),
                            new CodeLine(),
                            new ReturnLine("query;")
                        }
                    },
                    new MethodDefinition(AccessModifier.Public, "Task<List<CustOrderHist>>", "GetCustOrderHistAsync")
                    {
                        IsStatic = true,
                        IsAsync = true,
                        IsExtension = true,
                        Parameters =
                        {
                            new ParameterDefinition("NorthwindDbContext", "dbContext"),
                            new ParameterDefinition("string", "customerID")
                        },
                        Lines =
                        {
                            new CodeLine("var query = new"),
                            new CodeLine("{"),
                            new CodeLine(1, "Text = \" exec [dbo].[CustOrderHist] @CustomerID \","),
                            new CodeLine(1, "Parameters = new[]"),
                            new CodeLine(1, "{"),
                            new CodeLine(2, "new SqlParameter(\"@CustomerID\", customerID)"),
                            new CodeLine(1, "}"),
                            new CodeLine("};"),
                            new CodeLine(),
                            new ReturnLine("await dbContext"),
                            new CodeLine(1, ".Query<CustOrderHist>()"),
                            new CodeLine(1, ".FromSql(query.Text, query.Parameters)"),
                            new CodeLine(1, ".ToListAsync();")
                        }
                    }
                }
            };

            // Act


            var codeBuilder = new CSharpClassBuilder
            {
                OutputDirectory = @"C:\Temp\CatFactory.NetCore\DesignPatterns",
                ForceOverwrite = true,
                ObjectDefinition = definition,
                AddNamespacesAtStart = false
            };

            codeBuilder.CreateFile();
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
                    CSharpClassDefinition.CreateReadonlyProperty("NorthwindDbContext", "DbContext", accessModifier: AccessModifier.Protected),
                },
                Methods =
                {
                    new MethodDefinition(AccessModifier.Public, "", "Dispose")
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
                Parameters =
                {
                    new ParameterDefinition("int?", "supplierID")
                },
                Lines =
                {
                    new CodeLine("var query = DbContext.Products.AsQueryable();"),
                    new CodeLine(),
                    new CodeLine("if (supplierID.HasValue)"),
                    new CodeLine(1, "query = query.Where(item => item.SupplierID == supplierID);"),
                    new CodeLine(),
                    new ReturnLine("query;")
                }
            });

            definition.Methods.Add(new MethodDefinition(AccessModifier.Public, "IQueryable<Shipper>", "GetShippers")
            {
                Lines =
                {
                    new ReturnLine("DbContext.Shippers;")
                }
            });

            definition.Methods.Add(new MethodDefinition(AccessModifier.Public, "IQueryable<Order>", "GetOrders")
            {
                Lines =
                {
                    new ReturnLine("DbContext.Orders;")
                }
            });

            // Act
            //CSharpCodeBuilder.CreateFiles("C:\\Temp\\CatFactory.NetCore\\DesignPatterns", string.Empty, true, definition);

            var codeBuilder = new CSharpClassBuilder
            {
                OutputDirectory = @"C:\Temp\CatFactory.NetCore\DesignPatterns",
                ForceOverwrite = true,
                ObjectDefinition = definition,
                AddNamespacesAtStart = false
            };

            codeBuilder.CreateFile();
        }

        [Fact]
        public void TestScaffoldingClassWithOnlyFields()
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
                Name = "Foo",
                Fields =
                {
                    new FieldDefinition(AccessModifier.Public, "string", "Bar")
                    {
                        IsReadOnly = true,
                        Value = "\"ABC\""
                    }
                },
                Constructors =
                {
                    new ClassConstructorDefinition()
                }
            };

            // Act
            CSharpCodeBuilder.CreateFiles("C:\\Temp\\CatFactory.NetCore\\DesignPatterns", string.Empty, true, definition);
        }
    }
}
