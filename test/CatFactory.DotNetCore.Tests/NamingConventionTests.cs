using Xunit;

namespace CatFactory.DotNetCore.Tests
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
        }
    }
}
