using ExpressionParser.Parsing;
using ExpressionParser.SyntaxTree;

namespace Mathematics.ExpressionSimplification
{
    public class LeftNestedAlgebraicSimplifier
    {
        public static SyntaxNode Simplify(OperatorNode node, Environment environment)
        {
            OperatorNode nestedOperator = (OperatorNode)node.Left;
            IdentifierNode rightHandIdentifier = (IdentifierNode)node.Right;

            if (HasNumberOperand(nestedOperator))
            {
                return SimplifyNestedTerm(node);
            }
            return SimplifyNestedIdentifiers(node);
        }

        private static bool HasNumberOperand(OperatorNode node)
        {
            return node.Right.IsTypeOf(SyntaxNodeType.Number) ||
                node.Left.IsTypeOf(SyntaxNodeType.Number);
        }

        private static SyntaxNode SimplifyNestedTerm(OperatorNode node)
        {
            OperatorNode nestedOperator = (OperatorNode)node.Left;
            IdentifierNode rightHandIdentifier = (IdentifierNode)node.Right;
            IdentifierNode nestedIdentifier = GetIdentifier(nestedOperator);

            if (rightHandIdentifier.Value == nestedIdentifier.Value)
            {
                OperatorNode operatorNode = new OperatorNode(Operator.Multiplication);
                NumberNode coefficientNode = GetCoefficient(nestedOperator);

                if (coefficientNode.Value + 1 == 0)
                    return new NumberNode(0);

                operatorNode.Left = new NumberNode(coefficientNode.Value + 1);
                operatorNode.Right = rightHandIdentifier;
                return operatorNode;
            }
            return node;
        }

        private static IdentifierNode GetIdentifier(OperatorNode node)
        {
            if (node.Right.IsTypeOf(SyntaxNodeType.Identifier))
                return (IdentifierNode)node.Right;
            return (IdentifierNode)node.Left;
        }

        private static NumberNode GetCoefficient(OperatorNode node)
        {
            if (node.Right.IsTypeOf(SyntaxNodeType.Number))
                return (NumberNode)node.Right;
            return (NumberNode)node.Left;
        }

        private static SyntaxNode SimplifyNestedIdentifiers(OperatorNode node)
        {
            OperatorNode nestedOperator = (OperatorNode)node.Left;
            IdentifierNode rightHandIdentifier = (IdentifierNode)node.Right;
            IdentifierNode rightNestedIdentifier = (IdentifierNode)nestedOperator.Right;
            IdentifierNode leftNestedIdentifier = (IdentifierNode)nestedOperator.Left;

            if (rightHandIdentifier.Value == rightNestedIdentifier.Value)
                node = AddLikeTerms(node, rightHandIdentifier, leftNestedIdentifier);
            if (rightHandIdentifier.Value == leftNestedIdentifier.Value)
                node = AddLikeTerms(node, rightHandIdentifier, rightNestedIdentifier);
            return node;
        }

        private static OperatorNode AddLikeTerms(OperatorNode node, IdentifierNode likeTerm, IdentifierNode otherTerm)
        {
            OperatorNode operatorNode = new OperatorNode(Operator.Multiplication);
            operatorNode.Left = new NumberNode(2);
            operatorNode.Right = likeTerm;
            node.Left = operatorNode;
            node.Right = otherTerm;
            return node;
        }
    }
}
