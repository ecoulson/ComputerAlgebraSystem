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

        public OperatorNode(Operator op) : base(SyntaxNodeType.Operator)
        {
            Operator = op;
        }

        public override string ToString()
        {
            return $"{Left.ToString()} {OperatorToString()} {Right.ToString()}";
        }

        private string OperatorToString()
        {
            switch (Operator)
            {
                case Operator.Addition:
                    return "+";
                case Operator.Subtraction:
                    return "-";
                case Operator.Exponentiation:
                    return "^";
                case Operator.Multiplication:
                    return "*";
                case Operator.Division:
                    return "/";
                default:
                    throw new ArgumentException("Unknown operator exception");
            }
        }
    }
}
