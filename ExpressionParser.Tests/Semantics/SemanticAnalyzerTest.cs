using ExpressionParser.Lex;
using ExpressionParser.Parsing;
using ExpressionParser.Semantics;
using ExpressionParser.SyntaxTree;
using NUnit.Framework;

namespace ExpressionParser.Tests.SemanticTest
{
    [TestFixture]
    public class SemanticAnalyzerTest
    {
        [Test]
        public void Analyze_NegativeNumber_ReturnsNumberNode()
        {
            SyntaxNode node = new OperatorNode(Operator.Multiplication);
            node.Left = new NumberNode(-1);
            node.Right = new NumberNode(2);

            node = SemanticAnalyzer.Analyze(node, new Environment());

            Assert.AreEqual("-2", node.ToString());
        }

        [Test]
        public void Analyze_Identifier_ThrowsUndefinedIdentifier()
        {
            SyntaxNode node = new IdentifierNode("x");
            Environment environment = new Environment();

            UndefinedSymbolException exception = Assert.Throws<UndefinedSymbolException>(() =>
            {
                SemanticAnalyzer.Analyze(node, environment);
            });

            Assert.NotNull(exception);
            Assert.AreEqual("Undefined symbol 'x'", exception.Message);
        }

        [Test]
        public void Analyze_NumberSymbol_ReturnsIdentifierNode()
        {
            SyntaxNode node = new IdentifierNode("x");
            Environment environment = new Environment();
            environment.AddValue("x", 2);

            IdentifierNode number = (IdentifierNode)SemanticAnalyzer.Analyze(node, environment);

            Assert.NotNull(number);
            Assert.AreEqual(SyntaxNodeType.Identifier, number.Type);
            Assert.AreEqual("x", number.Value);
        }

        [Test]
        public void Analyze_VariableSymbol_ReturnsIdentifierNode()
        {
            SyntaxNode node = new IdentifierNode("x");
            Environment environment = new Environment();
            environment.AddSymbol("x");

            IdentifierNode symbol = (IdentifierNode)SemanticAnalyzer.Analyze(node, environment);

            Assert.NotNull(symbol);
            Assert.AreEqual(SyntaxNodeType.Identifier, symbol.Type);
            Assert.AreEqual("x", symbol.Value);
        }

        [Test]
        public void Analyze_NumberValue_ReturnsNumberNode()
        {
            SyntaxNode node = new NumberNode(1);

            NumberNode number = (NumberNode)SemanticAnalyzer.Analyze(node, new Environment());

            Assert.NotNull(number);
            Assert.AreEqual(SyntaxNodeType.Number, number.Type);
            Assert.AreEqual(1, number.Value);
        }

        [Test]
        public void Analyze_AmbigiousFunction_ReturnsFunctionNode()
        {
            IdentifierNode left = new IdentifierNode("x");
            SyntaxNode right = new NumberNode(1);
            SyntaxNode root = new FunctionOrDistributionNode(left, right);

            Environment environment = new Environment();
            environment.AddFunction("x", null);

            FunctionNode function = (FunctionNode)SemanticAnalyzer.Analyze(root, environment);
            IdentifierNode functionName = (IdentifierNode)function.Left;
            NumberNode callValue = (NumberNode)function.Right;

            Assert.NotNull(function);
            Assert.NotNull(function.Left);
            Assert.NotNull(function.Right);
            Assert.AreEqual(SyntaxNodeType.Function, function.Type);
            Assert.AreEqual("x", functionName.Value);
            Assert.AreEqual(1, callValue.Value);
        }

        [Test]
        public void Analyze_AmbigiousSymbolDistribution_ReturnsOperatorNode()
        {
            IdentifierNode left = new IdentifierNode("x");
            SyntaxNode right = new NumberNode(1);
            SyntaxNode root = new FunctionOrDistributionNode(left, right);

            Environment environment = new Environment();
            environment.AddSymbol("x");

            OperatorNode multiplier = (OperatorNode)SemanticAnalyzer.Analyze(root, environment);
            IdentifierNode lhs = (IdentifierNode)multiplier.Left;
            NumberNode rhs = (NumberNode)multiplier.Right;

            Assert.NotNull(multiplier);
            Assert.NotNull(multiplier.Left);
            Assert.NotNull(multiplier.Right);
            Assert.AreEqual(SyntaxNodeType.Operator, multiplier.Type);
            Assert.AreEqual(Operator.Multiplication, multiplier.Operator);
            Assert.AreEqual("x", lhs.Value);
            Assert.AreEqual(1, rhs.Value);
        }

