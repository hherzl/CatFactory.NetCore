using System;
using CatFactory.CodeFactory;

namespace CatFactory.DotNetCore
{
    public class DotNetCodeBuilder : CodeBuilder
    {
        public DotNetCodeBuilder()
        {
            FieldsRegionDescription = "[ Fields ]";
            ConstructorsRegionDescription = "[ Constructor ]";
            PropertiesRegionDescription = "[ Properties ]";
            MethodsRegionDescription = "[ Methods ]";
        }

        public String FieldsRegionDescription { get; set; }

        public String ConstructorsRegionDescription { get; set; }

        public String PropertiesRegionDescription { get; set; }

        public String MethodsRegionDescription { get; set; }
    }
}
