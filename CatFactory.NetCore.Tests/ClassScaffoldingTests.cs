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
            .UsingNs("System")
            .UsingNs("System.ComponentModel.DataAnnotations")
            .UsingNs("System.ComponentModel.DataAnnotations.Schema");

        definition.AddTableAttrib("Shippers", schema: "dbo");

        definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("int?", "ShipperId").AddKeyAttrib().AddDatabaseGeneratedAttrib());
        definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("string", "CompanyName").AddStringLengthAttrib(80).AddRequiredAttrib());
        definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("string", "Phone").AddStringLengthAttrib(48).AddRequiredAttrib());

        // Act
        CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, _solutionDirectory, _domainDirectory), _entitiesDirectory, true, definition);
    }

    [Fact]
    public void ScaffoldingProductViewModelClass()
    {
        // Arrange
        var definition = CSharpClassDefinition.Create(AccessModifier.Public, "Product", ns: "Domain.Entities", isPartial: true)
            .Implement("INotifyPropertyChanged")
            .UsingNs("System")
            .UsingNs("System.ComponentModel");

        definition.Events = new()
        {
            new(AccessModifier.Public, "PropertyChangedEventHandler", "PropertyChanged")
        };

        definition.AddViewModelProp("int?", "ProductId");
        definition.AddViewModelProp("string", "ProductName");
        definition.AddViewModelProp("int?", "SupplierId");
        definition.AddViewModelProp("int?", "CategoryId");
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
            .UsingNs("System")
            .UsingNs("System.ComponentModel.DataAnnotations")
            .UsingNs("System.ComponentModel.DataAnnotations.Schema");

        definition.AddTableAttrib("Suppliers", schema: "dbo");

        definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("int?", "SupplierId").AddKeyAttrib().AddDatabaseGeneratedAttrib());
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
            .UsingNs("System")
            .UsingNs("System.ComponentModel.DataAnnotations")
            .UsingNs("System.ComponentModel.DataAnnotations.Schema");

        definition.AddTableAttrib("Categories", schema: "dbo");

        definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("int?", "CategoryId").AddKeyAttrib().AddDatabaseGeneratedAttrib());
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
            .UsingNs("System")
            .UsingNs("System.ComponentModel.DataAnnotations")
            .UsingNs("System.ComponentModel.DataAnnotations.Schema");

        definition.AddTableAttrib("Orders", schema: "dbo");

        definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("int?", "OrderId").AddKeyAttrib().AddDatabaseGeneratedAttrib());
        definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("string", "CustomerId").AddColumnAttrib().AddStringLengthAttrib(5));
        definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("int?", "EmployeeId").AddColumnAttrib());
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
            .UsingNs("System")
            .AddCtor(CSharpClassDefinition.CreateCtor(invocation: "base()"))
            .AddCtor(CSharpClassDefinition.CreateCtor(invocation: "base(message)").AddParam("string", "message"))
            .AddCtor(CSharpClassDefinition.CreateCtor(invocation: "base(message, innerException)").AddParam("string", "message").AddParam("Exception", "innerException"));

        // Act
        CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, _solutionDirectory, _domainDirectory), _exceptionsDirectory, true, definition);
    }

    [Fact]
    public void ScaffoldingEntityExtensionsClass()
    {
        // Arrange
        var definition = CSharpClassDefinition.Create(AccessModifier.Public, "EntityExtensions", ns: "Domain.Entities", isStatic: true)
            .UsingNs("System")
            .UsingNs("Domain.Entities");

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
        definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProp("int", "Total", initValue: "0"));

        // Act
        CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, _solutionDirectory, _infrastructureDirectory, _persistenceDirectory), "QueryModels", true, definition);
    }

    [Fact]
    public void ScaffoldingDbContextClass()
    {
        // Arrange
        var definition = CSharpClassDefinition.Create(AccessModifier.Public, "NorthwindDbContext", ns: "Infrastructure.Persistence", baseClass: "DbContext", isPartial: true)
            .SetDocumentation(summary: "Represents Northwind database in EF Core Model")
            .UsingNs("System")
            .UsingNs("Microsoft.EntityFrameworkCore")
            .UsingNs("Domain.Entities")
            .UsingNs("Infrastructure.Persistence.QueryModels")
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
                .Line(1, "builder.HasKey(p => p.ProductId);")
                .Empty()
                .Line(1, "builder.Property(p => p.ProductId).UseIdentityColumn();")
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
            .Create(AccessModifier.Public, "NorthwindDbContextExtensions", ns: "Infrastructure.Persistence", isPartial: true, isStatic: true)
            .UsingNs("System")
            .UsingNs("System.Collections.Generic")
            .UsingNs("System.Linq")
            .UsingNs("System.Threading.Tasks")
            .UsingNs("Microsoft.EntityFrameworkCore")
            .UsingNs("Microsoft.Data.SqlClient")
            .UsingNs("Domain.Entities")
            .UsingNs("Infrastructure.Persistence.QueryModels");

        CSharpMethodDefinition.Create(AccessModifier.Public, "IQueryable<Order>", "GetOrders", isExtension: true, target: definition)
            .AddParam("NorthwindDbContext", "dbContext")
            .AddParam("string", "customerId", "null")
            .AddParam("int?", "employeeId", "null")
            .Set(body =>
            {
                body
                    .Line("var query = dbContext.Orders.AsQueryable();")
                    .Empty()
                    .Line("if (!string.IsNullOrEmpty(customerId))")
                    .Line(1, "query = query.Where(item => item.CustomerId == customerId);")
                    .Empty()
                    .Empty()
                    .Line("if (employeeId.HasValue)")
                    .Line(1, "query = query.Where(item => item.EmployeeId == employeeId);")
                    .Empty()
                    .Return("query;");
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
            .Create(AccessModifier.Public, "Repository", ns: "Infrastructure.Persistence", isPartial: true)
            .Implement("IRepository")
            .UsingNs("System")
            .AddCtor(CSharpClassDefinition.CreateCtor().AddParam("NorthwindDbContext", "dbContext").AddLine("_dbContext = dbContext;"));

        definition.Fields.Add(CSharpFieldDefinition.Create("NorthwindDbContext", "_dbContext", AccessModifier.Protected, isReadOnly: true));

        CSharpMethodDefinition.Create(AccessModifier.Public, "", "Dispose", target: definition)
            .Set(body => body.Todo("Implement dispose for DbContext"));

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
            .UsingNs("System")
            .UsingNs("System.Linq")
            .UsingNs("Microsoft.EntityFrameworkCore")
            .UsingNs("Domain.Entities");

        definition.Constructors.Add(CSharpClassDefinition.CreateCtor(invocation: "base(dbContext)").AddParam("NorthwindDbContext", "dbContext"));

        CSharpMethodDefinition.Create(AccessModifier.Public, "IQueryable<Product>", "GetProducts", target: definition)
            .AddParam("int?", "supplierId")
            .Set(body =>
            {
                body
                    .Line("var query = _dbContext.Products.AsQueryable();")
                    .Empty()
                    .Line("if (supplierId.HasValue)")
                    .Line(1, "query = query.Where(item => item.SupplierId == supplierId);")
                    .Empty()
                    .Return("query;");
            });

        CSharpMethodDefinition.Create(AccessModifier.Public, "IQueryable<Shipper>", "GetShippers", target: definition)
            .Set(body => body.Return("_dbContext.Shippers;"));

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
            .UsingNs("System")
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
            .Create(AccessModifier.Public, "Tokens", ns: "DesignPatterns", isStatic: true)
            .UsingNs("System")
            ;

        definition.Constants.Add(new(AccessModifier.Public, "int", "Foo", 1000));
        definition.Constants.Add(new(AccessModifier.Public, "string", "Bar", "ABC"));
        definition.Constants.Add(new(AccessModifier.Public, "bool", "Baz", true));

        // Act
        CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, "DesignPatterns"), string.Empty, true, definition);
    }

    [Fact]
    public void ScaffoldingClassWithGenericMethod()
    {
        // Arrange
        var definition = CSharpClassDefinition
            .Create(AccessModifier.Public, "QueryExtensions", isStatic: true)
            .UsingNs("System")
            .Ns("DesignPatterns");

        definition.Methods.Add(new MethodDefinition(AccessModifier.Public, "IEnumerable<TQueryModel>", "Foo")
            .IsExtension()
            .AddGenericType("TQueryModel", "class", "new()")
            .AddParam("IEnumerable<TQueryModel>", "sequence")
            .Set(body =>
            {
                body.Line("return sequence;");
            })
        );

        // Act
        CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, "DesignPatterns"), string.Empty, true, definition);
    }
}
