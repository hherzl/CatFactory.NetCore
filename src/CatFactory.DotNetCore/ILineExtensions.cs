using System;
using CatFactory.CodeFactory;

namespace CatFactory.DotNetCore
{
    public static class ILineExtensions
    {
        public static Boolean IsComment(this ILine line)
        {
            return line is CommentLine;
        }

        public static Boolean IsTodo(this ILine line)
        {
            return line is TodoLine;
        }

        public static Boolean IsWarning(this ILine line)
        {
            return line is WarningLine;
        }
    }
}
