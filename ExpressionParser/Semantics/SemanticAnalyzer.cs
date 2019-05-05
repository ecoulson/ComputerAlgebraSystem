using ExpressionParser.Parser;
using ExpressionParser.SyntaxTree;

namespace ExpressionParser.Semantics
{
    public static class SemanticAnalyzer
    {
        private static Environment environment;

        public static SyntaxNode Analyze(SyntaxNode root, Environment environment)
        {
            SemanticAnalyzer.environment = environment;
            return Analyze(root);
        }

        private static SyntaxNode Analyze(SyntaxNode node)
        {
            if (node.Type == SyntaxNodeType.Identifier)
            {
                return AnalyzeIdentifier((IdentifierNode)node);
            }
            if (node.Type == SyntaxNodeType.AmbigiousFunctionOrShortHandMultiplication)
            {
                return AnalyzeAmbiguousFunctionOrDistribution(node);
            }
            return node;
        }

        private static SyntaxNode AnalyzeIdentifier(IdentifierNode node)
        {
            if (environment.HasVariable(node.Value))
            {
                EnvironmentVariable variable = environment.Get(node.Value);

                if (variable.Type == EnvironmentVariableType.Number)
                {
                    return new NumberNode(variable.Value);
                }
                return node;
            }
            else
            {
                throw new UndefinedSymbolException(node);
            }
        }

        private static SyntaxNode AnalyzeAmbiguousFunctionOrDistribution(SyntaxNode node)
        {
            IdentifierNode symbol = (IdentifierNode)node.Left;
            EnvironmentVariable definition = environment.Get(symbol.Value);
            AssertNotNull(node.Right);

            if (definition.Type == EnvironmentVariableType.Function)
            {
                FunctionNode function = new FunctionNode();
                function.Left = AnalyzeIdentifier(symbol);
                function.Right = Analyze(node.Right);
                return function;
            }
            else
            {
                OperatorNode multiplication = new OperatorNode(Operator.Multiplication);
                multiplication.Left = AnalyzeIdentifier(symbol);
                multiplication.Right = Analyze(node.Right);
                return multiplication;
            }
        }

        private static void AssertNotNull(SyntaxNode node)
        {
            if (node == null) {
                throw new System.ArgumentNullException();
            }
        }
    }
}
