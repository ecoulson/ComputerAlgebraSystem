using System;
using ExpressionParser.Lex;

namespace ExpressionParser.SyntaxTree
{
    public class NumberNode : SyntaxNode
    {
        public double Value { get; }

        public NumberNode(Token token) : base(SyntaxNodeType.Number)
        {
            Value = double.Parse(token.Value);
        }

        public NumberNode(double? value) : base (SyntaxNodeType.Number)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }
            Value = (double)value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
