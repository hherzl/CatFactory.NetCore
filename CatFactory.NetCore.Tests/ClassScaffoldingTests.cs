using CatFactory.CodeFactory;
using CatFactory.NetCore.CodeFactory;
using CatFactory.NetCore.ObjectOrientedProgramming;
using CatFactory.ObjectOrientedProgramming;
using Xunit;
using Xunit.Sdk;

namespace CatFactory.NetCore.Tests
{
    public class ClassScaffoldingTests
    {
        [Fact]
        public void ScaffoldingShipperClass()
        {
            // Arrange
            var definition = CSharpClassDefinition
                .Create(AccessModifier.Public, "Shipper", ns: "DesignPatterns")
                .ImportNs("System")
                .ImportNs("System.ComponentModel.DataAnnotations")
                .ImportNs("System.ComponentModel.DataAnnotations.Schema")
                ;

            definition.AddTableAnnotation("Shippers", schema: "dbo");

            definition.Properties.Add(
                CSharpClassDefinition.CreateAutomaticProperty("int?", "ShipperID").AddKeyAnnotation().AddDatabaseGeneratedAnnotation()
            );
            
            definition.Properties.Add(
                CSharpClassDefinition.CreateAutomaticProperty("string", "CompanyName").AddStringLengthAnnotation(80).AddRequiredAnnotation()
            );
            
            definition.Properties.Add(
                CSharpClassDefinition.CreateAutomaticProperty("String", "Phone").AddStringLengthAnnotation(48).AddRequiredAnnotation()
            );

            // Act
            CSharpCodeBuilder.CreateFiles(@"C:\Temp\CatFactory.NetCore\DesignPatterns", string.Empty, true, definition);
        }

