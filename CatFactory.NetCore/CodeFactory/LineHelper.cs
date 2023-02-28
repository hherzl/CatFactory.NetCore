using CatFactory.CodeFactory;

namespace CatFactory.NetCore.CodeFactory
{
    public static class LineHelper
    {
        public static PreprocessorDirectiveLine Define(string message, params string[] args)
            => new("define");

        public static PreprocessorDirectiveLine Elif(string message, params string[] args)
            => new("elif");

        public static PreprocessorDirectiveLine Else(string message, params string[] args)
            => new("else");

        public static PreprocessorDirectiveLine EndIf()
            => new("endif");

        public static PreprocessorDirectiveLine EndRegion()
            => new("endregion");

        public static PreprocessorDirectiveLine If(string message, params string[] args)
            => new("if");

        public static PreprocessorDirectiveLine Region(string description, params string[] args)
            => new(string.Concat("region ", description), args);

        public static PreprocessorDirectiveLine Undef(string description, params string[] args)
            => new(string.Concat("undef ", description), args);

        public static PreprocessorDirectiveLine Warning(string message, params string[] args)
            => new(string.Concat("warning ", message), args);
    }
}
