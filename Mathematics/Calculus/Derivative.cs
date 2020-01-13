using System.Collections.Generic;
using ExpressionParser.Semantics;
using ExpressionParser.SyntaxTree;
using Mathematics.ExpressionSimplification;

namespace Mathematics.Calculus
{
    public class Derivative : IDerivative
    {
        private Expression expression;
        private string variable;
        private ConstantSimplifier simplifier;

        public Derivative(Expression expression, string variable)
        {
            this.expression = expression;
            this.variable = variable;
            simplifier = new ConstantSimplifier(expression.GetEnvironment());
        }

        public Expression Derive()
        {
            return new Expression(
                simplifier.Simplify(DeriveHelper(expression.Tree)),
                expression.GetEnvironment()
            );
        }

        private SyntaxNode DeriveHelper(SyntaxNode node)
        {
            if (IsConstant(node))
            {
                return new NumberNode(0);
            }
            if (node.IsTypeOf(SyntaxNodeType.Identifier))
            {
                return new NumberNode(1);
            }
            //if (node.IsTypeOf(SyntaxNodeType.Parentheses))
            //{
            //    return DeriveHelper(((ParenthesesNode)node).Expression);
            //}
            //if (node.IsTypeOf(SyntaxNodeType.Function))
            //{
                //FunctionNode functionNode = (FunctionNode)node;
                //if (functionNode.Name.Value.ToLower() == "ln")
                //{
                //    OperatorNode naturalLogRule = new OperatorNode(Operator.Multiplication);
                //    naturalLogRule.Left = new OperatorNode(Operator.Division);
                //    naturalLogRule.Left.Left = new NumberNode(1);
                //    naturalLogRule.Left.Right = node.Right.Copy();
                //    naturalLogRule.Right = DeriveHelper(node.Right);
                //    return naturalLogRule;
                //}
                //if (functionNode.Name.Value.ToLower() == "sin")
                //{
                //    OperatorNode sinRule = new OperatorNode(Operator.Multiplication);
                //    sinRule.Right = DeriveHelper(node.Right);
                //    sinRule.Left = new FunctionNode();
                //    sinRule.Left.Left = new IdentifierNode("cos");
                //    sinRule.Left.Right = node.Right.Copy();
                //    return sinRule;
                //}
                //if (functionNode.Name.Value.ToLower() == "cos")
                //{
                //    OperatorNode cosRule = new OperatorNode(Operator.Multiplication);
                //    cosRule.Left = new NumberNode(-1);
                //    cosRule.Right = new OperatorNode(Operator.Multiplication);
                //    cosRule.Right.Right = DeriveHelper(node.Right);
                //    cosRule.Right.Left = new FunctionNode();
                //    cosRule.Right.Left.Left = new IdentifierNode("sin");
                //    cosRule.Right.Left.Right = node.Right.Copy();
                //    return cosRule;
                //}
                //if (functionNode.Name.Value.ToLower() == "tan")
                //{
                //    OperatorNode tanRule = new OperatorNode(Operator.Multiplication);
                //    tanRule.Right = DeriveHelper(node.Right);
                //    tanRule.Left = new OperatorNode(Operator.Exponentiation);
                //    tanRule.Left.Right = new NumberNode(2);
                //    tanRule.Left.Left = new FunctionNode();
                //    tanRule.Left.Left.Left = new IdentifierNode("sec");
                //    tanRule.Left.Left.Right = node.Right.Copy();
                //    return tanRule;
                //}
                //if (functionNode.Name.Value.ToLower() == "sec")
                //{
                //    OperatorNode secRule = new OperatorNode(Operator.Multiplication);
                //    secRule.Right = DeriveHelper(node.Right);
                //    secRule.Left = new OperatorNode(Operator.Multiplication);
                //    secRule.Left.Right = new FunctionNode();
                //    secRule.Left.Right.Left = new IdentifierNode("tan");
                //    secRule.Left.Right.Right = node.Right.Copy();
                //    secRule.Left.Left = new FunctionNode();
                //    secRule.Left.Left.Left = new IdentifierNode("sec");
                //    secRule.Left.Left.Right = node.Right.Copy();
                //    return secRule;
                //}
                //if (functionNode.Name.Value.ToLower() == "csc")
                //{
                //    OperatorNode secRule = new OperatorNode(Operator.Multiplication);
                //    secRule.Right = DeriveHelper(node.Right);
                //    secRule.Left = new OperatorNode(Operator.Multiplication);
                //    secRule.Left.Right = new FunctionNode();
                //    secRule.Left.Right.Left = new IdentifierNode("tan");
                //    secRule.Left.Right.Right = node.Right.Copy();
                //    secRule.Left.Left = new FunctionNode();
                //    secRule.Left.Left.Left = new IdentifierNode("sec");
                //    secRule.Left.Left.Right = node.Right.Copy();
                //    return secRule;
                //}
                //if (functionNode.Name.Value.ToLower() == "cot")
                //{
                //    OperatorNode cotRule = new OperatorNode(Operator.Multiplication);
                //    cotRule.Right = DeriveHelper(node.Right);
                //    cotRule.Left = new OperatorNode(Operator.Exponentiation);
                //    cotRule.Left.Right = new NumberNode(2);
                //    cotRule.Left.Left = new FunctionNode();
                //    cotRule.Left.Left.Left = new IdentifierNode("csc");
                //    cotRule.Left.Left.Right = node.Right.Copy();
                //    return cotRule;
                //}
            //}
            //if (node.Type == SyntaxNodeType.Operator)
            //{
            //    OperatorNode operatorNode = (OperatorNode)node;
            //    if (operatorNode.Operator == Operator.Addition)
            //    {
            //        return DeriveAdditionOperator((OperatorNode)node);
            //    }
            //    if (operatorNode.Operator == Operator.Subtraction)
            //    {
            //        return DeriveAdditionOperator((OperatorNode)node);
            //    }
            //    if (operatorNode.Operator == Operator.Multiplication)
            //    {
            //        return DeriveMultiplicationOperator(node);
            //    }
            //    if (operatorNode.Operator == Operator.Division)
            //    {
            //        return DeriveDivisionOperator(node);
            //    }
            //    if (operatorNode.Operator == Operator.Exponentiation)
            //    {
            //        return DeriveExponentiationOperator(node);
            //    }
            //}
            return node;
        }

