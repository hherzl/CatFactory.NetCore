using CatFactory.CodeFactory;

namespace CatFactory.DotNetCore
{
    public static class LineHelper
    {
        public static PreprocessorDirectiveLine Define(string message, params string[] args)
            => new PreprocessorDirectiveLine("define");

        public static PreprocessorDirectiveLine Elif(string message, params string[] args)
            => new PreprocessorDirectiveLine("elif");

        public static PreprocessorDirectiveLine Else(string message, params string[] args)
            => new PreprocessorDirectiveLine("else");

        public static PreprocessorDirectiveLine EndIf()
            => new PreprocessorDirectiveLine("endif");

        public static PreprocessorDirectiveLine EndRegion()
            => new PreprocessorDirectiveLine("endregion");

        public static PreprocessorDirectiveLine If(string message, params string[] args)
            => new PreprocessorDirectiveLine("if");

        public static PreprocessorDirectiveLine Region(string description, params string[] args)
            => new PreprocessorDirectiveLine(string.Concat("region ", description), args);

        public static PreprocessorDirectiveLine Undef(string description, params string[] args)
            => new PreprocessorDirectiveLine(string.Concat("undef ", description), args);

        public static PreprocessorDirectiveLine Warning(string message, params string[] args)
            => new PreprocessorDirectiveLine(string.Concat("warning ", message), args);
    }
}
