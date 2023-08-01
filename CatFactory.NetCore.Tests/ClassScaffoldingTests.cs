using CatFactory.NetCore.CodeFactory;
using CatFactory.NetCore.ObjectOrientedProgramming;
using CatFactory.ObjectOrientedProgramming;
using Xunit;

namespace CatFactory.NetCore.Tests;

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
        var definition = CSharpClassDefinition.Create(AccessModifier.Public, "Shipper", ns: "Domain.Entities")
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
        var definition = CSharpClassDefinition.Create(AccessModifier.Public, "Product", ns: "Domain.Entities")
            .IsPartial()
            .Implement("INotifyPropertyChanged")
            .ImportNs("System")
            .ImportNs("System.ComponentModel")
            ;

        definition.Events = new()
        {
            new(AccessModifier.Public, "PropertyChangedEventHandler", "PropertyChanged")
        };

        definition.AddViewModelProp("int?", "ProductID");
        definition.AddViewModelProp("string", "ProductName");
        definition.AddViewModelProp("int?", "SupplierID");
        definition.AddViewModelProp("int?", "CategoryID");
        definition.AddViewModelProp("string", "QuantityPerUnit");
        definition.AddViewModelProp("decimal?", "UnitPrice");
        definition.AddViewModelProp("short?", "UnitsInStock");
        definition.AddViewModelProp("short?", "UnitsOnOrder");
        definition.AddViewModelProp("short?", "ReorderLevel");
        definition.AddViewModelProp("bool?", "Discontinued");

        // Act
        CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, _solutionDirectory, _domainDirectory), _entitiesDirectory, true, definition);
    }

    [Fact]
    public void ScaffoldingSupplierClass()
    {
        // Arrange
        var definition = CSharpClassDefinition.Create(AccessModifier.Public, "Supplier", ns: "Domain.Entities")
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
        var definition = CSharpClassDefinition.Create(AccessModifier.Public, "Category", ns: "Domain.Entities")
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
        var definition = CSharpClassDefinition.Create(AccessModifier.Public, "Order", ns: "Domain.Entities")
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
        var definition = CSharpClassDefinition.Create(AccessModifier.Public, "NorthwindException", ns: "Domain.Exceptions", baseClass: "Exception")
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
        var definition = CSharpClassDefinition.Create(AccessModifier.Public, "EntityExtensions", ns: "Domain.Entities")
            .IsStatic()
            .ImportNs("System")
            .ImportNs("Domain.Entities")
            ;

        CSharpMethodDefinition.Create(AccessModifier.Public, "string", "ToJson", isExtension: true, target: definition)
            .AddParam("Order", "entity")
            .Set(body => body.Return("string.Empty;"));

        definition.SimplifyDataTypes();

        // Act
        CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, _solutionDirectory, _domainDirectory), _entitiesDirectory, true, definition);
    }

    [Fact]
    public void ScaffoldingCustOrderHistResultClass()
    {
        // Arrange
        var definition = CSharpClassDefinition.Create(AccessModifier.Public, "CustOrderHist", ns: "Infrastructure.Persistence.QueryModels");

        definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("string", "ProductName"));
        definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("int", "Total", initializationValue: "0"));

        // Act
        CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, _solutionDirectory, _infrastructureDirectory, _persistenceDirectory), "QueryModels", true, definition);
    }

    [Fact]
    public void ScaffoldingDbContextClass()
    {
        // Arrange
        var definition = CSharpClassDefinition.Create(AccessModifier.Public, "NorthwindDbContext", ns: "Infrastructure.Persistence", baseClass: "DbContext")
            .SetDocumentation(summary: "Represents Northwind database in EF Core Model")
            .IsPartial()
            .ImportNs("System")
            .ImportNs("Microsoft.EntityFrameworkCore")
            .ImportNs("Domain.Entities")
            .ImportNs("Infrastructure.Persistence.QueryModels")
            ;

        definition.Constructors.Add(
            CSharpClassDefinition.CreateCtor(AccessModifier.Public, invocation: "base(options)", summary: "Initializes a new instance of NorthwindDbContext class")
                .AddParam("DbContextOptions<NorthwindDbContext>", "options", summary: "Instance of DbContext options")
        );

        definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("DbSet<Product>", "Products"));
        definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("DbSet<Supplier>", "Suppliers"));
        definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("DbSet<Category>", "Categories"));
        definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("DbSet<Shipper>", "Shippers"));
        definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("DbSet<Order>", "Orders"));

        CSharpMethodDefinition.Create(AccessModifier.Protected, "", "OnModelCreating", isOverride: true, target: definition)
            .AddParam("ModelBuilder", "modelBuilder")
            .Set(body =>
            {
                body
                .Line("modelBuilder.Entity<Product>(builder =>")
                .Line("{")
                .Comment(1, " Add configuration for 'dbo.Products' table")
                .Empty()
                .Line(1, "builder.ToTable(\"Products\", \"dbo\");")
                .Empty()
                .Line(1, "builder.HasKey(p => p.ProductID);")
                .Empty()
                .Line(1, "builder.Property(p => p.ProductID).UseIdentityColumn();")
                .Line(1, "builder.Property(p => p.ProductName).HasMaxLength(40).IsRequired();")
                .Line("});")
                .Empty()
                .Comment(" Register results for stored procedures")
                .Empty()
                .Comment(" 'dbo.CustOrderHist'")
                .Line("modelBuilder.Entity<CustOrderHist>().HasNoKey();");
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

        CSharpMethodDefinition.Create(AccessModifier.Public, "IQueryable<Order>", "GetOrders", isExtension: true, target: definition)
            .AddParam("NorthwindDbContext", "dbContext")
            .AddParam("string", "customerID", "null")
            .AddParam("int?", "employeeID", "null")
            .Set(body =>
            {
                body
                    .Line("var query = dbContext.Orders.AsQueryable();")
                    .Empty()
                    .Line("if (!string.IsNullOrEmpty(customerID))")
                    .Line(1, "query = query.Where(item => item.CustomerID == customerID);")
                    .Empty()
                    .Empty()
                    .Line("if (employeeID.HasValue)")
                    .Line(1, "query = query.Where(item => item.EmployeeID == employeeID);")
                    .Empty()
                    .Return("query;")
                    ;
            });

        CSharpMethodDefinition.Create(AccessModifier.Public, "Task<List<CustOrderHist>>", "GetCustOrderHistAsync", isExtension: true, isAsync: true, target: definition)
            .AddParam("NorthwindDbContext", "dbContext")
            .AddParam("string", "customerID")
            .Set(body =>
            {
                body
                    .Line("var query = new")
                    .Line("{")
                    .Line(1, "Text = \" EXEC [dbo].[CustOrderHist] @CustomerID \",")
                    .Line(1, "Parameters = new[]")
                    .Line(1, "{")
                    .Line(2, "new SqlParameter(\"@CustomerID\", customerID)")
                    .Line(1, "}")
                    .Line("};")
                    .Empty()
                    .Return("await dbContext")
                    .Line(1, ".Set<CustOrderHist>()")
                    .Line(1, ".FromSqlRaw(query.Text, query.Parameters)")
                    .Line(1, ".ToListAsync()")
                    .Line(1, ";");
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

        definition.Fields.Add(CSharpFieldDefinition.Create("NorthwindDbContext", "_dbContext", AccessModifier.Protected, isReadOnly: true));

        CSharpMethodDefinition.Create(AccessModifier.Public, "", "Dispose", target: definition)
            .Set(body => body.Todo("Implement dispose for DbContext"))
            ;

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

        CSharpMethodDefinition.Create(AccessModifier.Public, "IQueryable<Product>", "GetProducts", target: definition)
            .AddParam("int?", "supplierID")
            .Set(body =>
            {
                body
                    .Line("var query = _dbContext.Products.AsQueryable();")
                    .Empty()
                    .Line("if (supplierID.HasValue)")
                    .Line(1, "query = query.Where(item => item.SupplierID == supplierID);")
                    .Empty()
                    .Return("query;");
            });

        CSharpMethodDefinition.Create(AccessModifier.Public, "IQueryable<Shipper>", "GetShippers", target: definition)
            .Set(body =>body.Return("_dbContext.Shippers;"));

        CSharpMethodDefinition.Create(AccessModifier.Public, "IQueryable<Order>", "GetOrders", target: definition)
            .Set(body => body.Return("_dbContext.Orders;"));

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

        definition.Fields.Add(CSharpFieldDefinition.Create("string", "Bar", AccessModifier.Public, isReadOnly: true, value: "\"ABC\""));

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
