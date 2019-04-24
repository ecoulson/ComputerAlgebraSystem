using System;
namespace ExpressionParser.SyntaxTree
{
    public class NumberNode : SyntaxNode
    {
        public double Value { get; }

        public NumberNode(string value) : base(SyntaxNodeType.Number)
        {
            Value = double.Parse(value);
        }
    }
}
