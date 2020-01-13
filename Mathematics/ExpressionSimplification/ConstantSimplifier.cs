using System.Collections.Generic;
using ExpressionParser.Parsing;
using ExpressionParser.SyntaxTree;

namespace Mathematics.ExpressionSimplification
{
    public class ConstantSimplifier
    {
        private Environment environment;

        public ConstantSimplifier(Environment environment)
        {
            this.environment = environment;
        }

        public SyntaxNode Simplify(SyntaxNode node)
        {
            if (node == null)
            {
                return node;
            }
            switch(node.Type)
            {
                case SyntaxNodeType.AmbigiousFunctionOrShortHandMultiplication:
                    return Simplify(((FunctionOrDistributionNode)node).Expression);
                case SyntaxNodeType.Function:
                    FunctionNode functionNode = (FunctionNode)node;
                    List<SyntaxNode> simplifiedArguments = new List<SyntaxNode>();
                    foreach (SyntaxNode argument in functionNode.Arguments)
                    {
                        simplifiedArguments.Add(Simplify(argument));
                    }
                    functionNode.Arguments = simplifiedArguments;
                    return functionNode;
                case SyntaxNodeType.Operator:
                    return SimplifyOperator((OperatorNode)node);
                case SyntaxNodeType.Parentheses:
                    return Simplify(((ParenthesesNode)node).Expression);
                default:
                    return node;
            }
        }

        private SyntaxNode SimplifyOperator(OperatorNode node)
        {
            List<SyntaxNode> simplifiedOperands = new List<SyntaxNode>();
            foreach(SyntaxNode operand in node.Operands)
            {
                simplifiedOperands.Add(Simplify(operand));
            }
            node.Operands = simplifiedOperands;
            SyntaxNode simplifiedResult = FoldConstants(node);
            if (simplifiedResult.IsTypeOf(SyntaxNodeType.Operator))
            {
                simplifiedResult = SimplifyMultiplicativeIdentity((OperatorNode)simplifiedResult);
            }
            if (simplifiedResult.IsTypeOf(SyntaxNodeType.Operator))
            {
                simplifiedResult = SimplifyExponentiationIdentity((OperatorNode)simplifiedResult);
            }
            if (simplifiedResult.IsTypeOf(SyntaxNodeType.Operator))
            {
                simplifiedResult = SimplifyAdditiveIdentity((OperatorNode)simplifiedResult);
            }
            return simplifiedResult;
        }

        private SyntaxNode FoldConstants(OperatorNode node)
        {
            List<NumberNode> constants = GetConstants(node);
            NumberNode foldedValue = node.Operate(constants);
            if (constants.Count == node.Operands.Count)
            {
                return foldedValue;
            }
            foreach (NumberNode constant in constants)
            {
                node.Operands.Remove(constant);
            }
            node.Operands.Add(foldedValue);
            return node;
        }

        private List<NumberNode> GetConstants(OperatorNode node)
        {
            List<NumberNode> constants = new List<NumberNode>();
            foreach (SyntaxNode operand in node.Operands)
            {
                if (operand.IsTypeOf(SyntaxNodeType.Number))
                {
                    constants.Add((NumberNode)operand);
                }
            }
            return constants;
        }

        private SyntaxNode SimplifyMultiplicativeIdentity(OperatorNode node)
        {
            if (node.Operator == Operator.Multiplication)
            {
                List<SyntaxNode> simplifiedOperands = new List<SyntaxNode>();
                foreach (SyntaxNode operand in node.Operands)
                {
                    if (!operand.IsTypeOf(SyntaxNodeType.Number) || (operand.IsTypeOf(SyntaxNodeType.Number) && ((NumberNode)operand).Value != 1))
                    {
                        simplifiedOperands.Add(operand);
                    }
                }
                node.Operands = simplifiedOperands;
            }
            if (node.Operands.Count == 0)
            {
                return new NumberNode(1);
            }
            if (node.Operands.Count == 1)
            {
                return node.Operands[0];
            }
            return node;
        }

        private SyntaxNode SimplifyExponentiationIdentity(OperatorNode node)
        {
            if (node.Operator == Operator.Exponentiation)
            {
                List<SyntaxNode> simplifiedOperands = new List<SyntaxNode>();
                foreach (SyntaxNode operand in node.Operands)
                {
                    if (!operand.IsTypeOf(SyntaxNodeType.Number) || (operand.IsTypeOf(SyntaxNodeType.Number) && ((NumberNode)operand).Value != 1))
                    {
                        simplifiedOperands.Add(operand);
                    }
                }
                node.Operands = simplifiedOperands;
            }
            if (node.Operands.Count == 0)
            {
                return new NumberNode(1);
            }
            if (node.Operands.Count == 1)
            {
                return node.Operands[0];
            }
            return node;
        }

        private SyntaxNode SimplifyAdditiveIdentity(OperatorNode node)
        {
            if (node.Operator == Operator.Addition || node.Operator == Operator.Subtraction)
            {
                List<SyntaxNode> simplifiedOperands = new List<SyntaxNode>();
                foreach (SyntaxNode operand in node.Operands)
                {
                    if (!operand.IsTypeOf(SyntaxNodeType.Number) || (operand.IsTypeOf(SyntaxNodeType.Number) && ((NumberNode)operand).Value != 0))
                    {
                        simplifiedOperands.Add(operand);
                    }
                }
                node.Operands = simplifiedOperands;
            }
            if (node.Operands.Count == 0)
            {
                return new NumberNode(0);
            }
            if (node.Operands.Count == 1)
            {
                return node.Operands[0];
            }
            return node;
        }
    }
}
