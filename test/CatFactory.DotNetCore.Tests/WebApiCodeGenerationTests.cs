using System.Collections.Generic;
using CatFactory.CodeFactory;
using CatFactory.OOP;
using Xunit;

namespace CatFactory.DotNetCore.Tests
{
    public class WebApiCodeGenerationTests
    {
        [Fact]
        public void TestController()
        {
            // Arrange
            var classDef = new CSharpClassDefinition
            {
                Namespace = "Controllers",
                Name = "SalesController",
                BaseClass = "Controller",
                Attributes = new List<MetadataAttribute>()
                {
                    new MetadataAttribute("Route", "\"api/[controller]\"")
                }
            };

            classDef.Namespaces.Add("System");
            classDef.Namespaces.Add("System.Threading.Tasks");
            classDef.Namespaces.Add("Microsoft.AspNetCore.Mvc");

            classDef.Fields.Add(new FieldDefinition(AccessModifier.Protected, "ISalesRepository", "Repository") { IsReadOnly = true });

            classDef.Constructors.Add(new ClassConstructorDefinition(new ParameterDefinition("ISalesRepository", "repository"))
            {
                Lines = new List<CodeLine>()
                {
                    new CodeLine("Repository = repository;")
                }
            });

            classDef.Methods.Add(new MethodDefinition(AccessModifier.Protected, "void", "Disposed", new ParameterDefinition("Boolean", "disposing"))
            {
                IsOverride = true,
                Lines = new List<CodeLine>()
                {
                    new CodeLine("Repository?.Dispose();"),
                    new CodeLine(),
                    new CodeLine("base.Dispose(disposing);")
                }
            });

            classDef.Methods.Add(new MethodDefinition("Task<IActionResult>", "GetOrdersAsync", new ParameterDefinition("Int32?", "pageSize", "10"))
            {
                Attributes = new List<MetadataAttribute>()
                {
                    new MetadataAttribute("HttpGet"),
                    new MetadataAttribute("Route", "\"Order\"")
                },
                IsAsync = true
            });

            classDef.Methods.Add(new MethodDefinition("Task<IActionResult>", "GetOrderAsync", new ParameterDefinition("Int32", "id"))
            {
                Attributes = new List<MetadataAttribute>()
                {
                    new MetadataAttribute("HttpGet"),
                    new MetadataAttribute("Route", "\"Order/{id}\"")
                },
                IsAsync = true
            });

            classDef.Methods.Add(new MethodDefinition("Task<IActionResult>", "CreateOrderAsync", new ParameterDefinition("OrderViewModel", "value", new MetadataAttribute("FromBody")))
            {
                Attributes = new List<MetadataAttribute>()
                {
                    new MetadataAttribute("HttpPost"),
                    new MetadataAttribute("Route", "\"Order\"")
                },
                IsAsync = true
            });

            classDef.Methods.Add(new MethodDefinition("Task<IActionResult>", "UpdateOrderAsync", new ParameterDefinition("Int32", "id"), new ParameterDefinition("OrderViewModel", "value", new MetadataAttribute("FromBody")))
            {
                Attributes = new List<MetadataAttribute>()
                {
                    new MetadataAttribute("HttpPut"),
                    new MetadataAttribute("Route", "\"Order/{id}\"")
                },
                IsAsync = true
            });

            classDef.Methods.Add(new MethodDefinition("Task<IActionResult>", "DeleteOrderAsync", new ParameterDefinition("Int32", "id"))
            {
                Attributes = new List<MetadataAttribute>()
                {
                    new MetadataAttribute("HttpDelete"),
                    new MetadataAttribute("Route", "\"Order/{id}\"")
                },
                IsAsync = true
            });

            // Act
            var classBuilder = new CSharpClassBuilder()
            {
                ObjectDefinition = classDef,
                OutputDirectory = "C:\\Temp\\CatFactory.DotNetCore"
            };

            classBuilder.CreateFile();

        }
    }
}
