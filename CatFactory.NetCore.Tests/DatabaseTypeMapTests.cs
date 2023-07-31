using System.Threading.Tasks;
using CatFactory.SqlServer;
using Xunit;

namespace CatFactory.NetCore.Tests;

public class DatabaseTypeMapTests
{
    [Fact]
    public async Task ResolveDatabaseTypeMapsForOnlineStoreDbAsync()
    {
        // Arrange
        var database = await SqlServerDatabaseFactory
            .ImportAsync("server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;");

        // Act
        var varcharTypeMap = database.ResolveDatabaseType("varchar");
        var intTypeMap = database.ResolveDatabaseType("int");
        var datetimeMap = database.ResolveDatabaseType("datetime");

        // Assert
        Assert.False(varcharTypeMap == null);
        Assert.False(intTypeMap == null);
        Assert.False(datetimeMap == null);
    }

    [Fact]
    public async Task ResolveDatabaseTypeMapsFromComposedStringsForOnlineStoreDbAsync()
    {
        // Arrange
        var database = await SqlServerDatabaseFactory
            .ImportAsync("server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;");

        // Act
        var varcharTypeMap = database.ResolveDatabaseType("varchar(25)");
        var decimalMap = database.ResolveDatabaseType("decimal(8, 4)");

        // Assert
        Assert.False(varcharTypeMap == null);
        Assert.True(varcharTypeMap == "String");
        Assert.False(decimalMap == null);
        Assert.True(decimalMap == "Decimal?");
    }

    [Fact]
    public async Task ResolveDatabaseTypeMapsFromComposedStringsForAdventureWorks2017DbAsync()
    {
        // Arrange
        var database = await SqlServerDatabaseFactory
            .ImportAsync("server=(local); database=AdventureWorks2017; integrated security=yes; TrustServerCertificate=True;");

        // Act
        var nameTypeMap = database.ResolveDatabaseType("Name");
        var fooTypeMap = database.ResolveDatabaseType("foo");

        // Assert
        Assert.False(nameTypeMap == null);
        Assert.True(nameTypeMap == "String");
        Assert.True(fooTypeMap == "object");
    }
}
