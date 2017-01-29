using System;
using System.Collections.Generic;
using CatFactory.CodeFactory;
using CatFactory.OOP;
using Xunit;

namespace CatFactory.DotNetCore.Tests
{
    public class CodeGenerationTests
    {
        [Fact]
        public void TestCSharpClassGeneration()
        {
            var classDefinition = new CSharpClassDefinition()
            {
                Namespace = "ContactManager",
                Name = "Person",
            };

            classDefinition.Namespaces.Add("System");
            classDefinition.Namespaces.Add("System.ComponentModel");

            classDefinition.Attributes.Add(new MetadataAttribute("Table", "\"Persons\""));

            classDefinition.Properties.Add(new PropertyDefinition("Int32?", "ID", new MetadataAttribute("Key")));
            classDefinition.Properties.Add(new PropertyDefinition("String", "FirstName", new MetadataAttribute("Required"), new MetadataAttribute("StringLength", "25")));
            classDefinition.Properties.Add(new PropertyDefinition("String", "MiddleName", new MetadataAttribute("StringLength", "25")));
            classDefinition.Properties.Add(new PropertyDefinition("String", "LastName", new MetadataAttribute("Required"), new MetadataAttribute("StringLength", "25")));
            classDefinition.Properties.Add(new PropertyDefinition("String", "Gender", new MetadataAttribute("Required"), new MetadataAttribute("StringLength", "1")));
            classDefinition.Properties.Add(new PropertyDefinition("DateTime?", "BirthDate", new MetadataAttribute("Required")));

            var classBuilder = new CSharpClassBuilder()
            {
                ObjectDefinition = classDefinition,
                OutputDirectory = "C:\\Temp\\CatFactory.DotNetCore"
            };

            classBuilder.CreateFile();
        }

        [Fact]
        public void TestCSharpInterfaceGeneration()
        {
            var interfaceDefinition = new CSharpInterfaceDefinition()
            {
                Namespace = "ContactManager",
                Name = "IPerson",
                Namespaces = new List<String>()
                {
                    "System",
                    "System.ComponentModel"
                }
            };

            interfaceDefinition.Properties.Add(new PropertyDefinition("Int32?", "ID"));
            interfaceDefinition.Properties.Add(new PropertyDefinition("String", "FirstName"));
            interfaceDefinition.Properties.Add(new PropertyDefinition("String", "MiddleName"));
            interfaceDefinition.Properties.Add(new PropertyDefinition("String", "LastName"));
            interfaceDefinition.Properties.Add(new PropertyDefinition("String", "FullName") { IsReadOnly = true });
            interfaceDefinition.Properties.Add(new PropertyDefinition("String", "Gender"));
            interfaceDefinition.Properties.Add(new PropertyDefinition("DateTime?", "BirthDate"));
            interfaceDefinition.Properties.Add(new PropertyDefinition("Int32", "Age") { IsReadOnly = true });

            var classBuilder = new CSharpInterfaceBuilder()
            {
                ObjectDefinition = interfaceDefinition,
                OutputDirectory = "C:\\Temp\\CatFactory.DotNetCore"
            };

            classBuilder.CreateFile();
        }

        [Fact]
        public void TestCsharpViewModelClassGeneration()
        {
            var classDefinition = new CSharpClassDefinition();

            classDefinition.Namespace = "ViewModels";
            classDefinition.Name = "MyViewModel";

            classDefinition.Namespaces.Add("System");
            classDefinition.Namespaces.Add("System.ComponentModel");

            classDefinition.Implements.Add("INotifyPropertyChanged");

            classDefinition.Events.Add(new EventDefinition("PropertyChangedEventHandler", "PropertyChanged"));

            classDefinition.Fields.Add(new FieldDefinition("String", "m_firstName") { AccessModifier = AccessModifier.Private });
            classDefinition.Fields.Add(new FieldDefinition("String", "m_lastName") { AccessModifier = AccessModifier.Private });

            classDefinition.Properties.Add(new PropertyDefinition("String", "FirstName")
            {
                IsAutomatic = false,
                GetBody = new List<CodeLine>()
                {
                    new CodeLine("return m_firstName;")
                },
                SetBody = new List<CodeLine>()
                {
                    new CodeLine("if (m_firstName != value)"),
                    new CodeLine("{{"),
                    new CodeLine(1, "m_firstName = value;"),
                    new CodeLine(),
                    new CodeLine(1, "PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FirstName)));"),
                    new CodeLine("}}")
                }
            });

            classDefinition.Properties.Add(new PropertyDefinition("String", "LastName")
            {
                IsAutomatic = false,
                GetBody = new List<CodeLine>()
                {
                    new CodeLine("return m_firstName;")
                },
                SetBody = new List<CodeLine>()
                {
                    new CodeLine("if (m_lastName != value)"),
                    new CodeLine("{{"),
                    new CodeLine(1, "m_lastName = value;"),
                    new CodeLine(),
                    new CodeLine(1, "PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastName));"),
                    new CodeLine("}}")
                }
            });

            classDefinition.Properties.Add(new PropertyDefinition("String", "FullName")
            {
                IsAutomatic = false,
                IsReadOnly = true,
                GetBody = new List<CodeLine>()
                {
                    new CodeLine("return String.Format(\"{{0}} {{1}}\", FirstName, LastName);")
                }
            });

            var classBuilder = new CSharpClassBuilder()
            {
                ObjectDefinition = classDefinition,
                OutputDirectory = "C:\\Temp\\CatFactory.DotNetCore"
            };

            classBuilder.CreateFile();
        }

