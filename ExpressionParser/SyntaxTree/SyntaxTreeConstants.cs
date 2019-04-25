using System;
using System.Collections.Generic;
using ExpressionParser.Lex;

namespace ExpressionParser.SyntaxTree
{
    public class SyntaxTreeConstants
    {
        public static readonly List<TokenType> FormalTokenTypes = new List<TokenType>
        {
            TokenType.LeftParentheses,
            TokenType.Identifier,
            TokenType.Number
        };

        public static readonly List<TokenType> FactorOperatorTypes = new List<TokenType>
        {
            TokenType.Multiply,
            TokenType.Divide
        };
    }
}
