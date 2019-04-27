using System.Collections.Generic;
using ExpressionParser.Lex;
using ExpressionParser.SyntaxTree;

namespace ExpressionParser.Parser
{
    public static class Parser
    {
        public static Expression ParseExpression(string expression, Environment environment)
        {
            SyntaxNode root = SyntaxTreeBuilder.BuildTree(Lexer.Lex(expression));
            return null;
        }
    }
}
