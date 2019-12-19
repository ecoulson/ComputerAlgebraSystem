using ExpressionParser.Semantics;
using ExpressionParser.SyntaxTree;

namespace Mathematics.Calculus
{
    public class Derivative : IDerivative
    {
        private Expression expression;
        private string variable;

        public Derivative(Expression expression, string variable)
        {
            this.expression = expression;
            this.variable = variable;
        }

        public Expression Derive()
        {
            return new Expression(DeriveHelper(expression.Tree), expression.GetEnvironment());
        }

        private SyntaxNode DeriveHelper(SyntaxNode node)
        {
            if (IsConstant(node))
            {
                return new NumberNode(0);
            }
            if (node.Type == SyntaxNodeType.Identifier)
            {
                return new NumberNode(1);
            }
            if (node.Type == SyntaxNodeType.Parentheses)
            {
                return DeriveHelper(node.Left);
            }
            if (node.Type == SyntaxNodeType.Function)
            {
                IdentifierNode nameNode = (IdentifierNode)node.Left;
                if (nameNode.Value.ToLower() == "ln")
                {
                    OperatorNode naturalLogRule = new OperatorNode(Operator.Multiplication);
                    naturalLogRule.Left = new OperatorNode(Operator.Division);
                    naturalLogRule.Left.Left = new NumberNode(1);
                    naturalLogRule.Left.Right = node.Right.Copy();
                    naturalLogRule.Right = DeriveHelper(node.Right);
                    return naturalLogRule;
                }
                if (nameNode.Value.ToLower() == "sin")
                {
                    OperatorNode sinRule = new OperatorNode(Operator.Multiplication);
                    sinRule.Right = DeriveHelper(node.Right);
                    sinRule.Left = new FunctionNode();
                    sinRule.Left.Left = new IdentifierNode("cos");
                    sinRule.Left.Right = node.Right.Copy();
                    return sinRule;
                }
                if (nameNode.Value.ToLower() == "cos")
                {
                    OperatorNode cosRule = new OperatorNode(Operator.Multiplication);
                    cosRule.Left = new NumberNode(-1);
                    cosRule.Right = new OperatorNode(Operator.Multiplication);
                    cosRule.Right.Right = DeriveHelper(node.Right);
                    cosRule.Right.Left = new FunctionNode();
                    cosRule.Right.Left.Left = new IdentifierNode("sin");
                    cosRule.Right.Left.Right = node.Right.Copy();
                    return cosRule;
                }
                if (nameNode.Value.ToLower() == "tan")
                {
                    OperatorNode tanRule = new OperatorNode(Operator.Multiplication);
                    tanRule.Right = DeriveHelper(node.Right);
                    tanRule.Left = new OperatorNode(Operator.Exponentiation);
                    tanRule.Left.Right = new NumberNode(2);
                    tanRule.Left.Left = new FunctionNode();
                    tanRule.Left.Left.Left = new IdentifierNode("sec");
                    tanRule.Left.Left.Right = node.Right.Copy();
                    return tanRule;
                }
                if (nameNode.Value.ToLower() == "sec")
                {
                    OperatorNode secRule = new OperatorNode(Operator.Multiplication);
                    secRule.Right = DeriveHelper(node.Right);
                    secRule.Left = new OperatorNode(Operator.Multiplication);
                    secRule.Left.Right = new FunctionNode();
                    secRule.Left.Right.Left = new IdentifierNode("tan");
                    secRule.Left.Right.Right = node.Right.Copy();
                    secRule.Left.Left = new FunctionNode();
                    secRule.Left.Left.Left = new IdentifierNode("sec");
                    secRule.Left.Left.Right = node.Right.Copy();
                    return secRule;
                }
                if (nameNode.Value.ToLower() == "csc")
                {
                    OperatorNode secRule = new OperatorNode(Operator.Multiplication);
                    secRule.Right = DeriveHelper(node.Right);
                    secRule.Left = new OperatorNode(Operator.Multiplication);
                    secRule.Left.Right = new FunctionNode();
                    secRule.Left.Right.Left = new IdentifierNode("tan");
                    secRule.Left.Right.Right = node.Right.Copy();
                    secRule.Left.Left = new FunctionNode();
                    secRule.Left.Left.Left = new IdentifierNode("sec");
                    secRule.Left.Left.Right = node.Right.Copy();
                    return secRule;
                }
            }
            if (node.Type == SyntaxNodeType.Operator)
            {
                if (IsConstant(node.Left) && IsConstant(node.Right))
                {
                    return new NumberNode(0);
                }
                OperatorNode operatorNode = (OperatorNode)node;
                if (operatorNode.Operator == Operator.Addition)
                {
                    return DeriveAdditionOperator(node);
                }
                if (operatorNode.Operator == Operator.Subtraction)
                {
                    return DeriveSubtractionOperator(node);
                }
                if (operatorNode.Operator == Operator.Multiplication)
                {
                    return DeriveMultiplicationOperator(node);
                }
                if (operatorNode.Operator == Operator.Division)
                {
                    return DeriveDivisionOperator(node);
                }
                if (operatorNode.Operator == Operator.Exponentiation)
                {
                    return DeriveExponentiationOperator(node);
                }
            }
            return node;
        }