        private bool IsConstant(SyntaxNode node)
        {
            if (node.IsTypeOf(SyntaxNodeType.Number) || IsConstantIdentifier(node))
            {
                return true;
            }
            if (node.IsTypeOf(SyntaxNodeType.Identifier) || node.IsTypeOf(SyntaxNodeType.Function))
            {
                return false;
            }
            if (node.IsTypeOf(SyntaxNodeType.Parentheses))
            {
                return IsConstant(((ParenthesesNode)node).Expression);
            }
            if (node.Type == SyntaxNodeType.Operator)
            {
                foreach (SyntaxNode operand in ((OperatorNode)node).Operands)
                {
                    if (!IsConstant(operand)) 
                    {
                        return false;
                    }
                }
                return true;
            }
            throw new IllegalNodeTypeException($"Unexpected node type of {node.Type}");
        }

        private bool IsConstantIdentifier(SyntaxNode node)
        {
            return node.Type == SyntaxNodeType.Identifier &&
                (IsConstantNode(node) || IsConstantVariable(node));
        }

        private bool IsConstantNode(SyntaxNode node)
        {
            return expression.GetEnvironment().HasConstant(((IdentifierNode)node).Value);
        }

        private bool IsConstantVariable(SyntaxNode node)
        {
            return expression.GetEnvironment().HasVariable(((IdentifierNode)node).Value) &&
                ((IdentifierNode)node).Value != variable;
        }

        //private SyntaxNode DeriveAdditionOperator(OperatorNode node)
        //{
        //    OperatorNode additionNode = new OperatorNode(Operator.Addition);
        //    List<SyntaxNode> derivedOperands = new List<SyntaxNode>();
        //    foreach (SyntaxNode operand in node.Operands)
        //    {
        //        derivedOperands.Add(DeriveHelper(operand));
        //    }
        //    additionNode.Operands = derivedOperands;
        //    return additionNode;
        //}

        //private SyntaxNode DeriveMultiplicationOperator(SyntaxNode node)
        //{
        //    if (IsConstant(node.Left))
        //    {
        //        return DeriveConstantMultiplication(node.Left, node.Right);
        //    }
        //    if (IsConstant(node.Right))
        //    {
        //        return DeriveConstantMultiplication(node.Right, node.Left);
        //    }
        //    return DeriveProductRule(node);
        //}

        //private SyntaxNode DeriveConstantMultiplication(SyntaxNode constant, SyntaxNode toDerive)
        //{
        //    OperatorNode newRoot = new OperatorNode(Operator.Multiplication);
        //    newRoot.Right = DeriveHelper(toDerive);
        //    newRoot.Left = constant.Copy();
        //    return newRoot;
        //}

        //private SyntaxNode DeriveProductRule(SyntaxNode node)
        //{
        //    OperatorNode productRuleRoot = new OperatorNode(Operator.Addition);
        //    productRuleRoot.Left = new OperatorNode(Operator.Multiplication);
        //    productRuleRoot.Right = new OperatorNode(Operator.Multiplication);
        //    productRuleRoot.Left.Left = node.Left.Copy();
        //    productRuleRoot.Right.Right = node.Right.Copy();
        //    productRuleRoot.Left.Right = DeriveHelper(node.Right);
        //    productRuleRoot.Right.Left = DeriveHelper(node.Left);
        //    return productRuleRoot;
        //}