        [Test]
        public void Analyze_AmbigiousValueDistribution_ReturnsOperatorNode()
        {
            IdentifierNode left = new IdentifierNode("x");
            SyntaxNode right = new NumberNode(1);
            SyntaxNode root = new FunctionOrDistributionNode(left, right);

            Environment environment = new Environment();
            environment.AddValue("x", 2);

            OperatorNode multiplier = (OperatorNode)SemanticAnalyzer.Analyze(root, environment);
            IdentifierNode lhs = (IdentifierNode)multiplier.Left;
            NumberNode rhs = (NumberNode)multiplier.Right;

            Assert.NotNull(multiplier);
            Assert.NotNull(lhs);
            Assert.NotNull(rhs);
            Assert.AreEqual(SyntaxNodeType.Operator, multiplier.Type);
            Assert.AreEqual(Operator.Multiplication, multiplier.Operator);
            Assert.AreEqual("x", lhs.Value);
            Assert.AreEqual(1, rhs.Value);
        }

        [Test]
        public void Analyze_AmbiguousPredefinedFunction_ReturnsFunctionNode()
        {
            IdentifierNode left = new IdentifierNode("sin");
            SyntaxNode right = new NumberNode(1);
            SyntaxNode root = new FunctionOrDistributionNode(left, right);

            FunctionNode function = (FunctionNode)SemanticAnalyzer.Analyze(root, new Environment());
            IdentifierNode functionName = (IdentifierNode)function.Left;
            NumberNode callValue = (NumberNode)function.Right;

            Assert.NotNull(function);
            Assert.NotNull(function.Left);
            Assert.NotNull(function.Right);
            Assert.AreEqual(SyntaxNodeType.Function, function.Type);
            Assert.AreEqual("sin", functionName.Value);
            Assert.AreEqual(1, callValue.Value);
        }

        [Test]
        public void Analyze_RecognizeShorthandMultiplicationInsideIdentifer_ReturnsOperatorNode()
        {
            IdentifierNode symbol = new IdentifierNode("xy");
            Environment environment = new Environment();
            environment.AddSymbol("x");
            environment.AddSymbol("y");

            OperatorNode operatorNode = (OperatorNode)SemanticAnalyzer.Analyze(symbol, environment);
            IdentifierNode x = (IdentifierNode)operatorNode.Left;
            IdentifierNode y = (IdentifierNode)operatorNode.Right;

            Assert.NotNull(operatorNode);
            Assert.NotNull(x);
            Assert.NotNull(y);
            Assert.AreEqual(Operator.Multiplication, operatorNode.Operator);
            Assert.AreEqual(SyntaxNodeType.Identifier, x.Type);
            Assert.AreEqual(SyntaxNodeType.Identifier, y.Type);
            Assert.AreEqual("x", x.Value);
            Assert.AreEqual("y", y.Value);
        }

        [Test]
        public void Analyze_RecognizeShorthandMultiplicationInsideIdentiferWithOneVariable_ReturnsOperatorNode()
        {
            IdentifierNode symbol = new IdentifierNode("xx");
            Environment environment = new Environment();
            environment.AddSymbol("x");

            OperatorNode operatorNode = (OperatorNode)SemanticAnalyzer.Analyze(symbol, environment);
            IdentifierNode left = (IdentifierNode)operatorNode.Left;
            IdentifierNode right = (IdentifierNode)operatorNode.Right;

            Assert.NotNull(operatorNode);
            Assert.NotNull(left);
            Assert.NotNull(right);
            Assert.AreEqual(Operator.Multiplication, operatorNode.Operator);
            Assert.AreEqual(SyntaxNodeType.Identifier, left.Type);
            Assert.AreEqual(SyntaxNodeType.Identifier, right.Type);
            Assert.AreEqual("x", left.Value);
            Assert.AreEqual("x", right.Value);
        }

        [Test]
        public void Analyze_RecognizeShorthandMultiplicationInsideIdentifierWithPredefinedSymbols_ReturnsIdentifierNode()
        {
            IdentifierNode symbol = new IdentifierNode("xe");
            Environment environment = new Environment();
            environment.AddSymbol("x");

            OperatorNode operatorNode = (OperatorNode)SemanticAnalyzer.Analyze(symbol, environment);
            IdentifierNode left = (IdentifierNode)operatorNode.Left;
            IdentifierNode right = (IdentifierNode)operatorNode.Right;

            Assert.NotNull(operatorNode);
            Assert.NotNull(left);
            Assert.NotNull(right);
            Assert.AreEqual(Operator.Multiplication, operatorNode.Operator);
            Assert.AreEqual(SyntaxNodeType.Identifier, left.Type);
            Assert.AreEqual(SyntaxNodeType.Identifier, right.Type);
            Assert.AreEqual("x", left.Value);
            Assert.AreEqual("e", right.Value);
        }

        [Test]
        public void Analyze_Keyword_ReturnsIdentifierNode()
        {
            IdentifierNode symbol = new IdentifierNode("e");

            IdentifierNode node = (IdentifierNode)SemanticAnalyzer.Analyze(symbol, new Environment());

            Assert.NotNull(node);
            Assert.AreEqual(SyntaxNodeType.Identifier, node.Type);
            Assert.AreEqual("e", node.Value);
        }

