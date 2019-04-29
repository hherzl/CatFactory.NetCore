using System.Linq;
using CatFactory.NetCore.CodeFactory;
using CatFactory.NetCore.Tests.Models;
using CatFactory.ObjectRelationalMapping;
using Xunit;

namespace CatFactory.NetCore.Tests
{
    public class NamingConventionTests
    {
        [Theory]
        [InlineData("Class", "class", true)]
        [InlineData("Product", "product", true)]
        [InlineData("Product", "PRODUCT", true)]
        [InlineData("ProductPicture", "product_picture", true)]
        [InlineData("ProductPicture", "PRODUCT_PICTURE", true)]
        [InlineData("ProductPicture", "product picture", true)]
        [InlineData("ProductPicture", "PRODUCT PICTURE", true)]
        public void TestClassNamingConvention(string expected, string name, bool Equal)
        {
            // Arrange
            var namingConvention = new DotNetNamingConvention();

            // Act
            // Assert
            if (Equal)
            {
                Assert.Equal(expected, namingConvention.GetClassName(name));
            }
            else
            {
                Assert.NotEqual(expected, namingConvention.GetClassName(name));
            }
        }

        [Theory]
        [InlineData("IProduct", "product", true)]
        [InlineData("IProduct", "PRODUCT", true)]
        [InlineData("ISalesService", "sales_service", true)]
        [InlineData("ISalesService", "SALES_SERVICE", true)]
        [InlineData("ISalesService", "sales service", true)]
        [InlineData("ISalesService", "SALES SERVICE", true)]
        [InlineData("IInterface", "interface", true)]
        [InlineData("INterface", "nterface", true)]
        public void TestInterfaceNamingConvention(string expected, string name, bool Equal)
        {
            // Arrange
            var namingConvention = new DotNetNamingConvention();

            // Act
            // Assert
            if (Equal)
            {
                Assert.Equal(expected, namingConvention.GetInterfaceName(name));
            }
            else
            {
                Assert.NotEqual(expected, namingConvention.GetInterfaceName(name));
            }
        }

        [Theory]
        [InlineData("OrderHeaderID", "orderHeaderID", true)]
        [InlineData("OrderHeaderID", "OrderHeaderID", true)]
        [InlineData("OrderHeaderId", "order_header_id", true)]
        [InlineData("OrderHeaderId", "ORDER_HEADER_ID", true)]
        [InlineData("OrderHeaderId", "Order_Header_ID", true)]
        [InlineData("Class", "class", true)]
        [InlineData("SystemString", "System.String", true)]
        public void TestPropertyNamingConvention(string expected, string name, bool Equal)
        {
            // Arrange
            var namingConvention = new DotNetNamingConvention();

            // Act
            // Assert
            if (Equal)
            {
                Assert.Equal(expected, namingConvention.GetPropertyName(name));
            }
            else
            {
                Assert.NotEqual(expected, namingConvention.GetPropertyName(name));
            }
        }

        [Theory]
        [InlineData("GetEmployees", "getEmployees", true)]
        [InlineData("GetEmployees", "get_employees", true)]
        [InlineData("Foo", "foo", true)]
        [InlineData("Foo", "FOO", true)]
        [InlineData("Foo", "Foo", true)]
        [InlineData("Class", "class", true)]
        public void TestMethodNamingConvention(string expected, string name, bool Equal)
        {
            // Arrange
            var namingConvention = new DotNetNamingConvention();

            // Act
            // Assert
            if (Equal)
            {
                Assert.Equal(expected, namingConvention.GetMethodName(name));
            }
            else
            {
                Assert.NotEqual(expected, namingConvention.GetMethodName(name));
            }
        }

        [Theory]
        [InlineData("OrderHeaderID", "orderHeaderID", true)]
        [InlineData("Order_Header_ID", "orderHeaderId", true)]
        [InlineData("ORDER_HEADER_ID", "orderHeaderId", true)]
        //[InlineData("FOO", "foo", true)]
        //[InlineData("class", "class", false)]
        //[InlineData("class", "Vclass", true)]
        public void TestParameterNamingConvention(string value, string expected, bool equal)
        {
            // Arrange
            var namingConvention = new DotNetNamingConvention();

            // Act
            // Assert
            if (equal)
            {
                Assert.Equal(expected, namingConvention.GetParameterName(value));
            }
            else
            {
                Assert.NotEqual(expected, namingConvention.GetParameterName(value));
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
                    },
                    new Column
                    {
                        Name = "class"
                    },
                    new Column
                    {
                        Name = "interface"
                    },
                    new Column
                    {
                        Name = "System.String"
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
                    },
                    new Column
                    {
                    Name = "class"
                    },
                    new Column
                    {
                        Name = "interface"
                    },
                    new Column
                    {
                        Name = "System.String"
                    }
                }
            };

            // Act
            // Assert
            Assert.Equal("Foo1", project.GetPropertyName(table, table.Columns.First()));
            Assert.Equal("V2019", project.GetPropertyName(table, table.Columns[1]));
            Assert.Equal("Class", project.GetPropertyName(table, table.Columns[2]));
            Assert.Equal("Interface", project.GetPropertyName(table, table.Columns[3]));
            Assert.Equal("SystemString", project.GetPropertyName(table, table.Columns[4]));

            Assert.Equal("Bar1", project.GetPropertyName(view, view.Columns.First()));
            Assert.Equal("V2019", project.GetPropertyName(view, view.Columns[1]));
            Assert.Equal("Class", project.GetPropertyName(view, view.Columns[2]));
            Assert.Equal("Interface", project.GetPropertyName(view, view.Columns[3]));
            Assert.Equal("SystemString", project.GetPropertyName(view, view.Columns[4]));
        }
    }
}
