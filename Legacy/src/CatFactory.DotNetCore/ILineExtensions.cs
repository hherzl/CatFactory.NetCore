using CatFactory.CodeFactory;

namespace CatFactory.DotNetCore
{
    public static class ILineExtensions
    {
        public static bool IsComment(this ILine line)
            => line is CommentLine;

        public static bool IsPreprocessorDirective(this ILine line)
            => line is PreprocessorDirectiveLine;

        public static bool IsTodo(this ILine line)
            => line is TodoLine;
    }
}
