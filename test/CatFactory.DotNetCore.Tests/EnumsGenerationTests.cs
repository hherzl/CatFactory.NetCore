using System.Collections.Generic;
using CatFactory.OOP;
using Xunit;

namespace CatFactory.DotNetCore.Tests
{
    public class EnumsGenerationTests
    {
        [Fact]
        public void TestSimpleEnumGeneration()
        {
            // Arrange
            var definition = new CSharpEnumDefinition
            {
                Name = "OperationMode",
                Sets = new List<INameValue>
                {
                    new NameValue
                    {
                        Name = "First",
                        Value = "0"
                    },
                    new NameValue
                    {
                        Name = "Second",
                        Value = "1"
                    },
                    new NameValue
                    {
                        Name = "Third",
                        Value = "2"
                    }
                }
            };

            // Act
            CSharpEnumBuilder.CreateFiles("C:\\Temp\\CatFactory.DotNetCore", string.Empty, true, definition);
        }

        [Fact]
        public void TestEnumWithFlagsGeneration()
        {
            // Arrange
            var definition = new CSharpEnumDefinition
            {
                Name = "CarOptions",
                Attributes = new List<MetadataAttribute>
                {
                    new MetadataAttribute("Flags")
                },
                Sets = new List<INameValue>
                {
                    new NameValue
                    {
                        Name = "SunRoof",
                        Value = "0x01"
                    },
                    new NameValue
                    {
                        Name = "Spoiler",
                        Value = "0x02"
                    },
                    new NameValue
                    {
                        Name = "FogLights",
                        Value = "0x04"
                    },
                    new NameValue
                    {
                        Name = "TintedWindows",
                        Value = "0x08"
                    }
                }
            };

            // Act
            CSharpEnumBuilder.CreateFiles("C:\\Temp\\CatFactory.DotNetCore", string.Empty, true, definition);
        }
    }
}
