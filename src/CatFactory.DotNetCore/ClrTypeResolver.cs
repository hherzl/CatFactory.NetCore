using System;

namespace CatFactory.DotNetCore
{
    public class ClrTypeResolver : ITypeResolver
    {
        public ClrTypeResolver()
        {
            UseNullableTypes = true;
        }

        public Boolean UseNullableTypes { get; set; }

        public virtual String Resolve(String type)
        {
            var value = String.Empty;

            switch (type)
            {
                case "char":
                case "varchar":
                case "text":
                case "nchar":
                case "nvarchar":
                case "ntext":
                    value = "String";
                    break;

                case "money":
                case "decimal":
                case "numeric":
                    value = "Decimal";
                    break;

                case "real":
                    value = "Single";
                    break;

                case "tinyint":
                    value = "Byte";
                    break;

                case "image":
                    value = "Byte[]";
                    break;

                case "smallint":
                    value = "Int16";
                    break;

                case "int":
                    value = "Int32";
                    break;

                case "bigint":
                    value = "Int64";
                    break;

                case "uniqueidentifier":
                    value = "Guid";
                    break;

                case "bit":
                    value = "Boolean";
                    break;

                case "xml":
                    value = "String";
                    break;

                case "datetime":
                case "datetime2":
                    value = "DateTime";
                    break;

                default:
                    break;
            }

            if (String.Compare("BYTE[]", value, true) == 0)
            {
                return value;
            }
            else if (String.Compare("STRING", value, true) == 0)
            {
                return value;
            }
            else
            {
                return UseNullableTypes ? String.Format("{0}?", value) : value;
            }
        }
    }
}
