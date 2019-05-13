using System.Collections.Generic;
using ExpressionParser.Lex;
using ExpressionParser.Semantics;
using ExpressionParser.SyntaxTree;

namespace ExpressionParser.Parsing
{
    public static class Parser
    {
        public static SyntaxNode ParseExpression(string expression, Environment environment)
        {
            SyntaxNode root = SyntaxTreeBuilder.BuildTree(Lexer.Lex(new RawExpression(expression)));
            return SemanticAnalyzer.Analyze(root, environment);
        }
    }
}
