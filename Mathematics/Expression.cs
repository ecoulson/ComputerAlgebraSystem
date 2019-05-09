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
            if (CanSimplifyOperator(node))
            {
                OperatorNode operatorNode = (OperatorNode)node;
                NumberNode lhsNode = (NumberNode)node.Left;
                NumberNode rhsNode = (NumberNode)node.Right;

                switch (operatorNode.Operator)
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
            }
            return node;
        }

        private bool CanSimplifyOperator(SyntaxNode node)
        {
            return node.Left.Type == SyntaxNodeType.Number &&
                node.Right.Type == SyntaxNodeType.Number;
        }

        public override string ToString()
        {
            return Tree.ToString();
        }
    }
}
