using System;

namespace ExpressionParser.Lex
{
    public enum TokenType
    {
        Number,
        Divide,
        Multiply,
        Addition,
        Subtraction,
        Exponent,
        Identifier,
        WhiteSpace,
        LeftParentheses,
        RightParentheses
    }
}
