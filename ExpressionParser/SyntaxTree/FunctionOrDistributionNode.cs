using System;
namespace ExpressionParser.SyntaxTree
{
    public class FunctionOrDistributionNode : SyntaxNode
    {
        public FunctionOrDistributionNode(IdentifierNode left, SyntaxNode right) : base(SyntaxNodeType.Function)
        {
            Left = left;
            Right = right;
        }
    }
}
