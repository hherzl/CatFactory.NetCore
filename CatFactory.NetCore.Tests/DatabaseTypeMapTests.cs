using CatFactory.SqlServer;
using Xunit;

namespace CatFactory.NetCore.Tests
{
    public class DatabaseTypeMapTests
    {
        [Fact]
        public void TestResolveDatabaseTypeMapsForOnlineStoreDb()
        {
            // Arrange
            var database = SqlServerDatabaseFactory.Import("server=(local);database=OnlineStore;integrated security=yes;");

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
        public void TestResolveDatabaseTypeMapsFromComposedStringsForOnlineStoreDb()
        {
            // Arrange
            var database = SqlServerDatabaseFactory.Import("server=(local);database=OnlineStore;integrated security=yes;");

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
        public void TestResolveDatabaseTypeMapsFromComposedStringsForAdventureWorks2017Db()
        {
            // Arrange
            var database = SqlServerDatabaseFactory.Import("server=(local);database=AdventureWorks2017;integrated security=yes;");

            // Act
            var nameTypeMap = database.ResolveDatabaseType("Name");
            var fooTypeMap = database.ResolveDatabaseType("foo");

            // Assert
            Assert.False(nameTypeMap == null);
            Assert.True(nameTypeMap == "String");
            Assert.True(fooTypeMap == "object");
        }
    }
}
