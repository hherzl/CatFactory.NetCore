using System.Collections.Generic;
using CatFactory.CodeFactory;
using CatFactory.OOP;
using Xunit;

namespace CatFactory.DotNetCore.Tests
{
    public class ClassesGenerationTests
    {
        [Fact]
        public void TestContactClassGeneration()
        {
            // Arrange
            var definition = new CSharpClassDefinition
            {
                Namespaces = new List<string>()
                {
                    "System",
                    "System.ComponentModel"
                },
                Namespace = "ContactManager",
                Name = "Contact",
            };

            definition.Attributes.Add(new MetadataAttribute("Table", "\"Contact\""));

            definition.Properties.Add(new PropertyDefinition("Int32?", "ID", new MetadataAttribute("Key")));
            definition.Properties.Add(new PropertyDefinition("String", "FirstName", new MetadataAttribute("Required"), new MetadataAttribute("StringLength", "25")));
            definition.Properties.Add(new PropertyDefinition("String", "MiddleName", new MetadataAttribute("StringLength", "25")));
            definition.Properties.Add(new PropertyDefinition("String", "LastName", new MetadataAttribute("Required"), new MetadataAttribute("StringLength", "25")));

            // Act
            CSharpClassBuilder.CreateFiles("C:\\Temp\\CatFactory.DotNetCore", string.Empty, true, definition);
        }

        [Fact]
        public void TestPersonClassGeneration()
        {
            // Arrange
            var definition = new CSharpClassDefinition
            {
                Namespaces = new List<string>()
                {
                    "System",
                    "System.ComponentModel"
                },
                Namespace = "ContactManager",
                Name = "Person",
            };

            definition.Attributes.Add(new MetadataAttribute("Table", "\"Persons\"")
            {
                Sets = new List<MetadataAttributeSet>()
                {
                    new MetadataAttributeSet("Schema", "\"PersonalInfo\"")
                }
            });

            definition.Properties.Add(new PropertyDefinition("Int32?", "ID", new MetadataAttribute("Key")) { IsVirtual = true });
            definition.Properties.Add(new PropertyDefinition("String", "FirstName", new MetadataAttribute("Required"), new MetadataAttribute("StringLength", "25")));
            definition.Properties.Add(new PropertyDefinition("String", "MiddleName", new MetadataAttribute("StringLength", "25")));
            definition.Properties.Add(new PropertyDefinition("String", "LastName", new MetadataAttribute("Required"), new MetadataAttribute("StringLength", "25")));
            definition.Properties.Add(new PropertyDefinition("String", "Gender", new MetadataAttribute("Required"), new MetadataAttribute("StringLength", "1")));
            definition.Properties.Add(new PropertyDefinition("DateTime?", "BirthDate", new MetadataAttribute("Required")));

            definition.SimplifyDataTypes();

            // Act
            CSharpClassBuilder.CreateFiles("C:\\Temp\\CatFactory.DotNetCore", string.Empty, true, definition);
        }

