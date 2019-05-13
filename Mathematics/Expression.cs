﻿using ExpressionParser.Parsing;
using ExpressionParser.SyntaxTree;
using Mathematics.ExpressionSimplification;

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
            if (node == null)
                return node;

            node.Left = Simplify(node.Left);
            node.Right = Simplify(node.Right);

            if (node.IsTypeOf(SyntaxNodeType.Operator))
                return OperatorSimplification.Simplify((OperatorNode)node, environment);
            return node;
        }

        public override string ToString()
        {
            return Tree.ToString();
        }
    }
}
