using ExpressionParser.Parsing;
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
            ConstantSimplifier constantSimplifier = new ConstantSimplifier(environment);
            if (node == null)
                return node;
            return constantSimplifier.Simplify(node);
        }

        public Environment GetEnvironment()
        {
            return environment;
        }

        public override string ToString()
        {
            return Tree.ToString();
        }
    }
}