        private bool IsConstant(SyntaxNode node)
        {
            if (node.Type == SyntaxNodeType.Number || IsConstantIdentifier(node))
            {
                return true;
            }
            if (node.Type == SyntaxNodeType.Identifier || node.Type == SyntaxNodeType.Function)
            {
                return false;
            }
            if (node.Type == SyntaxNodeType.Parentheses)
            {
                return IsConstant(node.Left);
            }
            if (node.Type == SyntaxNodeType.Operator)
            {
                return IsConstant(node.Left) && IsConstant(node.Right);
            }
            throw new IllegalNodeTypeException($"Unexpected node type of {node.Type}");
        }

        private bool IsConstantIdentifier(SyntaxNode node)
        {
            return node.Type == SyntaxNodeType.Identifier &&
                expression.GetEnvironment().HasConstant(((IdentifierNode)node).Value);
        }

        private SyntaxNode DeriveAdditionOperator(SyntaxNode node)
        {
            OperatorNode additionNode = new OperatorNode(Operator.Addition);
            additionNode.Left = DeriveHelper(node.Left);
            additionNode.Right = DeriveHelper(node.Right);
            return additionNode;
        }

        private SyntaxNode DeriveSubtractionOperator(SyntaxNode node)
        {
            OperatorNode subtractionNode = new OperatorNode(Operator.Subtraction);
            subtractionNode.Left = DeriveHelper(node.Left);
            subtractionNode.Right = DeriveHelper(node.Right);
            return subtractionNode;
        }

        private SyntaxNode DeriveMultiplicationOperator(SyntaxNode node)
        {
            if (IsConstant(node.Left))
            {
                return DeriveConstantMultiplication(node.Left, node.Right);
            }
            if (IsConstant(node.Right))
            {
                return DeriveConstantMultiplication(node.Right, node.Left);
            }
            return DeriveProductRule(node);
        }

        private SyntaxNode DeriveConstantMultiplication(SyntaxNode constant, SyntaxNode toDerive)
        {
            OperatorNode newRoot = new OperatorNode(Operator.Multiplication);
            newRoot.Right = DeriveHelper(toDerive);
            newRoot.Left = constant.Copy();
            return newRoot;
        }

        private SyntaxNode DeriveProductRule(SyntaxNode node)
        {
            OperatorNode productRuleRoot = new OperatorNode(Operator.Addition);
            productRuleRoot.Left = new OperatorNode(Operator.Multiplication);
            productRuleRoot.Right = new OperatorNode(Operator.Multiplication);
            productRuleRoot.Left.Left = node.Left.Copy();
            productRuleRoot.Right.Right = node.Right.Copy();
            productRuleRoot.Left.Right = DeriveHelper(node.Right);
            productRuleRoot.Right.Left = DeriveHelper(node.Left);
            return productRuleRoot;
        }

        private SyntaxNode DeriveDivisionOperator(SyntaxNode node)
        {
            if (IsConstant(node.Left))
            {
                return DeriveDivisionWithVariableDenominator(node);
            }
            if (IsConstant(node.Right))
            {
                return DeriveDivisionWithConstantDenominator(node);
            }
            return DeriveQuotientRule(node);
        }

