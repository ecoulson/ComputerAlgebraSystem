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
                    return AnalyzeAmbiguousFunctionOrDistribution((FunctionOrDistributionNode)node);
                case SyntaxNodeType.Number:
                    return node;
                case SyntaxNodeType.Parentheses:
                    ParenthesesNode parenthesesNode = (ParenthesesNode)node;
                    parenthesesNode.Expression = Analyze(parenthesesNode.Expression);
                    return parenthesesNode;
                case SyntaxNodeType.Operator:
                    OperatorNode operatorNode = (OperatorNode)node;
                    List<SyntaxNode> newOperands = new List<SyntaxNode>();
                    foreach (SyntaxNode operand in operatorNode.Operands)
                    {
                        newOperands.Add(Analyze(operand));
                    }
                    operatorNode.Operands = newOperands;
                    return AnalyzeOperator(operatorNode);
                case SyntaxNodeType.Function:
                    FunctionNode functionNode = (FunctionNode)node;
                    functionNode.Name = (IdentifierNode)Analyze(functionNode.Name);
                    List<SyntaxNode> newArguments = new List<SyntaxNode>();
                    foreach (SyntaxNode argument in functionNode.Arguments)
                    {
                        newArguments.Add(Analyze(argument));
                    }
                    functionNode.Arguments = newArguments;
                    return functionNode;
                default:
                    throw new System.ArgumentException($"Unknown syntax node type '{node.Type}'");
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

        private static SyntaxNode AnalyzeAmbiguousFunctionOrDistribution(FunctionOrDistributionNode node)
        {
            if (environment.IsPredefinedFunction(node.Identifier.Value))
            {
                return CreateFunctionNode(node.Identifier, node.Expression);
            }

            EnvironmentVariable definition = environment.Get(node.Identifier.Value);
            if (definition.IsTypeOf(EnvironmentVariableType.Function))
            {
                return CreateFunctionNode(node.Identifier, node.Expression);
            }
            return CreateMultiplicationNode(node.Identifier, node.Expression);
        }

        private static SyntaxNode CreateFunctionNode(IdentifierNode name, SyntaxNode expression)
        {
            return new FunctionNode(
                (IdentifierNode)AnalyzeIdentifier(name),
                new List<SyntaxNode> { Analyze(expression) }
            );
        }

        private static SyntaxNode CreateMultiplicationNode(IdentifierNode lhs, SyntaxNode rhs)
        {
            return new OperatorNode(Operator.Multiplication, new List<SyntaxNode>
            {
                AnalyzeIdentifier(lhs), Analyze(rhs)
            });
        }

        private static SyntaxNode AnalyzeOperator(OperatorNode node)
        {
            if (IsNegativeConstantMultiplication(node))
            {
                NumberNode negative = null;
                NumberNode constant = null;
                foreach (SyntaxNode operand in node.Operands)
                {
                    if (operand.IsTypeOf(SyntaxNodeType.Number) && !IsNegativeOne(operand))
                    {
                        constant = (NumberNode)operand;
                    }
                    if (operand.IsTypeOf(SyntaxNodeType.Number) && IsNegativeOne(operand))
                    {
                        negative = (NumberNode)operand;
                    }
                }
                node.Operands.Remove(negative);
                node.Operands.Remove(constant);
                return new NumberNode(negative.Value * constant.Value);
            }
            return node;
        }

        private static bool IsNegativeConstantMultiplication(OperatorNode node)
        {
            if (node.Operator == Operator.Multiplication)
            {
                bool foundNegative = false;
                bool foundConstant = false;
                foreach (SyntaxNode operand in node.Operands)
                {
                    if (operand.IsTypeOf(SyntaxNodeType.Number) && !IsNegativeOne(operand))
                    {
                        foundConstant = true;
                    }
                    if (operand.IsTypeOf(SyntaxNodeType.Number) && IsNegativeOne(operand))
                    {
                        foundNegative = true;
                    }
                }
                return foundNegative && foundConstant;
            }
            return false;
        }

        private static bool IsNegativeOne(SyntaxNode node)
        {
            return ((NumberNode)node).Value == -1;
        }
    }
}
