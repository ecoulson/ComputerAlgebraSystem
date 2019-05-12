using System;
using ExpressionParser.SyntaxTree;

namespace Mathematics.ExpressionSimplification
{
    public static class NumericalOperatorSimplifier
    {
        public static SyntaxNode Simplify(OperatorNode node)
        {
            NumberNode lhsNode = (NumberNode)node.Left;
            NumberNode rhsNode = (NumberNode)node.Right;
            switch (node.Operator)
            {
                case Operator.Addition:
                    return new NumberNode(lhsNode.Value + rhsNode.Value);
                case Operator.Subtraction:
                    return new NumberNode(lhsNode.Value - rhsNode.Value);
                case Operator.Multiplication:
                    return new NumberNode(lhsNode.Value * rhsNode.Value);
                case Operator.Division:
                    return new NumberNode(lhsNode.Value / rhsNode.Value);
                case Operator.Exponentiation:
                    return new NumberNode(Math.Pow(lhsNode.Value, rhsNode.Value));
                default:
                    return node;
            }
        }
    }
}