        [Test]
        public void Analyze_AmbiguousVariable_ThrowsException()
        {
            IdentifierNode symbol = new IdentifierNode("xyz");
            Environment environment = new Environment();
            environment.AddSymbol("x");
            environment.AddSymbol("y");
            environment.AddSymbol("z");
            environment.AddSymbol("xy");
            environment.AddSymbol("yz");

            AmbiguousIdentifierException exception = Assert.Throws<AmbiguousIdentifierException>(() =>
            {
                SemanticAnalyzer.Analyze(symbol, environment);
            });

            Assert.NotNull(exception);
            Assert.AreEqual("Ambiguous identifier 'xyz' can be made from 'x, y, z', 'x, yz', 'xy, z'", exception.Message);
        }

        [Test]
        public void Analyze_Parentheses_ThrowsException()
        {
            ParenthesesNode node = new ParenthesesNode(new IdentifierNode("x"));

            Assert.Throws<UndefinedSymbolException>(() => SemanticAnalyzer.Analyze(node, new Environment()));
        }

        [Test]
        public void Analyze_Parentheses_ReturnsParenthesesNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            ParenthesesNode node = new ParenthesesNode(new IdentifierNode("x"));

            node = (ParenthesesNode)SemanticAnalyzer.Analyze(node, environment);
            IdentifierNode xNode = (IdentifierNode)node.Left;

            Assert.NotNull(node);
            Assert.NotNull(xNode);
            Assert.AreEqual(SyntaxNodeType.Parentheses, node.Type);
            Assert.AreEqual(SyntaxNodeType.Identifier, xNode.Type);
            Assert.AreEqual("x", xNode.Value);
        }

        [Test]
        public void Analyze_OperatorIllegalLHS_ThrowsException()
        {
            OperatorNode node = new OperatorNode(Operator.Addition);
            node.Left = new IdentifierNode("x");
            node.Right = new NumberNode(2);

            Assert.Throws<UndefinedSymbolException>(() => SemanticAnalyzer.Analyze(node, new Environment()));
        }

        [Test]
        public void Analyze_OperatorIllegalRHS_ThrowsException()
        {
            OperatorNode node = new OperatorNode(Operator.Addition);
            node.Left = new NumberNode(2);
            node.Right = new IdentifierNode("x");

            Assert.Throws<UndefinedSymbolException>(() => SemanticAnalyzer.Analyze(node, new Environment()));
        }

        [Test]
        public void Analyze_Operator_ReturnsOperatorNode()
        {
            OperatorNode node = new OperatorNode(Operator.Addition);
            node.Left = new NumberNode(2);
            node.Right = new NumberNode(2);

            node = (OperatorNode)SemanticAnalyzer.Analyze(node, new Environment());
            NumberNode lhsNode = (NumberNode)node.Left;
            NumberNode rhsNode = (NumberNode)node.Right;

            Assert.NotNull(node);
            Assert.NotNull(lhsNode);
            Assert.NotNull(rhsNode);
            Assert.AreEqual(node.Type, SyntaxNodeType.Operator);
            Assert.AreEqual(lhsNode.Type, SyntaxNodeType.Number);
            Assert.AreEqual(rhsNode.Type, SyntaxNodeType.Number);
            Assert.AreEqual(node.Operator, Operator.Addition);
            Assert.AreEqual(lhsNode.Value, 2);
            Assert.AreEqual(rhsNode.Value, 2);
        }

        [Test]
        public void Analyze_FunctionUndefinedName_ThrowsException()
        {
            FunctionNode node = new FunctionNode();
            node.Left = new IdentifierNode("f");
            node.Right = new NumberNode(2);

            Assert.Throws<UndefinedSymbolException>(() => SemanticAnalyzer.Analyze(node, new Environment()));
        }

        [Test]
        public void Analyze_FunctionIllegalCallExpression_ThrowsException()
        {
            FunctionNode node = new FunctionNode();
            node.Left = new IdentifierNode("ln");
            node.Right = new IdentifierNode("x");

            Assert.Throws<UndefinedSymbolException>(() => SemanticAnalyzer.Analyze(node, new Environment()));
        }

        [Test]
        public void Analyze_Function_ReturnsFunctionNode()
        {
            FunctionNode node = new FunctionNode();
            node.Left = new IdentifierNode("ln");
            node.Right = new IdentifierNode("e");

            node = (FunctionNode)SemanticAnalyzer.Analyze(node, new Environment());
            IdentifierNode nameNode = (IdentifierNode)node.Left;
            IdentifierNode callNode = (IdentifierNode)node.Right;

            Assert.NotNull(node);
            Assert.NotNull(nameNode);
            Assert.NotNull(callNode);
            Assert.AreEqual(node.Type, SyntaxNodeType.Function);
            Assert.AreEqual(nameNode.Type, SyntaxNodeType.Identifier);
            Assert.AreEqual(callNode.Type, SyntaxNodeType.Identifier);
            Assert.AreEqual(nameNode.Value, "ln");
            Assert.AreEqual(callNode.Value, "e");
        }
    }
}
