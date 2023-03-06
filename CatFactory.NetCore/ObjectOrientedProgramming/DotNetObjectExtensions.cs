namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public static class DotNetObjectExtensions
    {
        private static string SetType(string source)
        {
            switch (source)
            {
                case "Boolean":
                    return "bool";

                case "Task<Boolean>":
                    return "Task<bool>";

                case "Boolean?":
                case "Nullable<Boolean>":
                    return "bool?";

                case "Byte[]":
                    return "byte[]";

                case "Byte":
                    return "byte";

                case "Byte?":
                case "Nullable<Byte>":
                    return "byte?";

                case "Char":
                    return "char";

                case "Task<Char>":
                    return "Task<char>";

                case "Char?":
                case "Nullable<Char>":
                    return "char?";

                case "Decimal":
                    return "decimal";

                case "Task<Decimal>":
                    return "Task<decimal>";

                case "Decimal?":
                case "Nullable<Decimal>":
                    return "decimal?";

                case "Double":
                    return "double";

                case "Double?":
                case "Nullable<Double?>":
                    return "double?";

                case "Int16":
                    return "short";

                case "Task<Int16>":
                    return "Task<short>";

                case "Int16?":
                case "Nullable<Int16>":
                    return "short?";

                case "Int32":
                    return "int";

                case "Task<Int32>":
                    return "Task<int>";

                case "Int32?":
                case "Nullable<Int32>":
                    return "int?";

                case "Int64":
                    return "long";

                case "Task<Int64>":
                    return "Task<long?";

                case "Int64?":
                case "Nullable<Int64>":
                    return "long?";

                case "Single":
                    return "float";

                case "Task<Single>":
                    return "Task<float>";

                case "Single?":
                case "Nullable<Single>":
                    return "float?";

                case "String":
                    return "string";

                case "Task<String>":
                    return "Task<string>";

                default:
                    return source;
            }
        }

        public static void SimplifyDataTypes(this IDotNetObjectDefinition objDef)
        {
            if (objDef is CSharpClassDefinition classDef)
            {
                foreach (var field in classDef.Fields)
                    field.Type = SetType(field.Type);

                foreach (var property in classDef.Properties)
                    property.Type = SetType(property.Type);

                foreach (var constructor in classDef.Constructors)
                {
                    foreach (var parameter in constructor.Parameters)
                        parameter.Type = SetType(parameter.Type);
                }

                foreach (var method in classDef.Methods)
                {
                    method.Type = SetType(method.Type);

                    foreach (var parameter in method.Parameters)
                        parameter.Type = SetType(parameter.Type);
                }
            }

            if (objDef is CSharpRecordDefinition recordDef)
            {
                foreach (var field in recordDef.Fields)
                    field.Type = SetType(field.Type);

                foreach (var property in recordDef.Properties)
                    property.Type = SetType(property.Type);

                foreach (var constructor in recordDef.Constructors)
                {
                    foreach (var parameter in constructor.Parameters)
                        parameter.Type = SetType(parameter.Type);
                }

                foreach (var method in recordDef.Methods)
                {
                    method.Type = SetType(method.Type);

                    foreach (var parameter in method.Parameters)
                        parameter.Type = SetType(parameter.Type);
                }
            }

            if (objDef is CSharpInterfaceDefinition interfaceDef)
            {
                foreach (var property in interfaceDef.Properties)
                    property.Type = SetType(property.Type);

                foreach (var method in interfaceDef.Methods)
                {
                    method.Type = SetType(method.Type);

                    foreach (var parameter in method.Parameters)
                        parameter.Type = SetType(parameter.Type);
                }
            }
        }
    }
}
