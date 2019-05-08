using ExpressionParser.Lex;
using ExpressionParser.Parser;
using ExpressionParser.Semantics;
using ExpressionParser.SyntaxTree;
using NUnit.Framework;

namespace ExpressionParser.Tests.Semantics
{
    [TestFixture]
    public class SemanticAnalyzerTest
    {
        [Test]
        public void Analyze_Identifier_ThrowsUndefinedIdentifier()
        {
            SyntaxNode node = new IdentifierNode(new Token(TokenType.Identifier, "x"));
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
            SyntaxNode node = new IdentifierNode(new Token(TokenType.Identifier, "x"));
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
            SyntaxNode node = new IdentifierNode(new Token(TokenType.Identifier, "x"));
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
            environment.AddFunction("x", new Expression(null));

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
    }
}
