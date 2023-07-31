using CatFactory.NetCore.CodeFactory;
using CatFactory.NetCore.ObjectOrientedProgramming;
using CatFactory.ObjectOrientedProgramming;
using Xunit;

namespace CatFactory.NetCore.Tests
{
    public class RecordScaffoldingTests
    {
        private readonly string _baseDirectory;
        private readonly string _solutionDirectory;
        private readonly string _domainDirectory;
        private readonly string _entitiesDirectory;
        private readonly string _exceptionsDirectory;
        private readonly string _infrastructureDirectory;
        private readonly string _persistenceDirectory;
        private readonly string _queryModelsDirectory;

        public RecordScaffoldingTests()
        {
            _baseDirectory = @"C:\Temp\CatFactory.NetCore";
            _solutionDirectory = "CleanArchitecture";
            _domainDirectory = "Domain";
            _entitiesDirectory = "Entities";
            _exceptionsDirectory = "Exceptions";
            _infrastructureDirectory = "Infrastructure";
            _persistenceDirectory = "Persistence";
            _queryModelsDirectory = "QueryModels";
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
