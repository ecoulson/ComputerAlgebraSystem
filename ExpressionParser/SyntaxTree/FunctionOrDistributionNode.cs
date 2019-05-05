using System;
namespace ExpressionParser.SyntaxTree
{
    public class FunctionOrDistributionNode : SyntaxNode
    {
        public FunctionOrDistributionNode() : base(SyntaxNodeType.AmbigiousFunctionOrShortHandMultiplication)
        {

        }

        public FunctionOrDistributionNode(IdentifierNode left, SyntaxNode right) : base(SyntaxNodeType.AmbigiousFunctionOrShortHandMultiplication)
        {
            Left = left;
            Right = right;
        }
    }
}
