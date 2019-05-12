using ExpressionParser.Parsing;
using ExpressionParser.SyntaxTree;

namespace Mathematics.ExpressionSimplification
{
    public class AlgebraicIdentitySimplifier
    {
        public static SyntaxNode Simplify(OperatorNode node, Environment environment)
        {
            IdentifierNode lhsNode = (IdentifierNode)node.Left;
            IdentifierNode rhsNode = (IdentifierNode)node.Right;

            if (lhsNode.Value == rhsNode.Value)
            {
                switch (node.Operator)
                {
                    case Operator.Addition:
                        OperatorNode multiplicationOperator = new OperatorNode(Operator.Multiplication);
                        multiplicationOperator.Left = new NumberNode(2);
                        multiplicationOperator.Right = rhsNode;
                        return multiplicationOperator;
                    case Operator.Subtraction:
                        return new NumberNode(0);
                    case Operator.Multiplication:
                        OperatorNode exponentOperator = new OperatorNode(Operator.Exponentiation);
                        exponentOperator.Left = lhsNode;
                        exponentOperator.Right = new NumberNode(2);
                        return exponentOperator;
                    case Operator.Division:
                        return new NumberNode(1);
                }
            }
            return node;
        }
    }
}
