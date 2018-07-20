using CatFactory.CodeFactory;
using CatFactory.OOP;
using Xunit;

namespace CatFactory.NetCore.Tests
{
    public class WebApiCodeGenerationTests
    {
        [Fact]
        public void TestController()
        {
            // Arrange
            var definition = new CSharpClassDefinition
            {
                Namespace = "Controllers",
                Name = "SalesController",
                BaseClass = "Controller",
                Attributes =
                {
                    new MetadataAttribute("Route", "\"api/[controller]\"")
                }
            };

            definition.Namespaces.Add("System");
            definition.Namespaces.Add("System.Threading.Tasks");
            definition.Namespaces.Add("Microsoft.AspNetCore.Mvc");

            definition.Fields.Add(new FieldDefinition(AccessModifier.Protected, "ISalesRepository", "Repository") { IsReadOnly = true });

            definition.Constructors.Add(new ClassConstructorDefinition(new ParameterDefinition("ISalesRepository", "repository"))
            {
                Lines =
                {
                    new CodeLine("Repository = repository;")
                }
            });

            definition.Methods.Add(new MethodDefinition(AccessModifier.Protected, "void", "Disposed", new ParameterDefinition("Boolean", "disposing"))
            {
                IsOverride = true,
                Lines =
                {
                    new CodeLine("Repository?.Dispose();"),
                    new CodeLine(),
                    new CodeLine("base.Dispose(disposing);")
                }
            });

            definition.Methods.Add(new MethodDefinition("Task<IActionResult>", "GetOrdersAsync", new ParameterDefinition("Int32?", "pageSize", "10"), new ParameterDefinition("Int32?", "pageNumber", "1"))
            {
                Attributes =
                {
                    new MetadataAttribute("HttpGet", "\"Order\"")
                },
                IsAsync = true
            });

            definition.Methods.Add(new MethodDefinition("Task<IActionResult>", "GetOrderAsync", new ParameterDefinition("Int32", "id"))
            {
                Attributes =
                {
                    new MetadataAttribute("HttpGet", "\"Order/{id}\"")
                },
                IsAsync = true
            });

            definition.Methods.Add(new MethodDefinition("Task<IActionResult>", "CreateOrderAsync", new ParameterDefinition("OrderViewModel", "value", new MetadataAttribute("FromBody")))
            {
                Attributes =
                {
                    new MetadataAttribute("HttpPost", "\"Order\"")
                },
                IsAsync = true
            });

            definition.Methods.Add(new MethodDefinition("Task<IActionResult>", "UpdateOrderAsync", new ParameterDefinition("Int32", "id"), new ParameterDefinition("OrderViewModel", "value", new MetadataAttribute("FromBody")))
            {
                Attributes =
                {
                    new MetadataAttribute("HttpPut", "\"Order/{id}\"")
                },
                IsAsync = true
            });

            definition.Methods.Add(new MethodDefinition("Task<IActionResult>", "DeleteOrderAsync", new ParameterDefinition("Int32", "id"))
            {
                Attributes =
                {
                    new MetadataAttribute("HttpDelete", "\"Order/{id}\"")
                },
                IsAsync = true
            });

            // Act
            var builder = new CSharpClassBuilder
            {
                ObjectDefinition = definition,
                OutputDirectory = "C:\\Temp\\CatFactory.NetCore",
                ForceOverwrite = true
            };

            builder.CreateFile();
        }
    }
}
