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
        public void Analyze_NumberSymbol_ReturnsNumberNode()
        {
            SyntaxNode node = new IdentifierNode(new Token(TokenType.Identifier, "x"));
            Environment environment = new Environment();
            environment.AddValue("x", 2);

            NumberNode number = (NumberNode)SemanticAnalyzer.Analyze(node, environment);

            Assert.NotNull(number);
            Assert.AreEqual(SyntaxNodeType.Number, number.Type);
            Assert.AreEqual(2, number.Value);
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
            NumberNode lhs = (NumberNode)multiplier.Left;
            NumberNode rhs = (NumberNode)multiplier.Right;

            Assert.NotNull(multiplier);
            Assert.NotNull(multiplier.Left);
            Assert.NotNull(multiplier.Right);
            Assert.AreEqual(SyntaxNodeType.Operator, multiplier.Type);
            Assert.AreEqual(Operator.Multiplication, multiplier.Operator);
            Assert.AreEqual(2, lhs.Value);
            Assert.AreEqual(1, rhs.Value);
        }
    }
}
