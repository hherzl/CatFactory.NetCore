using CatFactory.CodeFactory;
using CatFactory.NetCore.CodeFactory;
using CatFactory.NetCore.ObjectOrientedProgramming;
using CatFactory.ObjectOrientedProgramming;
using Xunit;

namespace CatFactory.NetCore.Tests
{
    public class ClassScaffoldingTests : ScaffoldingTest
    {
        public ClassScaffoldingTests()
            : base()
        {
        }

        [Fact]
        public void ScaffoldingShipperClass()
        {
            // Arrange
            var definition = CSharpClassDefinition
                .Create(AccessModifier.Public, "Shipper", ns: "Domain.Entities")
                .ImportNs("System")
                .ImportNs("System.ComponentModel.DataAnnotations")
                .ImportNs("System.ComponentModel.DataAnnotations.Schema")
                ;

            definition.AddTableAttrib("Shippers", schema: "dbo");

            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("int?", "ShipperID").AddKeyAttrib().AddDatabaseGeneratedAttrib());
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("string", "CompanyName").AddStringLengthAttrib(80).AddRequiredAttrib());
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("string", "Phone").AddStringLengthAttrib(48).AddRequiredAttrib());

            // Act
            CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, _solutionDirectory, _domainDirectory), _entitiesDirectory, true, definition);
        }

        [Fact]
        public void ScaffoldingProductViewModelClass()
        {
            // Arrange
            var definition = CSharpClassDefinition
                .Create(AccessModifier.Public, "Product", ns: "Domain.Entities")
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
            CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, _solutionDirectory, _domainDirectory), _entitiesDirectory, true, definition);
        }

        [Fact]
        public void ScaffoldingSupplierClass()
        {
            // Arrange
            var definition = CSharpClassDefinition
                .Create(AccessModifier.Public, "Supplier", ns: "Domain.Entities")
                .ImportNs("System")
                .ImportNs("System.ComponentModel.DataAnnotations")
                .ImportNs("System.ComponentModel.DataAnnotations.Schema")
                ;

            definition.AddTableAttrib("Suppliers", schema: "dbo");

            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("int?", "SupplierID").AddKeyAttrib().AddDatabaseGeneratedAttrib());
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("string", "CompanyName").AddColumnAttrib().AddRequiredAttrib().AddStringLengthAttrib(40));

            definition.SimplifyDataTypes();

            // Act
            CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, _solutionDirectory, _domainDirectory), _entitiesDirectory, true, definition);
        }

        [Fact]
        public void ScaffoldingCategoryClass()
        {
            // Arrange
            var definition = CSharpClassDefinition
                .Create(AccessModifier.Public, "Category", ns: "Domain.Entities")
                .ImportNs("System")
                .ImportNs("System.ComponentModel.DataAnnotations")
                .ImportNs("System.ComponentModel.DataAnnotations.Schema")
                ;

            definition.AddTableAttrib("Categories", schema: "dbo");

            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("int?", "CategoryID").AddKeyAttrib().AddDatabaseGeneratedAttrib());
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("string", "CategoryName").AddColumnAttrib().AddRequiredAttrib().AddStringLengthAttrib(15));
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("string", "Description").AddColumnAttrib());

            definition.SimplifyDataTypes();

            // Act
            CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, _solutionDirectory, _domainDirectory), _entitiesDirectory, true, definition);
        }

        [Fact]
        public void ScaffoldingOrderClass()
        {
            // Arrange
            var definition = CSharpClassDefinition
                .Create(AccessModifier.Public, "Order", ns: "Domain.Entities")
                .ImportNs("System")
                .ImportNs("System.ComponentModel.DataAnnotations")
                .ImportNs("System.ComponentModel.DataAnnotations.Schema")
                ;

            definition.AddTableAttrib("Orders", schema: "dbo");

            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("int?", "OrderID").AddKeyAttrib().AddDatabaseGeneratedAttrib());
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("string", "CustomerID").AddColumnAttrib().AddStringLengthAttrib(5));
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("int?", "EmployeeID").AddColumnAttrib());
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("DateTime?", "OrderDate").AddColumnAttrib());
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("DateTime?", "RequiredDate").AddColumnAttrib());
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("DateTime?", "ShippedDate").AddColumnAttrib());
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("int?", "ShipVia").AddColumnAttrib());
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("decimal?", "Freight").AddColumnAttrib());

            definition.SimplifyDataTypes();

            // Act
            CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, _solutionDirectory, _domainDirectory), _entitiesDirectory, true, definition);
        }

        [Fact]
        public void ScaffoldingNorthwindExceptionClass()
        {
            // Arrange
            var definition = CSharpClassDefinition
                .Create(AccessModifier.Public, "NorthwindException", ns: "Domain.Exceptions", baseClass: "Exception")
                .ImportNs("System")
                .AddCtor(CSharpClassDefinition.CreateCtor(invocation: "base()"))
                .AddCtor(CSharpClassDefinition.CreateCtor(invocation: "base(message)").AddParam("string", "message"))
                .AddCtor(CSharpClassDefinition.CreateCtor(invocation: "base(message, innerException)").AddParam("string", "message").AddParam("Exception", "innerException"))
                ;

            // Act
            CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, _solutionDirectory, _domainDirectory), _exceptionsDirectory, true, definition);
        }

        [Fact]
        public void ScaffoldingEntityExtensionsClass()
        {
            // Arrange
            var definition = CSharpClassDefinition
                .Create(AccessModifier.Public, "EntityExtensions", ns: "Domain.Entities")
                .IsStatic()
                .ImportNs("System")
                .ImportNs("Domain.Entities")
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
            CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, _solutionDirectory, _domainDirectory), _entitiesDirectory, true, definition);
        }

        [Fact]
        public void ScaffoldingCustOrderHistResultClass()
        {
            // Arrange
            var definition = CSharpClassDefinition
                .Create(AccessModifier.Public, "CustOrderHist", ns: "Infrastructure.Persistence.QueryModels")
                ;

            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("string", "ProductName"));

            definition.Properties.Add(new(AccessModifier.Public, "int", "Total")
            {
                IsAutomatic = true,
                InitializationValue = "0"
            });

            // Act
            CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, _solutionDirectory, _infrastructureDirectory, _persistenceDirectory), "QueryModels", true, definition);
        }

        [Fact]
        public void ScaffoldingDbContextClass()
        {
            // Arrange
            var definition = CSharpClassDefinition
                .Create(AccessModifier.Public, "NorthwindDbContext", ns: "Infrastructure.Persistence", baseClass: "DbContext")
                .SetDocumentation(summary: "Represents Northwind database in EF Core Model")
                .IsPartial()
                .ImportNs("System")
                .ImportNs("Microsoft.EntityFrameworkCore")
                .ImportNs("Domain.Entities")
                .ImportNs("Infrastructure.Persistence.QueryModels")
                ;

            definition.Constructors.Add(new(AccessModifier.Public, new ParameterDefinition("DbContextOptions<NorthwindDbContext>", "options")
            {
                Documentation = new("Instance of DbContext options")
            })
            {
                Invocation = "base(options)",
                Documentation = new("Initializes a new instance of NorthwindDbContext class")
            });

            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("DbSet<Product>", "Products"));
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("DbSet<Supplier>", "Suppliers"));
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("DbSet<Category>", "Categories"));
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("DbSet<Shipper>", "Shippers"));
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("DbSet<Order>", "Orders"));

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
                    new CodeLine(1, "builder.HasKey(p => p.ProductID);"),
                    new CodeLine(),
                    new CodeLine(1, "builder.Property(p => p.ProductID).UseIdentityColumn();"),
                    new CodeLine(1, "builder.Property(p => p.ProductName).HasMaxLength(40).IsRequired();"),
                    new CodeLine("});"),
                    new CodeLine(),
                    new CommentLine(" Register results for stored procedures"),
                    new CodeLine(),
                    new CommentLine(" 'dbo.CustOrderHist'"),
                    new CodeLine("modelBuilder.Entity<CustOrderHist>().HasNoKey();")
                }
            });

            // Act
            CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, _solutionDirectory, _infrastructureDirectory), _persistenceDirectory, true, definition);
        }

        [Fact]
        public void ScaffoldingDbContextExtensionClass()
        {
            // Arrange
            var definition = CSharpClassDefinition
                .Create(AccessModifier.Public, "NorthwindDbContextExtensions", ns: "Infrastructure.Persistence")
                .IsPartial()
                .IsStatic()
                .ImportNs("System")
                .ImportNs("System.Collections.Generic")
                .ImportNs("System.Linq")
                .ImportNs("System.Threading.Tasks")
                .ImportNs("Microsoft.EntityFrameworkCore")
                .ImportNs("Microsoft.Data.SqlClient")
                .ImportNs("Domain.Entities")
                .ImportNs("Infrastructure.Persistence.QueryModels")
                ;

            definition.Methods.Add(new(AccessModifier.Public, "IQueryable<Order>", "GetOrders")
            {
                IsStatic = true,
                IsExtension = true,
                Parameters =
                {
                    new ParameterDefinition("NorthwindDbContext", "dbContext"),
                    new ParameterDefinition("string", "customerID", "null"),
                    new ParameterDefinition("int?", "employeeID", "null")
                },
                Lines =
                {
                    new CodeLine("var query = dbContext.Orders.AsQueryable();"),
                    new CodeLine(),
                    new CodeLine("if (!string.IsNullOrEmpty(customerID))"),
                    new CodeLine(1, "query = query.Where(item => item.CustomerID == customerID);"),
                    new CodeLine(),
                    new CodeLine(),
                    new CodeLine("if (employeeID.HasValue)"),
                    new CodeLine(1, "query = query.Where(item => item.EmployeeID == employeeID);"),
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
                    new CodeLine(1, "Text = \" EXEC [dbo].[CustOrderHist] @CustomerID \","),
                    new CodeLine(1, "Parameters = new[]"),
                    new CodeLine(1, "{"),
                    new CodeLine(2, "new SqlParameter(\"@CustomerID\", customerID)"),
                    new CodeLine(1, "}"),
                    new CodeLine("};"),
                    new CodeLine(),
                    new ReturnLine("await dbContext"),
                    new CodeLine(1, ".Set<CustOrderHist>()"),
                    new CodeLine(1, ".FromSqlRaw(query.Text, query.Parameters)"),
                    new CodeLine(1, ".ToListAsync()"),
                    new CodeLine(1, ";")
                }
            });

            // Act
            var codeBuilder = new CSharpClassBuilder
            {
                OutputDirectory = Path.Combine(_baseDirectory, _solutionDirectory, _infrastructureDirectory, _persistenceDirectory),
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
                .Create(AccessModifier.Public, "Repository", ns: "Infrastructure.Persistence")
                .IsPartial()
                .Implement("IRepository")
                .ImportNs("System")
                .AddCtor(CSharpClassDefinition.CreateCtor().AddParam("NorthwindDbContext", "dbContext").AddLine("_dbContext = dbContext;"))
                ;

            definition.Fields.Add(new FieldDefinition(AccessModifier.Protected, "NorthwindDbContext", "_dbContext")
            {
                IsReadOnly = true
            });

            definition.Methods.Add(new(AccessModifier.Public, "", "Dispose")
            {
                Lines =
                {
                    new TodoLine("Implement dispose for DbContext")
                }
            });

            // Act
            CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, _solutionDirectory, _infrastructureDirectory), _persistenceDirectory, true, definition);
        }

        [Fact]
        public void ScaffoldingRepositoryClass()
        {
            // Arrange
            var definition = CSharpClassDefinition
                .Create(AccessModifier.Public, "NorthwindRepository", ns: "Infrastructure.Persistence", baseClass: "Repository")
                .Implement("INorthwindRepository")
                .ImportNs("System")
                .ImportNs("System.Linq")
                .ImportNs("Microsoft.EntityFrameworkCore")
                .ImportNs("Domain.Entities")
                ;

            definition.Constructors.Add(CSharpClassDefinition.CreateCtor(invocation: "base(dbContext)").AddParam("NorthwindDbContext", "dbContext"));

            definition.Methods.Add(new MethodDefinition(AccessModifier.Public, "IQueryable<Product>", "GetProducts")
            {
                Parameters =
                {
                    new ParameterDefinition("int?", "supplierID")
                },
                Lines =
                {
                    new CodeLine("var query = _dbContext.Products.AsQueryable();"),
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
                    new ReturnLine("_dbContext.Shippers;")
                }
            });

            definition.Methods.Add(new MethodDefinition(AccessModifier.Public, "IQueryable<Order>", "GetOrders")
            {
                Lines =
                {
                    new ReturnLine("_dbContext.Orders;")
                }
            });

            // Act
            var codeBuilder = new CSharpClassBuilder
            {
                OutputDirectory = Path.Combine(_baseDirectory, _solutionDirectory, _infrastructureDirectory, _persistenceDirectory),
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
            CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, "DesignPatterns"), string.Empty, true, definition);
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
            CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, "DesignPatterns"), string.Empty, true, definition);
        }
    }
}
