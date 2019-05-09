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
            if (node.Type == SyntaxNodeType.Operator)
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
            if (CanSimplifyBasicAlgebraOperator(node))
            {
                return SimplifyBasicAlgebraicOperation(node);
            }
            if (node.Left.Type == SyntaxNodeType.Operator && node.Right.Type == SyntaxNodeType.Identifier)
            {
                OperatorNode operatorNode = new OperatorNode(Operator.Multiplication);
                NumberNode coefficientNode = (NumberNode)node.Left.Left;
                IdentifierNode rightHandIdentifier = (IdentifierNode)node.Right;

                operatorNode.Left = new NumberNode(coefficientNode.Value + 1);
                operatorNode.Right = rightHandIdentifier;
                return operatorNode;
            }
            return node;
        }


        private bool CanNumericallySimplifyOperator(OperatorNode node)
        {
            return node.Left.Type == SyntaxNodeType.Number &&
                node.Right.Type == SyntaxNodeType.Number;
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

        private bool CanSimplifyBasicAlgebraOperator(OperatorNode node)
        {
            return node.Left.Type == SyntaxNodeType.Identifier &&
                node.Right.Type == SyntaxNodeType.Identifier;
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

        public override string ToString()
        {
            return Tree.ToString();
        }
    }
}