        [Fact]
        public void TestCsharpViewModelClassGeneration()
        {
            // Arrange
            var definition = new CSharpClassDefinition();

            definition.Namespaces.Add("System");
            definition.Namespaces.Add("System.ComponentModel");

            definition.Namespace = "ViewModels";
            definition.Name = "MyViewModel";

            definition.Implements.Add("INotifyPropertyChanged");

            definition.Events.Add(new EventDefinition("PropertyChangedEventHandler", "PropertyChanged"));

            definition.Fields.Add(new FieldDefinition(AccessModifier.Private, "String", "m_firstName"));
            definition.Fields.Add(new FieldDefinition(AccessModifier.Private, "String", "m_middleName"));
            definition.Fields.Add(new FieldDefinition(AccessModifier.Private, "String", "m_lastName"));

            definition.Constructors.Add(new ClassConstructorDefinition());
            definition.Constructors.Add(new ClassConstructorDefinition());

            definition.Properties.Add(new PropertyDefinition("String", "FirstName")
            {
                IsAutomatic = false,
                GetBody = new List<ILine>()
                {
                    new CodeLine("return m_firstName;")
                },
                SetBody = new List<ILine>()
                {
                    new CodeLine("if (m_firstName != value)"),
                    new CodeLine("{"),
                    new CodeLine(1, "m_firstName = value;"),
                    new CodeLine(),
                    new CodeLine(1, "PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FirstName)));"),
                    new CodeLine("}")
                }
            });

            definition.Properties.Add(new PropertyDefinition("String", "MiddleName")
            {
                IsAutomatic = false,
                GetBody = new List<ILine>()
                {
                    new CodeLine("return m_middleName;")
                },
                SetBody = new List<ILine>()
                {
                    new CodeLine("if (m_middleName != value)"),
                    new CodeLine("{"),
                    new CodeLine(1, "m_middleName = value;"),
                    new CodeLine(),
                    new CodeLine(1, "PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MiddleName)));"),
                    new CodeLine("}")
                }
            });

            definition.Properties.Add(new PropertyDefinition("String", "LastName")
            {
                IsAutomatic = false,
                GetBody = new List<ILine>()
                {
                    new CodeLine("return m_firstName;")
                },
                SetBody = new List<ILine>()
                {
                    new CodeLine("if (m_lastName != value)"),
                    new CodeLine("{"),
                    new CodeLine(1, "m_lastName = value;"),
                    new CodeLine(),
                    new CodeLine(1, "PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastName));"),
                    new CodeLine("}")
                }
            });

            definition.Properties.Add(new PropertyDefinition("String", "FullName")
            {
                IsVirtual = true,
                IsAutomatic = false,
                IsReadOnly = true,
                GetBody = new List<ILine>()
                {
                    new CodeLine("return String.Format(\"{0} {1} {2}\", FirstName, MiddleName, LastName);")
                }
            });

            // Act
            CSharpClassBuilder.CreateFiles("C:\\Temp\\CatFactory.DotNetCore", string.Empty, true, definition);
        }

        [Fact]
        public void TestCsharpViewModelClassGenerationWithExtensionMethods()
        {
            // Arrange
            var definition = new CSharpClassDefinition();

            definition.Namespaces.Add("System");
            definition.Namespaces.Add("System.ComponentModel");

            definition.Namespace = "ViewModels";
            definition.Name = "ProductViewModel";

            definition.Implements.Add("INotifyPropertyChanged");

            definition.Events.Add(new EventDefinition("PropertyChangedEventHandler", "PropertyChanged"));

            definition.AddViewModelProperty("Int32?", "ProductId");
            definition.AddViewModelProperty("String", "ProductName");
            definition.AddViewModelProperty("Int32?", "ProductCategoryID");
            definition.AddViewModelProperty("Decimal?", "UnitPrice");
            definition.AddViewModelProperty("String", "ProductDescription");

            // Act
            CSharpClassBuilder.CreateFiles("C:\\Temp\\CatFactory.DotNetCore", string.Empty, true, definition);
        }

        [Fact]
        public void TestCsharpRepositoryClassGeneration()
        {
            // Arrange
            var definition = new CSharpClassDefinition();

            definition.Namespaces.Add("System");
            definition.Namespaces.Add("System.Collections.Generic");

            definition.Namespace = "Repositories";
            definition.Name = "DboRepository";

            definition.Implements.Add("IRepository");

            definition.Fields.Add(new FieldDefinition(AccessModifier.Private, "DbContext", "m_dbContext"));

            definition.Constructors.Add(new ClassConstructorDefinition());

            definition.AddReadOnlyProperty("DbContext", "DbContext", new CodeLine("return m_dbContext;"));

            definition.Properties.Add(new PropertyDefinition("IUserInfo", "UserInfo"));

            definition.Methods.Add(new MethodDefinition("Task<Int32>", "CommitChangesAsync")
            {
                IsAsync = true,
                Lines = new List<ILine>()
                {
                    new CommentLine("Save changes in async way"),
                    new CodeLine("return await DbContext.SaveChangesAsync();")
                }
            });

            definition.Methods.Add(new MethodDefinition("IEnumerable<TEntity>", "GetAll")
            {
                Lines = new List<ILine>()
                {
                    new TodoLine("Check generic cast"),
                    new CodeLine("return DbContext.Set<TEntity>();")
                }
            });

            definition.Methods.Add(new MethodDefinition("void", "Add", new ParameterDefinition("TEntity", "entity"))
            {
                Lines = new List<ILine>()
                {
                    new CommentLine("Check entry state"),
                    new CodeLine("DbContext.Set<TEntity>().Add(entity);"),
                    new CodeLine(),
                    new TodoLine("Save changes in async way"),
                    new CodeLine("DbContext.SaveChanges();")
                }
            });

            // Act
            CSharpClassBuilder.CreateFiles("C:\\Temp\\CatFactory.DotNetCore", string.Empty, true, definition);
        }

