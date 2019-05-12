using ExpressionParser.Parsing;
using ExpressionParser.SyntaxTree;

namespace Mathematics.ExpressionSimplification
{
    public class NestedAlgebraicSimplifier
    {
        public static SyntaxNode Simplify(OperatorNode node, Environment environment)
        {
            OperatorNode operatorNode = new OperatorNode(Operator.Multiplication);
            NumberNode leftCoefficient = (NumberNode)node.Left.Left;
            NumberNode rightCoefficient = (NumberNode)node.Right.Left;

            operatorNode.Left = new NumberNode(leftCoefficient.Value + rightCoefficient.Value);
            operatorNode.Right = node.Left.Right;
            return operatorNode;
        }
    }
}
