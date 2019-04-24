using System;
namespace ExpressionParser.SyntaxTree
{
    public class FunctionNode : SyntaxNode
    {
        public FunctionNode(IdentifierNode left, SyntaxNode right) : base(SyntaxNodeType.Function)
        {
            Left = left;
            Right = right;
        }
    }
}