        [Fact]
        public void TestCsharpClassWithMethodsGeneration()
        {
            // Arrange
            var definition = new CSharpClassDefinition();

            definition.Namespaces.Add("System");

            definition.Namespace = "Operations";
            definition.Name = "Helpers";

            definition.Methods.Add(new MethodDefinition("void", "Foo", new ParameterDefinition("int", "foo", "0"), new ParameterDefinition("int", "bar", "1"))
            {
                IsAsync = true
            });

            definition.Methods.Add(new MethodDefinition("void", "Bar", new ParameterDefinition("int", "a")));

            definition.Methods.Add(new MethodDefinition("int", "Zaz")
            {
                Lines = new List<ILine>()
                {
                    new CodeLine("return 0;")
                }
            });

            definition.Methods.Add(new MethodDefinition("int", "Qux")
            {
                GenericType = "T",
                WhereConstraints = new List<string>()
                {
                    "T : class"
                },
                Lines = new List<ILine>()
                {
                    new CodeLine("return 0;")
                }
            });

            // Act
            CSharpClassBuilder.CreateFiles("C:\\Temp\\CatFactory.DotNetCore", string.Empty, true, definition);
        }

        [Fact]
        public void TestCSharpStaticClassWithExtensionMethods()
        {
            // Arrange
            var definition = new CSharpClassDefinition
            {
                Namespaces = new List<string>()
                {
                    "System"
                },
                Namespace = "Helpers",
                Name = "Extensions",
                IsStatic = true
            };

            definition.Methods.Add(new MethodDefinition("void", "Foo", new ParameterDefinition("int", "foo", "0"), new ParameterDefinition("int", "bar", "1"))
            {
                IsStatic = true,
                IsExtension = true
            });

            definition.Methods.Add(new MethodDefinition("String", "Bar", new ParameterDefinition("long", "x", "0"))
            {
                IsStatic = true,
                IsExtension = true,
                GenericType = "TModel",
                WhereConstraints = new List<string>()
                {
                    "TModel : class"
                }
            });

            // Act
            CSharpClassBuilder.CreateFiles("C:\\Temp\\CatFactory.DotNetCore", string.Empty, true, definition);
        }

        [Fact]
        public void TestCSharpClassWithFinalizer()
        {
            // Arrange
            var definition = new CSharpClassDefinition
            {
                Namespaces = new List<string>()
                {
                    "System.Data.Common"
                },
                Namespace = "Infrastructure",
                Name = "DbContext",
                IsStatic = true
            };

            definition.Fields.Add(new FieldDefinition(AccessModifier.Private, "IDbConnection", "Connection") { IsStatic = true });

            definition.StaticConstructor = new ClassConstructorDefinition
            {
                Lines = new List<ILine>()
                {
                    new CodeLine("Connection = DbFactory.GetDefaultInstance();")
                }
            };

            definition.Finalizer = new FinalizerDefinition
            {
                Lines = new List<ILine>()
                {
                    new CodeLine("Connection.Dispose();")
                }
            };

            // Act
            CSharpClassBuilder.CreateFiles("C:\\Temp\\CatFactory.DotNetCore", string.Empty, true, definition);
        }

        [Fact]
        public void TestCSharpClassWithIndexer()
        {
            // Arrange
            var definition = new CSharpClassDefinition
            {
                Namespaces = new List<string>()
                {
                    "System",
                    "System.Collections.Generic"
                },
                Namespace = "Mapping",
                Name = "DataSet"
            };

            definition.Indexers.Add(new Indexer
            {
                AccessModifier = AccessModifier.Public,
                Type = "string",
                Parameters = new List<ParameterDefinition>()
                {
                    new ParameterDefinition
                    {
                        Type = "int",
                        Name = "index"
                    }
                },
                GetBody = new List<ILine>()
                {
                    new CodeLine("return m_columns[index];")
                },
                SetBody = new List<ILine>()
                {
                    new CodeLine("m_columns = value;")
                }
            });

            definition.AddPropertyWithField("List<string>", "Columns");

            // Act
            CSharpClassBuilder.CreateFiles("C:\\Temp\\CatFactory.DotNetCore", string.Empty, true, definition);
        }
    }
}
