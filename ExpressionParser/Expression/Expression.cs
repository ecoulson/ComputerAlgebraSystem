using System;
using ExpressionParser.SyntaxTree;

namespace ExpressionParser
{
    public class Expression
    {
        public SyntaxNode Tree { get; }

        public Expression(SyntaxNode tree)
        {
            Tree = tree;
        }

        public double Evaluate(double x)
        {
            throw new NotImplementedException();
        }
    }
}
