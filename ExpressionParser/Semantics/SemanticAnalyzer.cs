using System.Collections.Generic;
using ExpressionParser.Parsing;
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
            switch (node.Type)
            {
                case SyntaxNodeType.Identifier:
                    return AnalyzeIdentifier((IdentifierNode)node);
                case SyntaxNodeType.AmbigiousFunctionOrShortHandMultiplication:
                    return AnalyzeAmbiguousFunctionOrDistribution(node);
                case SyntaxNodeType.Number:
                    return node;
                case SyntaxNodeType.Parentheses:
                    node.Left = Analyze(node.Left);
                    return node;
                case SyntaxNodeType.Operator:
                    node.Left = Analyze(node.Left);
                    node.Right = Analyze(node.Right);
                    return node;
                case SyntaxNodeType.Function:
                    node.Left = Analyze(node.Left);
                    node.Right = Analyze(node.Right);
                    return node;
                default:
                    throw new System.ArgumentException($"Unkown syntax node type '{node.Type}'");
            }
        }

        private static SyntaxNode AnalyzeIdentifier(IdentifierNode node)
        {
            if (environment.IsKeyword(node.Value) || environment.HasVariable(node.Value))
            {
                return node;
            }
            return AnalyzeAmbiguousIdentifier(node);
        }

        private static SyntaxNode AnalyzeAmbiguousIdentifier(IdentifierNode node)
        {
            List<IdentifierResolution> multiplications =
                    IdentifierResolver.Resolve(environment.Symbols(), node.Value);
            if (multiplications.Count == 0)
            {
                throw new UndefinedSymbolException(node);
            }
            if (multiplications.Count > 1)
            {
                throw new AmbiguousIdentifierException(node.Value, multiplications);
            }
            return multiplications[0].ToSyntaxTree();
        }

        private static SyntaxNode AnalyzeAmbiguousFunctionOrDistribution(SyntaxNode node)
        {
            IdentifierNode symbol = (IdentifierNode)node.Left;
            if (environment.IsPredefinedFunction(symbol.Value))
            {
                return CreateFunctionNode(symbol, node.Right);
            }

            EnvironmentVariable definition = environment.Get(symbol.Value);
            if (definition.IsTypeOf(EnvironmentVariableType.Function))
            {
                return CreateFunctionNode(symbol, node.Right);
            }
            return CreateMultiplicationNode(symbol, node.Right);
        }

        private static SyntaxNode CreateFunctionNode(IdentifierNode name, SyntaxNode expression)
        {
            return new FunctionNode
            {
                Left = AnalyzeIdentifier(name),
                Right = Analyze(expression)
            };
        }

        private static SyntaxNode CreateMultiplicationNode(IdentifierNode lhs, SyntaxNode rhs)
        {
            return new OperatorNode(Operator.Multiplication)
            {
                Left = AnalyzeIdentifier(lhs),
                Right = Analyze(rhs)
            };
        }
    }
}
