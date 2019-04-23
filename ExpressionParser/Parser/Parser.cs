using System.Collections.Generic;
using ExpressionParser.Lex;
using ExpressionParser.SyntaxTree;

namespace ExpressionParser.Parser
{
    public static class Parser
    {
        public static Expression ParseExpression(string expression)
        {
            SyntaxTreeBuilder treeBuilder = new SyntaxTreeBuilder();
            SyntaxNode root = treeBuilder.BuildTree(Lexer.Lex(expression));
            return null;
        }
    }
}
