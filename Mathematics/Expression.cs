using ExpressionParser.Parsing;
using ExpressionParser.SyntaxTree;

namespace Mathematics
{
    public class Expression
    {
        public SyntaxNode Tree { get; }
        private Environment environment;

        public Expression(SyntaxNode tree, Environment environment)
        {
            Tree = tree;
            this.environment = environment;
        }

        public Expression Simplify()
        {
            return new Expression(Simplify(Tree), environment);
        }

        private SyntaxNode Simplify(SyntaxNode node)
        {
            if (node.IsTypeOf(SyntaxNodeType.Operator))
            {
                return SimplifyOperator((OperatorNode)node);
            }
            return node;
        }

        private SyntaxNode SimplifyOperator(OperatorNode node)
        {
            node.Left = Simplify(node.Left);
            node.Right = Simplify(node.Right);

            if (CanNumericallySimplifyOperator(node))
            {
                return SimplifyNumericalOperation(node);
            }
            if (CanAlgebraicallySimplifyBasicOperator(node))
            {
                return SimplifyBasicAlgebraicOperation(node);
            }
            if (CanAlgebraicallySimplifyLeftNestedOperator(node))
            {
                return AlgebraicallySimplifyLeftNestedOperator(node);
            }
            if (CanAlgebraicallySimplifyRightNestedOperator(node))
            {
                return AlgebraicallySimplifyRightNestedOperator(node);
            }
            if (CanAlgebraicallySimplifyNestedOperator(node))
            {
                return AlgebraicallySimplifyNestedOperator(node);
            }
            return node;
        }

        private bool CanNumericallySimplifyOperator(OperatorNode node)
        {
            return node.Left.IsTypeOf(SyntaxNodeType.Number) &&
                node.Right.IsTypeOf(SyntaxNodeType.Number);
        }

        private SyntaxNode SimplifyNumericalOperation(OperatorNode node)
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
                    return new NumberNode(System.Math.Pow(lhsNode.Value, rhsNode.Value));
            }
            return node;
        }

        private bool CanAlgebraicallySimplifyBasicOperator(OperatorNode node)
        {
            return node.Left.IsTypeOf(SyntaxNodeType.Identifier) &&
                node.Right.IsTypeOf(SyntaxNodeType.Identifier);
        }

        private SyntaxNode SimplifyBasicAlgebraicOperation(OperatorNode node)
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

        private bool CanAlgebraicallySimplifyLeftNestedOperator(OperatorNode node)
        {
            return node.Left.IsTypeOf(SyntaxNodeType.Operator) &&
                node.Right.IsTypeOf(SyntaxNodeType.Identifier);
        }

        private SyntaxNode AlgebraicallySimplifyLeftNestedOperator(OperatorNode node)
        {
            OperatorNode nestedOperator = (OperatorNode)node.Left;
            IdentifierNode rightHandIdentifier = (IdentifierNode)node.Right;

            if (HasNumberOperand(nestedOperator)) 
            {
                return SimplifyNestedNumberAndIdentifierOperator(node);
            }

            IdentifierNode rightNestedIdentifier = (IdentifierNode)nestedOperator.Right;
            IdentifierNode leftNestedIdentifier = (IdentifierNode)nestedOperator.Left;
            if (rightHandIdentifier.Value == rightNestedIdentifier.Value)
            {
                OperatorNode operatorNode = new OperatorNode(Operator.Multiplication);
                operatorNode.Left = new NumberNode(2);
                operatorNode.Right = rightHandIdentifier;
                node.Left = operatorNode;
                node.Right = leftNestedIdentifier;
            }
            if (rightHandIdentifier.Value == leftNestedIdentifier.Value)
            {
                OperatorNode operatorNode = new OperatorNode(Operator.Multiplication);
                operatorNode.Left = new NumberNode(2);
                operatorNode.Right = rightHandIdentifier;
                node.Left = operatorNode;
                node.Right = rightNestedIdentifier;
            }
            return node;
        }

        private bool HasNumberOperand(OperatorNode node)
        {
            return node.Right.IsTypeOf(SyntaxNodeType.Number) ||
                node.Left.IsTypeOf(SyntaxNodeType.Number);
        }

        private SyntaxNode SimplifyNestedNumberAndIdentifierOperator(OperatorNode node)
        {
            OperatorNode nestedOperator = (OperatorNode)node.Left;
            IdentifierNode rightHandIdentifier = (IdentifierNode)node.Right;
            IdentifierNode nestedIdentifier = GetIdentifier(nestedOperator);

            if (rightHandIdentifier.Value == nestedIdentifier.Value)
            {
                OperatorNode operatorNode = new OperatorNode(Operator.Multiplication);
                NumberNode coefficientNode = GetCoefficient(nestedOperator);

                operatorNode.Left = new NumberNode(coefficientNode.Value + 1);
                operatorNode.Right = rightHandIdentifier;
                return operatorNode;
            }
            return node;
        }

        private IdentifierNode GetIdentifier(OperatorNode node)
        {
            if (node.Right.IsTypeOf(SyntaxNodeType.Identifier))
                return (IdentifierNode)node.Right;
            if (node.Left.IsTypeOf(SyntaxNodeType.Identifier))
                return (IdentifierNode)node.Left;
            return null;
        }

        private NumberNode GetCoefficient(OperatorNode node)
        {
            if (node.Right.IsTypeOf(SyntaxNodeType.Number))
                return (NumberNode)node.Right;
            if (node.Left.IsTypeOf(SyntaxNodeType.Number))
                return (NumberNode)node.Left;
            return null;
        }

        public bool CanAlgebraicallySimplifyRightNestedOperator(OperatorNode node)
        {
            return node.Left.IsTypeOf(SyntaxNodeType.Identifier) &&
                node.Right.IsTypeOf(SyntaxNodeType.Operator);
        }

        public SyntaxNode AlgebraicallySimplifyRightNestedOperator(OperatorNode node)
        {
            OperatorNode operatorNode = new OperatorNode(Operator.Multiplication);
            NumberNode coefficientNode = (NumberNode)node.Right.Left;
            IdentifierNode leftHandIdentifier = (IdentifierNode)node.Left;

            operatorNode.Left = new NumberNode(coefficientNode.Value + 1);
            operatorNode.Right = leftHandIdentifier;
            return operatorNode;
        }

        public bool CanAlgebraicallySimplifyNestedOperator(OperatorNode node)
        {
            return node.Left.IsTypeOf(SyntaxNodeType.Operator) &&
                node.Right.IsTypeOf(SyntaxNodeType.Operator);
        }

        public SyntaxNode AlgebraicallySimplifyNestedOperator(OperatorNode node)
        {
            OperatorNode operatorNode = new OperatorNode(Operator.Multiplication);
            NumberNode leftCoefficient = (NumberNode)node.Left.Left;
            NumberNode rightCoefficient = (NumberNode)node.Right.Left;

            operatorNode.Left = new NumberNode(leftCoefficient.Value + rightCoefficient.Value);
            operatorNode.Right = new IdentifierNode("j");
            return operatorNode;
        }

        public override string ToString()
        {
            return Tree.ToString();
        }
    }
}
