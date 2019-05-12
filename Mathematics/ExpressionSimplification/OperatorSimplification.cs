using ExpressionParser.Parsing;
using ExpressionParser.SyntaxTree;

namespace Mathematics.ExpressionSimplification
{
    public static class OperatorSimplification
    {
        public static SyntaxNode Simplify(OperatorNode node, Environment environment)
        {
            if (IsNumericalSimplification(node))
                return NumericalOperatorSimplifier.Simplify(node);
            if (IsAlgebraicIdentitySimplification(node))
                return AlgebraicIdentitySimplifier.Simplify(node, environment);
            if (IsAlgebraicLeftNestedSimplification(node))
                return LeftNestedAlgebraicSimplifier.Simplify(node, environment);
            if (IsAlgebraicRightNestedSimplification(node))
                return RightNestedAlgebraicSimplifier.Simplify(node, environment);
            if (IsAlgebraicNestedSimplification(node))
                return NestedAlgebraicSimplifier.Simplify(node, environment);
            return node;
        }

        private static bool IsNumericalSimplification(OperatorNode node)
        {
            return node.Left.IsTypeOf(SyntaxNodeType.Number) &&
                node.Right.IsTypeOf(SyntaxNodeType.Number);
        }

        private static bool IsAlgebraicIdentitySimplification(OperatorNode node)
        {
            return node.Left.IsTypeOf(SyntaxNodeType.Identifier) &&
                node.Right.IsTypeOf(SyntaxNodeType.Identifier);
        }

        private static bool IsAlgebraicLeftNestedSimplification(OperatorNode node)
        {
            return node.Left.IsTypeOf(SyntaxNodeType.Operator) &&
                node.Right.IsTypeOf(SyntaxNodeType.Identifier);
        }

        private static bool IsAlgebraicRightNestedSimplification(OperatorNode node)
        {
            return node.Left.IsTypeOf(SyntaxNodeType.Identifier) &&
                node.Right.IsTypeOf(SyntaxNodeType.Operator);
        }

        private static bool IsAlgebraicNestedSimplification(OperatorNode node)
        {
            return node.Left.IsTypeOf(SyntaxNodeType.Operator) &&
                node.Right.IsTypeOf(SyntaxNodeType.Operator);
        }
    }
}
