using System;
namespace ExpressionParser.SyntaxTree
{
    public abstract class SyntaxNode
    {
        public SyntaxNodeType Type { get; }
        public SyntaxNode Left { get; set; }
        public SyntaxNode Right { get; set; }

        public SyntaxNode(SyntaxNodeType type)
        {
            Type = type;
        }

        public bool IsTypeOf(SyntaxNodeType type)
        {
            return type == Type;
        }

        public abstract SyntaxNode Copy();
    }
}