        private SyntaxNode DeriveDivisionWithVariableDenominator(SyntaxNode node)
        {
            OperatorNode root = new OperatorNode(Operator.Multiplication);
            root.Left = node.Left.Copy();
            root.Right = new OperatorNode(Operator.Exponentiation);
            root.Right.Right = new NumberNode(-1);
            root.Right.Left = node.Right.Copy();
            return DeriveHelper(root);
        }

        private SyntaxNode DeriveDivisionWithConstantDenominator(SyntaxNode node)
        {
            SyntaxNode divisionNode = new OperatorNode(Operator.Division);
            divisionNode.Left = DeriveHelper(node.Left);
            divisionNode.Right = node.Right.Copy();
            return divisionNode;
        }

        private SyntaxNode DeriveQuotientRule(SyntaxNode node)
        {
            OperatorNode quotientRuleRoot = new OperatorNode(Operator.Division);
            quotientRuleRoot.Right = new OperatorNode(Operator.Exponentiation);
            quotientRuleRoot.Right.Right = new NumberNode(2);
            quotientRuleRoot.Right.Left = node.Right.Copy();
            quotientRuleRoot.Left = new OperatorNode(Operator.Subtraction);
            quotientRuleRoot.Left.Left = new OperatorNode(Operator.Multiplication);
            quotientRuleRoot.Left.Left.Left = node.Right.Copy();
            quotientRuleRoot.Left.Left.Right = DeriveHelper(node.Left);
            quotientRuleRoot.Left.Right = new OperatorNode(Operator.Multiplication);
            quotientRuleRoot.Left.Right.Left = node.Left.Copy();
            quotientRuleRoot.Left.Right.Right = DeriveHelper(node.Right);
            return quotientRuleRoot;
        }

        private SyntaxNode DeriveExponentiationOperator(SyntaxNode node)
        {
            if (IsConstant(node.Right))
            {
                return DerivePowerRule(node);
            }
            if (IsConstant(node.Left))
            {
                return DeriveExponential(node);
            }
            return DerivePowerTower(node);
        }

        private SyntaxNode DerivePowerRule(SyntaxNode node)
        {
            OperatorNode powerRuleRoot = new OperatorNode(Operator.Multiplication);
            powerRuleRoot.Left = node.Right.Copy();
            powerRuleRoot.Right = new OperatorNode(Operator.Multiplication);
            powerRuleRoot.Right.Right = DeriveHelper(node.Left);
            powerRuleRoot.Right.Left = new OperatorNode(Operator.Exponentiation);
            powerRuleRoot.Right.Left.Left = node.Left.Copy();
            powerRuleRoot.Right.Left.Right = new OperatorNode(Operator.Subtraction);
            powerRuleRoot.Right.Left.Right.Left = node.Right.Copy();
            powerRuleRoot.Right.Left.Right.Right = new NumberNode(1);
            return powerRuleRoot;
        }

        private SyntaxNode DeriveExponential(SyntaxNode node)
        {
            OperatorNode exponentiationRuleRoot = new OperatorNode(Operator.Multiplication);
            exponentiationRuleRoot.Right = DeriveHelper(node.Right);
            exponentiationRuleRoot.Left = new OperatorNode(Operator.Multiplication);
            exponentiationRuleRoot.Left.Left = new FunctionNode(new IdentifierNode("ln"), node.Left.Copy());
            exponentiationRuleRoot.Left.Right = node.Copy();
            return exponentiationRuleRoot;
        }

        private SyntaxNode DerivePowerTower(SyntaxNode node)
        {
            OperatorNode powerTowerRuleRoot = new OperatorNode(Operator.Multiplication);
            powerTowerRuleRoot.Left = node.Copy();
            OperatorNode naturalLogTransformation = new OperatorNode(Operator.Multiplication);
            naturalLogTransformation.Left = node.Left.Copy();
            naturalLogTransformation.Right = new FunctionNode(new IdentifierNode("ln"), node.Right.Copy());
            powerTowerRuleRoot.Right = DeriveHelper(naturalLogTransformation);
            return powerTowerRuleRoot;
        }
    }
}
