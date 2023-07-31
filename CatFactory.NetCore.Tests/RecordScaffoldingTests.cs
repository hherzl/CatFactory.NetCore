using CatFactory.NetCore.CodeFactory;
using CatFactory.NetCore.ObjectOrientedProgramming;
using CatFactory.ObjectOrientedProgramming;
using Xunit;

namespace CatFactory.NetCore.Tests
{
    public class RecordScaffoldingTests : ScaffoldingTest
    {
        public RecordScaffoldingTests()
            : base()
        {
        }

        [Fact]
        public void ScaffoldingProductQueryModelRecord()
        {
            // Arrange
            var definition = new CSharpRecordDefinition
            {
                Namespaces =
                {
                    "System"
                },
                Namespace = "Infrastructure.Persistence.QueryModels",
                AccessModifier = AccessModifier.Public,
                Name = "ProductQueryModel"
            };

            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProperty("int?", "Id"));
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProperty("string", "Name"));
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProperty("int?", "SupplierID"));
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProperty("string", "Supplier"));
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProperty("int?", "CategoryID"));
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProperty("string", "Category"));
            definition.Properties.Add(CSharpClassDefinition.CreateAutomaticProperty("decimal?", "UnitPrice"));

            // Act
            CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, _solutionDirectory, _infrastructureDirectory, _persistenceDirectory), _queryModelsDirectory, true, definition);
        }

        [Fact]
        public void ScaffoldingInvoiceViewModelRecord()
        {
            // Arrange
            var definition = new CSharpRecordDefinition
            {
                Namespaces =
                {
                    "System",
                    "System.ComponentModel"
                },
                Namespace = "DesignPatterns",
                AccessModifier = AccessModifier.Public,
                Name = "Invoice",
                Implements =
                {
                    "INotifyPropertyChanged"
                },
                Events =
                {
                    new EventDefinition("PropertyChangedEventHandler", "PropertyChanged")
                    {
                        AccessModifier = AccessModifier.Public
                    }
                }
            };

            definition.AddViewModelProperty("int?", "Id");
            definition.AddViewModelProperty("DateTime?", "CreatedOn");

            // Act
            CSharpCodeBuilder.CreateFiles(@"C:\Temp\CatFactory.NetCore\DesignPatterns", string.Empty, true, definition);
        }

        [Fact]
        public void RefactInterfaceFromRecord()
        {
            // Arrange
            var recordDefinition = new CSharpRecordDefinition
            {
                Namespaces =
                {
                    "System",
                    "System.ComponentModel"
                },
                Namespace = "DesignPatterns",
                AccessModifier = AccessModifier.Public,
                Name = "TeacherViewModel"
            };

            recordDefinition.AddViewModelProperty("Guid?", "Id");
            recordDefinition.AddViewModelProperty("string", "GivenName");
            recordDefinition.AddViewModelProperty("string", "MiddleName");
            recordDefinition.AddViewModelProperty("string", "FamilyName");
            recordDefinition.AddViewModelProperty("string", "Email");

            recordDefinition.Methods.Add(new MethodDefinition(AccessModifier.Public, "void", "Foo"));
            recordDefinition.Methods.Add(new MethodDefinition(AccessModifier.Private, "void", "Bar"));

            // Act
            var interfaceDefinition = recordDefinition.RefactInterface();

            // Assert
            Assert.True(interfaceDefinition.Properties.Count == recordDefinition.Properties.Count);
            Assert.True(interfaceDefinition.Methods.Count == recordDefinition.Methods.Where(item => item.AccessModifier == AccessModifier.Public).Count());
        }
    }
}
