using CatFactory.NetCore.CodeFactory;
using CatFactory.NetCore.Tests.Models;
using CatFactory.ObjectRelationalMapping;
using Xunit;

namespace CatFactory.NetCore.Tests;

public class NamingConventionTests
{
    [Theory]
    [InlineData("service", "IService", true)]
    [InlineData("SERVICE", "IService", true)]
    [InlineData("sales_service", "ISalesService", true)]
    [InlineData("SALES_SERVICE", "ISalesService", true)]
    [InlineData("sales service", "ISalesService", true)]
    [InlineData("SALES SERVICE", "ISalesService", true)]
    public void InterfaceNamingConvention(string value, string expected, bool equal)
    {
        // Arrange
        var namingConvention = new DotNetNamingConvention();

        // Act
        // Assert
        if (equal)
            Assert.Equal(namingConvention.GetInterfaceName(value), expected);
        else
            Assert.NotEqual(namingConvention.GetInterfaceName(value), expected);
    }

    [Theory]
    [InlineData("product", "Product", true)]
    [InlineData("PRODUCT", "Product", true)]
    [InlineData("product_picture", "ProductPicture", true)]
    [InlineData("PRODUCT_PICTURE", "ProductPicture", true)]
    [InlineData("product picture", "ProductPicture", true)]
    [InlineData("PRODUCT PICTURE", "ProductPicture", true)]
    public void ClassNamingConvention(string value, string expected, bool equal)
    {
        // Arrange
        var namingConvention = new DotNetNamingConvention();

        // Act
        // Assert
        if (equal)
            Assert.Equal(namingConvention.GetClassName(value), expected);
        else
            Assert.NotEqual(namingConvention.GetClassName(value), expected);
    }

    [Theory]
    [InlineData("max amount", "MaxAmount", true)]
    [InlineData("max_amount", "MaxAmount", true)]
    [InlineData("MIN AMOUNT", "MinAmount", true)]
    [InlineData("MIN_AMOUNT", "MinAmount", true)]
    public void ConstantNamingConvention(string value, string expected, bool equal)
    {
        // Arrange
        var namingConvention = new DotNetNamingConvention();

        // Act
        // Assert
        if (equal)
            Assert.Equal(namingConvention.GetConstantName(value), expected);
        else
            Assert.NotEqual(namingConvention.GetConstantName(value), expected);
    }

    [Theory]
    [InlineData("orderHeaderID", "_orderHeaderID", true)]
    [InlineData("OrderHeaderID", "_orderHeaderID", true)]
    [InlineData("order_header_id", "_orderHeaderId", true)]
    [InlineData("ORDER_HEADER_ID", "_orderHeaderId", true)]
    [InlineData("Order_Header_ID", "_orderHeaderId", true)]
    [InlineData("System.String", "_systemString", true)]
    public void FieldNamingConvention(string value, string expected, bool equal)
    {
        // Arrange
        var namingConvention = new DotNetNamingConvention();

        // Act
        // Assert
        if (equal)
            Assert.Equal(namingConvention.GetFieldName(value), expected);
        else
            Assert.NotEqual(namingConvention.GetFieldName(value), expected);
    }

    [Theory]
    [InlineData("orderHeaderID", "OrderHeaderID", true)]
    [InlineData("OrderHeaderID", "OrderHeaderID", true)]
    [InlineData("order_header_id", "OrderHeaderId", true)]
    [InlineData("ORDER_HEADER_ID", "OrderHeaderId", true)]
    [InlineData("Order_Header_ID", "OrderHeaderId", true)]
    [InlineData("System.String", "SystemString", true)]
    public void PropertyNamingConvention(string value, string expected, bool equal)
    {
        // Arrange
        var namingConvention = new DotNetNamingConvention();

        // Act
        // Assert
        if (equal)
            Assert.Equal(namingConvention.GetPropertyName(value), expected);
        else
            Assert.NotEqual(namingConvention.GetPropertyName(value), expected);
    }

    [Theory]
    [InlineData("getEmployees", "GetEmployees", true)]
    [InlineData("get Employees", "GetEmployees", true)]
    [InlineData("get_employees", "GetEmployees", true)]
    [InlineData("GET EMPLOYEES", "GetEmployees", true)]
    public void MethodNamingConvention(string value, string expected, bool equal)
    {
        // Arrange
        var namingConvention = new DotNetNamingConvention();

        // Act
        // Assert
        if (equal)
            Assert.Equal(namingConvention.GetMethodName(value), expected);
        else
            Assert.NotEqual(namingConvention.GetMethodName(value), expected);
    }

    [Theory]
    [InlineData("OrderHeaderID", "orderHeaderID", true)]
    [InlineData("Order_Header_ID", "orderHeaderId", true)]
    [InlineData("Order Header ID", "orderHeaderId", true)]
    [InlineData("ORDER_HEADER_ID", "orderHeaderId", true)]
    [InlineData("ORDER HEADER ID", "orderHeaderId", true)]
    public void ParameterNamingConvention(string value, string expected, bool equal)
    {
        // Arrange
        var namingConvention = new DotNetNamingConvention();

        // Act
        // Assert
        if (equal)
            Assert.Equal(namingConvention.GetParameterName(value), expected);
        else
            Assert.NotEqual(namingConvention.GetParameterName(value), expected);
    }

    [Fact]
    public void ProjectNamingConvention()
    {
        // Arrange
        var project = new CSharpProject<CSharpProjectSettings>();
        var table = new Table
        {
            Name = "Foo",
            Columns =
            {
                new Column { Name = "Foo" },
                new Column { Name = "2019" },
                new Column { Name = "class" },
                new Column { Name = "interface" },
                new Column { Name = "System.String" }
            }
        };

        var view = new View
        {
            Name = "Bar",
            Columns =
            {
                new Column { Name = "Bar" },
                new Column { Name = "2019" },
                new Column { Name = "class" },
                new Column { Name = "interface" },
                new Column { Name = "System.String" }
            }
        };

        // Act
        // Assert
        Assert.Equal("Foo1", project.GetPropName(table, table.Columns.First()));
        Assert.Equal("V2019", project.GetPropName(table, table.Columns[1]));
        Assert.Equal("Class", project.GetPropName(table, table.Columns[2]));
        Assert.Equal("Interface", project.GetPropName(table, table.Columns[3]));
        Assert.Equal("SystemString", project.GetPropName(table, table.Columns[4]));

        Assert.Equal("Bar1", project.GetPropName(view, view.Columns.First()));
        Assert.Equal("V2019", project.GetPropName(view, view.Columns[1]));
        Assert.Equal("Class", project.GetPropName(view, view.Columns[2]));
        Assert.Equal("Interface", project.GetPropName(view, view.Columns[3]));
        Assert.Equal("SystemString", project.GetPropName(view, view.Columns[4]));
    }
}
