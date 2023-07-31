using CatFactory.NetCore.CodeFactory;
using CatFactory.NetCore.ObjectOrientedProgramming;
using CatFactory.ObjectOrientedProgramming;
using Xunit;

namespace CatFactory.NetCore.Tests;

public class RecordScaffoldingTests : ScaffoldingTest
{
    public RecordScaffoldingTests()
        : base()
    {
    }

    [Fact]
    public void Scaffolding_ProductQueryModel_CreatesFile()
    {
        // Arrange
        var definition = CSharpRecordDefinition
            .Create(AccessModifier.Public, "ProductQueryModel", ns: "Infrastructure.Persistence.QueryModels")
            .ImportNs("System")
            ;

        definition.Properties.Add(CSharpRecordDefinition.CreateAutomaticProp("int?", "Id"));
        definition.Properties.Add(CSharpRecordDefinition.CreateAutomaticProp("string", "Name"));
        definition.Properties.Add(CSharpRecordDefinition.CreateAutomaticProp("int?", "SupplierID"));
        definition.Properties.Add(CSharpRecordDefinition.CreateAutomaticProp("string", "Supplier"));
        definition.Properties.Add(CSharpRecordDefinition.CreateAutomaticProp("int?", "CategoryID"));
        definition.Properties.Add(CSharpRecordDefinition.CreateAutomaticProp("string", "Category"));
        definition.Properties.Add(CSharpRecordDefinition.CreateAutomaticProp("decimal?", "UnitPrice"));

        // Act
        CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, _solutionDirectory, _infrastructureDirectory, _persistenceDirectory), _queryModelsDirectory, true, definition);
    }

    [Fact]
    public void ScaffoldingInvoiceViewModelRecord()
    {
        // Arrange
        var definition = CSharpRecordDefinition
            .Create(AccessModifier.Public, "Invoice", ns: "DesignPatterns")
            .Implement("INotifyPropertyChanged")
            .ImportNs("System")
            .ImportNs("System.ComponentModel")
            ;

        definition.AddViewModelProp("int?", "Id");
        definition.AddViewModelProp("DateTime?", "CreatedOn");

        definition.Events.Add(new(AccessModifier.Public, "PropertyChangedEventHandler", "PropertyChanged"));

        // Act
        CSharpCodeBuilder.CreateFiles(Path.Combine(_baseDirectory, "DesignPatterns"), true, definition);
    }

    [Fact]
    public void RefactInterfaceFromRecord()
    {
        // Arrange
        var recordDefinition = CSharpRecordDefinition
            .Create(AccessModifier.Public, "TeacherViewModel", ns: "DesignPatterns")
            .Implement("INotifyPropertyChanged")
            .ImportNs("System")
            .ImportNs("System.ComponentModel")
            ;

        recordDefinition.AddViewModelProp("Guid?", "Id");
        recordDefinition.AddViewModelProp("string", "GivenName");
        recordDefinition.AddViewModelProp("string", "MiddleName");
        recordDefinition.AddViewModelProp("string", "FamilyName");
        recordDefinition.AddViewModelProp("string", "Email");
        recordDefinition.AddViewModelProp("string", "Phone");

        recordDefinition.Methods.Add(new MethodDefinition(AccessModifier.Public, "void", "Foo"));
        recordDefinition.Methods.Add(new MethodDefinition(AccessModifier.Private, "void", "Bar"));

        // Act
        var interfaceDefinition = recordDefinition.RefactInterface();

        // Assert
        Assert.True(interfaceDefinition.Properties.Count == recordDefinition.Properties.Count);
        Assert.True(interfaceDefinition.Methods.Count == recordDefinition.Methods.Where(item => item.AccessModifier == AccessModifier.Public).Count());
    }
}
