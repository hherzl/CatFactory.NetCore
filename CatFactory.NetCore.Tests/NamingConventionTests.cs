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

            // Act and Assert
            Assert.True("Product" == namingConvention.GetClassName("product"));
            Assert.True("Product" == namingConvention.GetClassName("PRODUCT"));
            Assert.True("ProductPicture" == namingConvention.GetClassName("product_picture"));
            Assert.True("ProductPicture" == namingConvention.GetClassName("PRODUCT_PICTURE"));
        }

        [Fact]
        public void TestInterfaceNamingConvention()
        {
            // Arrange
            var namingConvention = new DotNetNamingConvention();

            // Act and Assert
            Assert.True("IProduct" == namingConvention.GetInterfaceName("product"));
            Assert.True("IProduct" == namingConvention.GetInterfaceName("PRODUCT"));
        }
    }
}
