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
                Namespaces = new List<String>()
                {
                    "System",
                    "System.ComponentModel"
                }
            };

            classDefinition.Attributes.Add(new MetadataAttribute("Table")
            {
                Arguments = new List<String>() { "\"Persons\"" }
            });

            classDefinition.Properties.Add(new PropertyDefinition("Int32?", "ID")
            {
                Attributes = new List<MetadataAttribute>()
                {
                    new MetadataAttribute("Key")
                }
            });

            classDefinition.Properties.Add(new PropertyDefinition("String", "FirstName")
            {
                Attributes = new List<MetadataAttribute>()
                {
                    new MetadataAttribute("Required"),
                    new MetadataAttribute("StringLength")
                    {
                        Arguments = new List<String>() { "25" }
                    }
                }
            });

            classDefinition.Properties.Add(new PropertyDefinition("String", "MiddleName")
            {
                Attributes = new List<MetadataAttribute>()
                {
                    new MetadataAttribute("StringLength")
                    {
                        Arguments = new List<String>() { "25" }
                    }
                }
            });

            classDefinition.Properties.Add(new PropertyDefinition("String", "LastName")
            {
                Attributes = new List<MetadataAttribute>()
                {
                    new MetadataAttribute("Required"),
                    new MetadataAttribute("StringLength")
                    {
                        Arguments = new List<String>() { "25" }
                    }
                }
            });

            classDefinition.Properties.Add(new PropertyDefinition("String", "Gender")
            {
                Attributes = new List<MetadataAttribute>()
                {
                    new MetadataAttribute("Required"),
                    new MetadataAttribute("StringLength")
                    {
                        Arguments = new List<String>() { "1" }
                    }
                }
            });

            classDefinition.Properties.Add(new PropertyDefinition("DateTime?", "BirthDate")
            {
                Attributes = new List<MetadataAttribute>()
                {
                    new MetadataAttribute("Required")
                }
            });

            var classBuilder = new CSharpClassBuilder()
            {
                ObjectDefinition = classDefinition,
                OutputDirectory = "C:\\Temp"
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
            interfaceDefinition.Properties.Add(new PropertyDefinition("String", "Gender"));
            interfaceDefinition.Properties.Add(new PropertyDefinition("DateTime?", "BirthDate"));

            var classBuilder = new CSharpInterfaceBuilder()
            {
                ObjectDefinition = interfaceDefinition,
                OutputDirectory = "C:\\Temp"
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
                    new CodeLine(1, "PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(\"FirstName\"));"),
                    new CodeLine("}}")
                }
            });

            var classBuilder = new CSharpClassBuilder()
            {
                ObjectDefinition = classDefinition,
                OutputDirectory = "C:\\Temp"
            };

            classBuilder.CreateFile();
        }
    }
}
