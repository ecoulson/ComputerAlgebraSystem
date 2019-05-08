using System;
namespace ExpressionParser.SyntaxTree
{
    public enum SyntaxNodeType
    {
        Identifier,
        Function,
        AmbigiousFunctionOrShortHandMultiplication,
        Number,
        Operator,
        Parentheses
    }
}