        [Fact]
        public void TestCsharpViewModelClassGenerationWithExtensionMethods()
        {
            var classDefinition = new CSharpClassDefinition();

            classDefinition.Namespace = "ViewModels";
            classDefinition.Name = "ProductViewModel";

            classDefinition.Namespaces.Add("System");
            classDefinition.Namespaces.Add("System.ComponentModel");

            classDefinition.Implements.Add("INotifyPropertyChanged");

            classDefinition.Events.Add(new EventDefinition("PropertyChangedEventHandler", "PropertyChanged"));

            classDefinition.AddViewModelProperty("Int32?", "ProductId");
            classDefinition.AddViewModelProperty("String", "ProductName");
            classDefinition.AddViewModelProperty("Int32?", "ProductCategoryID");
            classDefinition.AddViewModelProperty("Decimal?", "UnitPrice");
            classDefinition.AddViewModelProperty("String", "ProductDescription");

            var classBuilder = new CSharpClassBuilder()
            {
                ObjectDefinition = classDefinition,
                OutputDirectory = "C:\\Temp\\CatFactory.DotNetCore"
            };

            classBuilder.CreateFile();
        }

        [Fact]
        public void TestCsharpRepositoryClassGeneration()
        {
            var classDefinition = new CSharpClassDefinition();

            classDefinition.Namespace = "Repositories";
            classDefinition.Name = "DboRepository";

            classDefinition.Namespaces.Add("System");
            classDefinition.Namespaces.Add("System.Collections.Generic");

            classDefinition.Implements.Add("IRepository");

            classDefinition.Fields.Add(new FieldDefinition("DbContext", "m_dbContext") { AccessModifier = AccessModifier.Private });

            classDefinition.Constructors.Add(new ClassConstructorDefinition());

            classDefinition.AddReadOnlyProperty("DbContext", "DbContext", new CodeLine("return m_dbContext;"));

            classDefinition.Properties.Add(new PropertyDefinition("IUserInfo", "UserInfo"));

            classDefinition.Methods.Add(new MethodDefinition("Task<Int32>", "CommitChangesAsync")
            {
                IsAsync = true,
                Lines = new List<CodeLine>()
                {
                    new CodeLine("return await DbContext.SaveChangesAsync();")
                }
            });

            classDefinition.Methods.Add(new MethodDefinition("IEnumerable<TEntity>", "GetAll")
            {
                Lines = new List<CodeLine>()
                {
                    new CodeLine("return DbContext.Set<TEntity>();")
                }
            });

            classDefinition.Methods.Add(new MethodDefinition("void", "Add")
            {
                Parameters = new List<ParameterDefinition>()
                {
                    new ParameterDefinition("TEntity", "entity")
                },
                Lines = new List<CodeLine>()
                {
                    new CodeLine("DbContext.Set<TEntity>().Add(entity);"),
                    new CodeLine(),
                    new CodeLine("DbContext.SaveChanges();")
                }
            });

            var classBuilder = new CSharpClassBuilder()
            {
                ObjectDefinition = classDefinition,
                OutputDirectory = "C:\\Temp\\CatFactory.DotNetCore"
            };

            classBuilder.CreateFile();
        }

        [Fact]
        public void TestCsharpClassWithMethodsGeneration()
        {
            var classDefinition = new CSharpClassDefinition();

            classDefinition.Namespaces.Add("System");

            classDefinition.Namespace = "Operations";
            classDefinition.Name = "Helpers";

            classDefinition.Methods.Add(new MethodDefinition("void", "Foo")
            {
                IsAsync = true,
                Parameters = new List<ParameterDefinition>()
                {
                    new ParameterDefinition("int", "foo") { DefaultValue = "0" },
                    new ParameterDefinition("int", "bar") { DefaultValue = "1" }
                }
            });

            classDefinition.Methods.Add(new MethodDefinition("void", "Bar")
            {
                Parameters = new List<ParameterDefinition>()
                {
                    new ParameterDefinition("int", "a")
                }
            });

            classDefinition.Methods.Add(new MethodDefinition("int", "Zaz")
            {
                Lines = new List<CodeLine>()
                {
                    new CodeLine("return 0;")
                }
            });

            var classBuilder = new CSharpClassBuilder()
            {
                ObjectDefinition = classDefinition,
                OutputDirectory = "C:\\Temp\\CatFactory.DotNetCore"
            };

            classBuilder.CreateFile();
        }
    }
}
