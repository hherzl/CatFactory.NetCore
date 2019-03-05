using System.Linq;
using CatFactory.NetCore.CodeFactory;
using CatFactory.NetCore.Tests.Models;
using CatFactory.ObjectRelationalMapping;
using Xunit;

namespace CatFactory.NetCore.Tests
{
    public class NamingConventionTests
    {
        [Fact]
        public void TestClassNamingConvention()
        {
            // Arrange
            var namingConvention = new DotNetNamingConvention();

            // Act
            // Assert
            Assert.True("Product" == namingConvention.GetClassName("product"));
            Assert.True("Product" == namingConvention.GetClassName("PRODUCT"));
            Assert.True("ProductPicture" == namingConvention.GetClassName("product_picture"));
            Assert.True("ProductPicture" == namingConvention.GetClassName("PRODUCT_PICTURE"));
            Assert.True("ProductPicture" == namingConvention.GetClassName("product picture"));
            Assert.True("ProductPicture" == namingConvention.GetClassName("PRODUCT PICTURE"));
        }

        [Fact]
        public void TestInterfaceNamingConvention()
        {
            // Arrange
            var namingConvention = new DotNetNamingConvention();

            // Act
            // Assert
            Assert.True("IProduct" == namingConvention.GetInterfaceName("product"));
            Assert.True("IProduct" == namingConvention.GetInterfaceName("PRODUCT"));
            Assert.True("ISalesService" == namingConvention.GetInterfaceName("sales_service"));
            Assert.True("ISalesService" == namingConvention.GetInterfaceName("SALES_SERVICE"));
            Assert.True("ISalesService" == namingConvention.GetInterfaceName("sales service"));
            Assert.True("ISalesService" == namingConvention.GetInterfaceName("SALES SERVICE"));
        }

        [Fact]
        public void TestPropertyNamingConvention()
        {
            // Arrange
            var namingConvention = new DotNetNamingConvention();

            // Act
            // Assert
            Assert.True("OrderHeaderID" == namingConvention.GetPropertyName("orderHeaderID"));
            Assert.True("OrderHeaderID" == namingConvention.GetPropertyName("OrderHeaderID"));
            Assert.True("OrderHeaderId" == namingConvention.GetPropertyName("order_header_id"));
            Assert.True("OrderHeaderId" == namingConvention.GetPropertyName("ORDER_HEADER_ID"));
            Assert.True("OrderHeaderId" == namingConvention.GetPropertyName("Order_Header_ID"));
        }

        [Fact]
        public void TestMethodNamingConvention()
        {
            // Arrange
            var namingConvention = new DotNetNamingConvention();

            // Act
            // Assert
            Assert.True("GetEmployees" == namingConvention.GetMethodName("getEmployees"));
            Assert.True("GetEmployees" == namingConvention.GetMethodName("get_employees"));
            Assert.True("Foo" == namingConvention.GetMethodName("foo"));
            Assert.True("Foo" == namingConvention.GetMethodName("FOO"));
            Assert.True("Foo" == namingConvention.GetMethodName("Foo"));
        }

        [Theory]
        [InlineData("OrderHeaderID","orderHeaderID",true)]
        [InlineData("Order_Header_ID","orderHeaderID",true)]
        [InlineData("FOO","foo",true)]
        [InlineData("class","class",false)]
        [InlineData("class","@class",true)]
        public void TestParameterNamingConvention(string  value, string expected, bool equal)
        {
            // Arrange
            var namingConvention = new DotNetNamingConvention();

            // Act
            // Assert
            if(equal)
            {
                Assert.Equal(expected,namingConvention.GetParameterName(value));
            }
            else
            {
                Assert.NotEqual(expected,namingConvention.GetParameterName(value));
            }
        }

        [Fact]
        public void TestProjectNamingConvention()
        {
            // Arrange
            var project = new CSharpProject<CSharpProjectSettings>();
            var table = new Table
            {
                Name = "Foo",
                Columns =
                {
                    new Column
                    {
                        Name = "Foo"
                    },
                    new Column
                    {
                        Name = "2019"
                    }
                }
            };

            var view = new View
            {
                Name = "Bar",
                Columns =
                {
                    new Column
                    {
                        Name = "Bar"
                    },
                    new Column
                    {
                        Name = "2019"
                    }
                }
            };

            // Act
            // Assert
            Assert.True("Foo1" == project.GetPropertyName(table, table.Columns.First()));
            Assert.True("V2019" == project.GetPropertyName(table, table.Columns[1]));
            Assert.True("Bar1" == project.GetPropertyName(view, view.Columns.First()));
            Assert.True("V2019" == project.GetPropertyName(view, view.Columns[1]));
        }
    }
}
