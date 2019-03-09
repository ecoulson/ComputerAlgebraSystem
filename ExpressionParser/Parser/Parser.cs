using System;
using ExpressionParser.Lex;

namespace ExpressionParser.Parser
{
    public static readonly class Parser
    {
        public static Expression ParseExpression(string expression)
        {
            Lexer.lex();
        }
    }
}
