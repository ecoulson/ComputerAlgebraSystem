using System;
using ExpressionParser.SyntaxTree;

namespace Mathematics
{
    public class Expression
    {
        public SyntaxNode Tree { get; }

        public Expression(SyntaxNode tree)
        {
            Tree = tree;
        }

        public Expression Simplify()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return Tree.ToString();
        }
    }
}