        //private SyntaxNode DeriveDivisionOperator(SyntaxNode node)
        //{
        //    if (IsConstant(node.Left))
        //    {
        //        return DeriveDivisionWithVariableDenominator(node);
        //    }
        //    if (IsConstant(node.Right))
        //    {
        //        return DeriveDivisionWithConstantDenominator(node);
        //    }
        //    return DeriveQuotientRule(node);
        //}

        //private SyntaxNode DeriveDivisionWithVariableDenominator(SyntaxNode node)
        //{
        //    OperatorNode root = new OperatorNode(Operator.Multiplication);
        //    root.Left = node.Left.Copy();
        //    root.Right = new OperatorNode(Operator.Exponentiation);
        //    root.Right.Right = new NumberNode(-1);
        //    root.Right.Left = node.Right.Copy();
        //    return DeriveHelper(root);
        //}

        //private SyntaxNode DeriveDivisionWithConstantDenominator(SyntaxNode node)
        //{
        //    SyntaxNode divisionNode = new OperatorNode(Operator.Division);
        //    divisionNode.Left = DeriveHelper(node.Left);
        //    divisionNode.Right = node.Right.Copy();
        //    return divisionNode;
        //}

        //private SyntaxNode DeriveQuotientRule(SyntaxNode node)
        //{
        //    OperatorNode quotientRuleRoot = new OperatorNode(Operator.Division);
        //    quotientRuleRoot.Right = new OperatorNode(Operator.Exponentiation);
        //    quotientRuleRoot.Right.Right = new NumberNode(2);
        //    quotientRuleRoot.Right.Left = node.Right.Copy();
        //    quotientRuleRoot.Left = new OperatorNode(Operator.Subtraction);
        //    quotientRuleRoot.Left.Left = new OperatorNode(Operator.Multiplication);
        //    quotientRuleRoot.Left.Left.Left = node.Right.Copy();
        //    quotientRuleRoot.Left.Left.Right = DeriveHelper(node.Left);
        //    quotientRuleRoot.Left.Right = new OperatorNode(Operator.Multiplication);
        //    quotientRuleRoot.Left.Right.Left = node.Left.Copy();
        //    quotientRuleRoot.Left.Right.Right = DeriveHelper(node.Right);
        //    return quotientRuleRoot;
        //}

        //private SyntaxNode DeriveExponentiationOperator(SyntaxNode node)
        //{
        //    if (IsConstant(node.Right))
        //    {
        //        return DerivePowerRule(node);
        //    }
        //    if (IsConstant(node.Left))
        //    {
        //        return DeriveExponential(node);
        //    }
        //    return DerivePowerTower(node);
        //}

        //private SyntaxNode DerivePowerRule(SyntaxNode node)
        //{
        //    OperatorNode powerRuleRoot = new OperatorNode(Operator.Multiplication);
        //    powerRuleRoot.Left = node.Right.Copy();
        //    powerRuleRoot.Right = new OperatorNode(Operator.Multiplication);
        //    powerRuleRoot.Right.Right = DeriveHelper(node.Left);
        //    powerRuleRoot.Right.Left = new OperatorNode(Operator.Exponentiation);
        //    powerRuleRoot.Right.Left.Left = node.Left.Copy();
        //    powerRuleRoot.Right.Left.Right = new OperatorNode(Operator.Subtraction);
        //    powerRuleRoot.Right.Left.Right.Left = node.Right.Copy();
        //    powerRuleRoot.Right.Left.Right.Right = new NumberNode(1);
        //    return powerRuleRoot;
        //}

        //private SyntaxNode DeriveExponential(SyntaxNode node)
        //{
        //    OperatorNode exponentiationRuleRoot = new OperatorNode(Operator.Multiplication);
        //    exponentiationRuleRoot.Right = DeriveHelper(node.Right);
        //    exponentiationRuleRoot.Left = new OperatorNode(Operator.Multiplication);
        //    exponentiationRuleRoot.Left.Left = new FunctionNode(new IdentifierNode("ln"), node.Left.Copy());
        //    exponentiationRuleRoot.Left.Right = node.Copy();
        //    return exponentiationRuleRoot;
        //}

        //private SyntaxNode DerivePowerTower(SyntaxNode node)
        //{
        //    OperatorNode powerTowerRuleRoot = new OperatorNode(Operator.Multiplication);
        //    powerTowerRuleRoot.Left = node.Copy();
        //    OperatorNode naturalLogTransformation = new OperatorNode(Operator.Multiplication);
        //    naturalLogTransformation.Left = node.Left.Copy();
        //    naturalLogTransformation.Right = new FunctionNode(new IdentifierNode("ln"), node.Right.Copy());
        //    powerTowerRuleRoot.Right = DeriveHelper(naturalLogTransformation);
        //    return powerTowerRuleRoot;
        //}
    }
}
