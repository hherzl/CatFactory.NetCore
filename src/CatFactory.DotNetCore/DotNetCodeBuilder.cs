using System;
using CatFactory.CodeFactory;

namespace CatFactory.DotNetCore
{
    public class DotNetCodeBuilder : CodeBuilder
    {
        public DotNetCodeBuilder()
        {
            EventsRegionDescription = "[ Events ]";
            FieldsRegionDescription = "[ Fields ]";
            ConstructorsRegionDescription = "[ Constructor ]";
            PropertiesRegionDescription = "[ Properties ]";
            MethodsRegionDescription = "[ Methods ]";
        }

        public String EventsRegionDescription { get; set; }

        public String FieldsRegionDescription { get; set; }

        public String ConstructorsRegionDescription { get; set; }

        public String PropertiesRegionDescription { get; set; }

        public String MethodsRegionDescription { get; set; }
    }
}
