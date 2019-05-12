using ExpressionParser.Parsing;
using ExpressionParser.SyntaxTree;

namespace Mathematics.ExpressionSimplification
{
    public class RightNestedAlgebraicSimplifier
    {
        public static SyntaxNode Simplify(OperatorNode node, Environment environment)
        {
            OperatorNode operatorNode = new OperatorNode(Operator.Multiplication);
            NumberNode coefficientNode = (NumberNode)node.Right.Left;
            IdentifierNode leftHandIdentifier = (IdentifierNode)node.Left;

            operatorNode.Left = new NumberNode(coefficientNode.Value + 1);
            operatorNode.Right = leftHandIdentifier;
            return operatorNode;
        }
    }
}
