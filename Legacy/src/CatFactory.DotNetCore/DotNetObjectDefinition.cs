namespace CatFactory.DotNetCore
{
    public static class DotNetObjectDefinition
    {
        private static string SetType(string source)
        {
            switch (source)
            {
                case "Boolean":
                    return "bool";

                case "Boolean?":
                case "Nullable<Boolean>":
                    return "bool?";

                case "Byte":
                    return "byte";

                case "Byte?":
                case "Nullable<Byte>":
                    return "byte?";

                case "Char":
                    return "char";

                case "Char?":
                case "Nullable<Char>":
                    return "char?";

                case "Decimal":
                    return "decimal";

                case "Decimal?":
                case "Nullable<Decimal>":
                    return "decimal?";

                case "Int16":
                    return "short";

                case "Int16?":
                case "Nullable<Int16>":
                    return "short?";

                case "Int32":
                    return "int";

                case "Int32?":
                case "Nullable<Int32>":
                    return "int";

                case "Int64":
                    return "long";

                case "Int64?":
                case "Nullable<Int64>":
                    return "long?";

                case "Single":
                    return "float";

                case "Single?":
                case "Nullable<Single>":
                    return "float?";

                case "String":
                    return "string";

                default:
                    return source;
            }
        }

        public static void SimplifyDataTypes(this IDotNetObjectDefinition objectDefinition)
        {
            var classDef = objectDefinition as CSharpClassDefinition;

            if (classDef != null)
            {
                foreach (var field in classDef.Fields)
                {
                    field.Type = SetType(field.Type);
                }

                foreach (var property in classDef.Properties)
                {
                    property.Type = SetType(property.Type);
                }

                foreach (var constructor in classDef.Constructors)
                {
                    foreach (var parameter in constructor.Parameters)
                    {
                        parameter.Type = SetType(parameter.Type);
                    }
                }

                foreach (var method in classDef.Methods)
                {
                    method.Type = SetType(method.Type);

                    foreach (var parameter in method.Parameters)
                    {
                        parameter.Type = SetType(parameter.Type);
                    }
                }
            }

            var interfaceDef = objectDefinition as CSharpInterfaceDefinition;

            if (interfaceDef != null)
            {
                foreach (var property in interfaceDef.Properties)
                {
                    property.Type = SetType(property.Type);
                }

                foreach (var method in interfaceDef.Methods)
                {
                    method.Type = SetType(method.Type);

                    foreach (var parameter in method.Parameters)
                    {
                        parameter.Type = SetType(parameter.Type);
                    }
                }
            }
        }
    }
}
