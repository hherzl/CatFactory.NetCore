using System.Linq;
using CatFactory.NetCore.CodeFactory;
using CatFactory.NetCore.ObjectOrientedProgramming;
using CatFactory.ObjectOrientedProgramming;
using Xunit;

namespace CatFactory.NetCore.Tests
{
    public class RecordScaffoldingTests
    {
        [Fact]
        public void ScaffoldingStudentRecord()
        {
            // Arrange
            var definition = new CSharpRecordDefinition
            {
                Namespaces =
                {
                    "System",
                    "System.ComponentModel.DataAnnotations",
                    "System.ComponentModel.DataAnnotations.Schema"
                },
                Namespace = "DesignPatterns",
                AccessModifier = AccessModifier.Public,
                Name = "Student"
            };

            definition.Properties.Add(
                CSharpClassDefinition.CreateAutomaticProperty("int?", "Id", attributes: new[]
                {
                    new MetadataAttribute("Key"),
                    new MetadataAttribute("DatabaseGenerated", "DatabaseGeneratedOption.Identity")
                })
            );

            definition.Properties.Add(
                CSharpClassDefinition.CreateAutomaticProperty("string", "GivenName", attributes: new[]
                {
                    new MetadataAttribute("Required"),
                    new MetadataAttribute("StringLength", "20")
                })
            );

            definition.Properties.Add(
                CSharpClassDefinition.CreateAutomaticProperty("string", "MiddleName", attributes: new[]
                {
                    new MetadataAttribute("Required"),
                    new MetadataAttribute("StringLength", "20")
                })
            );

            definition.Properties.Add(
                CSharpClassDefinition.CreateAutomaticProperty("string", "FamilyName", attributes: new[]
                {
                    new MetadataAttribute("Required"),
                    new MetadataAttribute("StringLength", "20")
                })
            );

            // Act
            CSharpCodeBuilder.CreateFiles(@"C:\Temp\CatFactory.NetCore\DesignPatterns", string.Empty, true, definition);
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
