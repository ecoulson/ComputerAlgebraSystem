using System;
using System.Collections.Generic;
using ExpressionParser.Lex;

namespace ExpressionParser.SyntaxTree
{
    public class OperatorNode : SyntaxNode
    {
        public Operator Operator { get; }
        public List<SyntaxNode> Operands { get; set; }

        public OperatorNode(Token token) : base(SyntaxNodeType.Operator)
        {
            Operator = ConvertToken(token);
        }

        private Operator ConvertToken(Token token)
        {
            switch(token.Type)
            {
                case TokenType.Addition:
                    return Operator.Addition;
                case TokenType.Subtraction:
                    return Operator.Subtraction;
                case TokenType.Multiply:
                    return Operator.Multiplication;
                case TokenType.Divide:
                    return Operator.Division;
                case TokenType.Exponent:
                    return Operator.Exponentiation;
                default:
                    throw new Exception($"Illegal token of type '{token.Type}'");
            }
        }

        public OperatorNode(Operator op) : base(SyntaxNodeType.Operator)
        {
            Operator = op;
        }

        public OperatorNode(Token token, List<SyntaxNode> operands) : base(SyntaxNodeType.Operator)
        {
            Operator = ConvertToken(token);
            Operands = operands;
        }

        public OperatorNode(Operator op, List<SyntaxNode> operands) : base(SyntaxNodeType.Operator)
        {
            Operator = op;
            Operands = operands;
        }

        public override string ToString()
        {
            string toString = "";
            if (Operator == Operator.Multiplication || Operator == Operator.Division)
            {
                foreach (SyntaxNode operand in Operands)
                {
                    if (operand.IsTypeOf(SyntaxNodeType.Operator) && (((OperatorNode)operand).Operator == Operator.Addition || ((OperatorNode)operand).Operator == Operator.Subtraction))
                    {
                        toString += $"({operand.ToString()}) {OperatorToString()} ";
                    }
                    else
                    {
                        toString += $"{operand.ToString()} {OperatorToString()} ";
                    }
                }
            }
            else if (Operator == Operator.Exponentiation)
            {
                foreach(SyntaxNode operand in Operands)
                {
                    if (operand.IsTypeOf(SyntaxNodeType.Operator) && ((OperatorNode)operand).Operator != Operator.Exponentiation)
                    {
                        toString += $"({operand.ToString()}) {OperatorToString()} ";
                    }
                    else
                    {
                        toString += $"{operand.ToString()} {OperatorToString()} ";
                    }
                }
            }
            else
            {
                foreach (SyntaxNode operand in Operands)
                {
                    toString += $"{operand.ToString()} {OperatorToString()} ";
                }
            }
            return toString.Substring(0, toString.Length - 3);
        }

        public override SyntaxNode Copy()
        {
            return new OperatorNode(Operator, new List<SyntaxNode>(Operands));
        }

        public NumberNode Operate(List<NumberNode> constants)
        {
            if (constants.Count > 0)
            {
                NumberNode result = constants[0];
                for (int i = 1; i < constants.Count; i++)
                {
                    result = Operate(result, constants[i]); 
                }
                return result;
            }
            throw new Exception("Can not operate on an empty list");
        }

        private NumberNode Operate(NumberNode lhs, NumberNode rhs)
        {
            switch (Operator)
            {
                case Operator.Addition:
                    return new NumberNode(lhs.Value + rhs.Value);
                case Operator.Subtraction:
                    return new NumberNode(lhs.Value - rhs.Value);
                case Operator.Multiplication:
                    return new NumberNode(lhs.Value * rhs.Value);
                case Operator.Division:
                    return new NumberNode(lhs.Value / rhs.Value);
                case Operator.Exponentiation:
                    return new NumberNode(Math.Pow(lhs.Value, rhs.Value));
                default:
                    throw new Exception("Can not operate on unrecognized operator");
            }
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
