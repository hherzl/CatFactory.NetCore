using CatFactory.OOP;
using Xunit;

namespace CatFactory.NetCore.Tests
{
    public class InterfacesGenerationTests
    {
        [Fact]
        public void TestCSharpInterfaceGeneration()
        {
            // Arrange
            var definition = new CSharpInterfaceDefinition
            {
                Namespace = "ContactManager",
                Name = "IPerson",
                Namespaces =
                {
                    "System",
                    "System.ComponentModel"
                }
            };

            definition.Properties.Add(new PropertyDefinition("Int32?", "ID"));
            definition.Properties.Add(new PropertyDefinition("String", "FirstName"));
            definition.Properties.Add(new PropertyDefinition("String", "MiddleName"));
            definition.Properties.Add(new PropertyDefinition("String", "LastName"));
            definition.Properties.Add(new PropertyDefinition("String", "FullName") { IsReadOnly = true });
            definition.Properties.Add(new PropertyDefinition("String", "Gender"));
            definition.Properties.Add(new PropertyDefinition("DateTime?", "BirthDate"));
            definition.Properties.Add(new PropertyDefinition("Int32", "Age") { IsReadOnly = true });

            // Act
            CSharpCodeBuilder.CreateFiles("C:\\Temp\\CatFactory.NetCore", string.Empty, true, definition);
        }

        [Fact]
        public void TestCSharpContractInterfaceGeneration()
        {
            // Arrange
            var definition = new CSharpInterfaceDefinition
            {
                Namespace = "Contracts",
                Name = "IRepository",
                Namespaces =
                {
                    "System",
                    "System.Threading.Tasks"
                }
            };

            definition.Properties.Add(new PropertyDefinition("DbContext", "DbContext") { IsReadOnly = true });

            definition.Methods.Add(new MethodDefinition("Task<Int32>", "CommitChanges", new ParameterDefinition("int", "foo", "0")));

            definition.Methods.Add(new MethodDefinition("Task<Int32>", "CommitChangesAsync"));

            definition.Methods.Add(new MethodDefinition("TEntity", "Audit")
            {
                GenericTypes =
                {
                    new GenericTypeDefinition
                    {
                        Name = "TEntity",
                        Constraint = "TEntity : class, new()"
                    }
                }
            });

            // Act
            CSharpCodeBuilder.CreateFiles("C:\\Temp\\CatFactory.NetCore", string.Empty, true, definition);
        }

        [Fact]
        public void TestCSharpGenericInterfaceGeneration()
        {
            // Arrange
            var definition = new CSharpInterfaceDefinition
            {
                Namespaces =
                {
                    "System",
                    "System.Threading.Tasks"
                },
                Namespace = "Contracts",
                IsPartial = true,
                Name = "IGenericRepository",
                GenericTypes =
                {
                    new GenericTypeDefinition
                    {
                        Name = "TEntity",
                        Constraint = "TEntity : class, new()"
                    }
                }
            };

            definition.Properties.Add(new PropertyDefinition("DbContext", "DbContext") { IsReadOnly = true });

            definition.Methods.Add(new MethodDefinition("Task<Int32>", "CommitChanges", new ParameterDefinition("int", "foo", "0")));

            definition.Methods.Add(new MethodDefinition("Task<Int32>", "CommitChangesAsync"));

            definition.Methods.Add(new MethodDefinition("void", "Audit")
            {
                GenericTypes =
                {
                    new GenericTypeDefinition
                    {
                        Name = "TEntity"
                    }
                }
            });

            // Act
            CSharpCodeBuilder.CreateFiles("C:\\Temp\\CatFactory.NetCore", string.Empty, true, definition);
        }
    }
}