        [Fact]
        public void ScaffoldingProductViewModelClass()
        {
            // Arrange
            var definition = CSharpClassDefinition
                .Create(AccessModifier.Public, "Product", ns: "DesignPatterns")
                .IsPartial()
                .Implement("INotifyPropertyChanged")
                .ImportNs("System")
                .ImportNs("System.ComponentModel")
                ;

            definition.Events = new()
            {
                new(AccessModifier.Public, "PropertyChangedEventHandler", "PropertyChanged")
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
            CSharpCodeBuilder.CreateFiles(@"C:\Temp\CatFactory.NetCore\DesignPatterns", string.Empty, true, definition);
        }

        [Fact]
        public void ScaffoldingOrderClass()
        {
            // Arrange
            var definition = CSharpClassDefinition
                .Create(AccessModifier.Public, "Order", ns: "DesignPatterns")
                .ImportNs("System")
                .ImportNs("System.ComponentModel.DataAnnotations")
                .ImportNs("System.ComponentModel.DataAnnotations.Schema")
                ;

            definition.AddTableAnnotation("Orders", schema: "dbo");

            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProperty("int?", "OrderID").AddKeyAnnotation().AddDatabaseGeneratedAnnotation());
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProperty("DateTime?", "OrderDate").AddColumnAnnotation());
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProperty("string", "CustomerID").AddColumnAnnotation().AddStringLengthAnnotation(5));
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProperty("int?", "ShipperID").AddColumnAnnotation());

            definition.SimplifyDataTypes();

            // Act
            CSharpCodeBuilder.CreateFiles(@"C:\Temp\CatFactory.NetCore\DesignPatterns", string.Empty, true, definition);
        }

        [Fact]
        public void ScaffoldingEntityExtensionsClass()
        {
            // Arrange
            var definition = CSharpClassDefinition
                .Create(AccessModifier.Public, "EntityExtensions", ns: "DesignPatterns")
                .IsStatic()
                .ImportNs("System")
                ;

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
            CSharpCodeBuilder.CreateFiles(@"C:\Temp\CatFactory.NetCore\DesignPatterns", string.Empty, true, definition);
        }

        [Fact]
        public void ScaffoldingCustOrderHistResultClass()
        {
            // Arrange
            var definition = CSharpClassDefinition
                .Create(AccessModifier.Public, "CustOrderHist", ns: "DesignPatterns")
                ;

            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProperty("string", "ProductName"));

            definition.Properties.Add(new(AccessModifier.Public, "int", "Total")
            {
                IsAutomatic = true,
                InitializationValue = "0"
            });

            // Act
            CSharpCodeBuilder.CreateFiles(@"C:\Temp\CatFactory.NetCore\DesignPatterns", string.Empty, true, definition);
        }

        [Fact]
        public void ScaffoldingDbContextClass()
        {
            // Arrange
            var definition = CSharpClassDefinition
                .Create(AccessModifier.Public, "NorthwindDbContext", ns: "DesignPatterns", baseClass: "DbContext")
                .SetDocumentation(summary: "Represents Northwind database in EF Core Model")
                .IsPartial()
                .ImportNs("System")
                .ImportNs("Microsoft.EntityFrameworkCore")
                ;

            definition.Constructors.Add(new(AccessModifier.Public, new ParameterDefinition("DbContextOptions<NorthwindDbContext>", "options")
            {
                Documentation = new("Instance of DbContext options")
            })
            {
                Invocation = "base(options)",
                Documentation = new("Initializes a new instance of NorthwindDbContext class")
            });

            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProperty("DbSet<Product>", "Products"));
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProperty("DbSet<Shipper>", "Shippers"));
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProperty("DbSet<Order>", "Orders"));

            definition.Methods.Add(new(AccessModifier.Protected, "void", "OnModelCreating", new ParameterDefinition("ModelBuilder", "modelBuilder"))
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
            });

            // Act
            CSharpCodeBuilder.CreateFiles(@"C:\Temp\CatFactory.NetCore\DesignPatterns", string.Empty, true, definition);
        }

        [Fact]
        public void ScaffoldingDbContextExtensionClass()
        {
            // Arrange
            var definition = CSharpClassDefinition
                .Create(AccessModifier.Public, "NorthwindDbContextExtensions", ns: "DesignPatterns")
                .IsPartial()
                .IsStatic()
                .ImportNs("System")
                .ImportNs("System.Collections.Generic")
                .ImportNs("System.Data.SqlClient")
                .ImportNs("System.Linq")
                .ImportNs("System.Threading.Tasks")
                .ImportNs("Microsoft.EntityFrameworkCore")
                ;

            definition.Methods.Add(new(AccessModifier.Public, "IQueryable<Product>", "GetProducts")
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
            });

            definition.Methods.Add(new(AccessModifier.Public, "Task<List<CustOrderHist>>", "GetCustOrderHistAsync")
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
            });

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
        public void ScaffoldingBaseRepositoryClass()
        {
            // Arrange
            var definition = CSharpClassDefinition
                .Create(AccessModifier.Public, "Repository", ns: "DesignPatterns")
                .IsPartial()
                .Implement("IRepository")
                .ImportNs("System")
                ;

            definition.Constructors.Add(new(AccessModifier.Public, new ParameterDefinition("NorthwindDbContext", "dbContext"))
            {
                Lines =
                {
                    new CodeLine("DbContext = dbContext;")
                }
            });

            definition.Properties.Add(CSharpClassDefinition.CreateReadonlyProperty("NorthwindDbContext", "DbContext", accessModifier: AccessModifier.Protected));
            
            definition.Methods.Add(new(AccessModifier.Public, "", "Dispose")
            {
                Lines =
                {
                    new TodoLine("Implement dispose for DbContext")
                }
            });

            // Act
            CSharpCodeBuilder.CreateFiles(@"C:\Temp\CatFactory.NetCore\DesignPatterns", string.Empty, true, definition);
        }

        [Fact]
        public void ScaffoldingRepositoryClass()
        {
            // Arrange
            var definition = CSharpClassDefinition
                .Create(AccessModifier.Public, "NorthwindRepository", ns: "DesignPatterns", baseClass: "Repository")
                .Implement("INorthwindRepository")
                .ImportNs("System")
                .ImportNs("System.Linq")
                .ImportNs("Microsoft.EntityFrameworkCore")
                ;

            definition.Constructors.Add(new(AccessModifier.Public, new ParameterDefinition("NorthwindDbContext", "dbContext"))
            {
                Invocation = "base(dbContext)"
            });

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
        public void ScaffoldingClassWithReadonlyFields()
        {
            // Arrange
            var definition = CSharpClassDefinition
                .Create(AccessModifier.Public, "Foo", ns: "DesignPatterns")
                .AddDefaultCtor()
                .ImportNs("System")
                ;

            definition.Fields.Add(new(AccessModifier.Public, "string", "Bar")
            {
                IsReadOnly = true,
                Value = "\"ABC\""
            });

            // Act
            CSharpCodeBuilder.CreateFiles(@"C:\Temp\CatFactory.NetCore\DesignPatterns", string.Empty, true, definition);
        }

        [Fact]
        public void ScaffoldingClassWithConstants()
        {
            // Arrange
            var definition = CSharpClassDefinition
                .Create(AccessModifier.Public, "Tokens", ns: "DesignPatterns")
                .IsStatic()
                .ImportNs("System")
                ;

            definition.Constants.Add(new ConstantDefinition(AccessModifier.Public, "int", "Foo", 1000));
            definition.Constants.Add(new ConstantDefinition(AccessModifier.Public, "string", "Bar", "ABC"));
            definition.Constants.Add(new ConstantDefinition(AccessModifier.Public, "bool", "Baz", true));

            // Act
            CSharpCodeBuilder.CreateFiles(@"C:\Temp\CatFactory.NetCore\DesignPatterns", string.Empty, true, definition);
        }

        [Fact]
        public void RefactInterfaceFromClass()
        {
            // Arrange
            var classDefinition = CSharpClassDefinition
                .Create(AccessModifier.Public, "UserViewModel", ns: "DesignPatterns")
                .ImportNs("System")
                .ImportNs("System.ComponentModel")
                ;

            classDefinition.AddViewModelProperty("Guid?", "Id");
            classDefinition.AddViewModelProperty("string", "GivenName");
            classDefinition.AddViewModelProperty("string", "MiddleName");
            classDefinition.AddViewModelProperty("string", "FamilyName");
            classDefinition.AddViewModelProperty("string", "Email");
            classDefinition.AddViewModelProperty("string", "UserName");

            classDefinition.AddPropertyWithField("DateTime?", "BirthDate");

            classDefinition.Methods.Add(new MethodDefinition(AccessModifier.Public, "void", "Foo"));
            classDefinition.Methods.Add(new MethodDefinition(AccessModifier.Private, "void", "Bar"));

            // Act
            var interfaceDefinition = classDefinition.RefactInterface();

            // Assert
            Assert.True(interfaceDefinition.Properties.Count == classDefinition.Properties.Count);
            Assert.True(interfaceDefinition.Methods.Count == classDefinition.Methods.Where(item => item.AccessModifier == AccessModifier.Public).Count());
        }
    }
}
