using System;
using ExpressionParser.Lex;

namespace ExpressionParser.SyntaxTree
{
    public class OperatorNode : SyntaxNode
    {
        public Operator Operator { get; }

        public OperatorNode(Token token) : base(SyntaxNodeType.Operator)
        {
            switch (token.Type)
            {
                case TokenType.Addition:
                    Operator = Operator.Addition;
                    break;
                case TokenType.Subtraction:
                    Operator = Operator.Subtraction;
                    break;
                case TokenType.Multiply:
                    Operator = Operator.Multiplication;
                    break;
                case TokenType.Divide:
                    Operator = Operator.Division;
                    break;
                case TokenType.Exponent:
                    Operator = Operator.Exponentiation;
                    break;
                default:
                    throw new Exception($"Illegal token of type '{token.Type}'");
            }
        }
    }
}
