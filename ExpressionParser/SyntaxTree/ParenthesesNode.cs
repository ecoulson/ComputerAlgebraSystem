using System;
namespace ExpressionParser.SyntaxTree
{
    public class ParenthesesNode : SyntaxNode
    {
        public SyntaxNode Expression { get; set; }

        public ParenthesesNode(SyntaxNode expression) : base(SyntaxNodeType.Parentheses)
        {
            Expression = expression;
        }

        public override string ToString()
        {
            return $"({Expression.ToString()})";
        }

        public override SyntaxNode Copy()
        {
            return new ParenthesesNode(Expression.Copy());
        }
    }
}
