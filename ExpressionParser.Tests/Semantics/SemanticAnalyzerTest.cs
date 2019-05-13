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
            SyntaxNode node = new OperatorNode(Operator.Multiplication)
            {
                Left = new NumberNode(-1),
                Right = new NumberNode(2)
            };

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

            node = SemanticAnalyzer.Analyze(node, environment);

            Assert.AreEqual("x", node.ToString());
        }

        [Test]
        public void Analyze_VariableSymbol_ReturnsIdentifierNode()
        {
            SyntaxNode node = new IdentifierNode("x");
            Environment environment = new Environment();
            environment.AddSymbol("x");

            node = SemanticAnalyzer.Analyze(node, environment);

            Assert.AreEqual("x", node.ToString());
        }

        [Test]
        public void Analyze_NumberValue_ReturnsNumberNode()
        {
            SyntaxNode node = new NumberNode(1);

            node = SemanticAnalyzer.Analyze(node, new Environment());

            Assert.AreEqual("1", node.ToString());
        }

        [Test]
        public void Analyze_AmbigiousFunction_ReturnsFunctionNode()
        {
            IdentifierNode left = new IdentifierNode("x");
            SyntaxNode right = new NumberNode(1);
            SyntaxNode root = new FunctionOrDistributionNode(left, right);

            Environment environment = new Environment();
            environment.AddFunction("x", null);

            root = SemanticAnalyzer.Analyze(root, environment);

            Assert.AreEqual("x(1)", root.ToString());
        }

        [Test]
        public void Analyze_AmbigiousSymbolDistribution_ReturnsOperatorNode()
        {
            IdentifierNode left = new IdentifierNode("x");
            SyntaxNode right = new NumberNode(1);
            SyntaxNode root = new FunctionOrDistributionNode(left, right);

            Environment environment = new Environment();
            environment.AddSymbol("x");

            root = SemanticAnalyzer.Analyze(root, environment);

            Assert.AreEqual("x * 1", root.ToString());
        }

        [Test]
        public void Analyze_AmbigiousValueDistribution_ReturnsOperatorNode()
        {
            IdentifierNode left = new IdentifierNode("x");
            SyntaxNode right = new NumberNode(1);
            SyntaxNode root = new FunctionOrDistributionNode(left, right);

            Environment environment = new Environment();
            environment.AddValue("x", 2);

            root = SemanticAnalyzer.Analyze(root, environment);

            Assert.AreEqual("x * 1", root.ToString());
        }

        [Test]
        public void Analyze_AmbiguousPredefinedFunction_ReturnsFunctionNode()
        {
            IdentifierNode left = new IdentifierNode("sin");
            SyntaxNode right = new NumberNode(1);
            SyntaxNode root = new FunctionOrDistributionNode(left, right);

            root = SemanticAnalyzer.Analyze(root, new Environment());

            Assert.AreEqual("sin(1)", root.ToString());
        }

        [Test]
        public void Analyze_RecognizeShorthandMultiplicationInsideIdentifer_ReturnsOperatorNode()
        {
            SyntaxNode root = new IdentifierNode("xy");
            Environment environment = new Environment();
            environment.AddSymbol("x");
            environment.AddSymbol("y");

            root = SemanticAnalyzer.Analyze(root, environment);

            Assert.AreEqual("x * y", root.ToString());
        }

        [Test]
        public void Analyze_RecognizeShorthandMultiplicationInsideIdentiferWithOneVariable_ReturnsOperatorNode()
        {
            SyntaxNode root = new IdentifierNode("xx");
            Environment environment = new Environment();
            environment.AddSymbol("x");

            root = SemanticAnalyzer.Analyze(root, environment);

            Assert.AreEqual("x * x", root.ToString());
        }

        [Test]
        public void Analyze_RecognizeShorthandMultiplicationInsideIdentifierWithPredefinedSymbols_ReturnsIdentifierNode()
        {
            SyntaxNode root = new IdentifierNode("xe");
            Environment environment = new Environment();
            environment.AddSymbol("x");

            root = SemanticAnalyzer.Analyze(root, environment);

            Assert.AreEqual("x * e", root.ToString());
        }

        [Test]
        public void Analyze_Keyword_ReturnsIdentifierNode()
        {
            SyntaxNode root = new IdentifierNode("e");

            root = SemanticAnalyzer.Analyze(root, new Environment());

            Assert.AreEqual("e", root.ToString());
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
            SyntaxNode root = new ParenthesesNode(new IdentifierNode("x"));

            root = SemanticAnalyzer.Analyze(root, environment);

            Assert.AreEqual("(x)", root.ToString());
        }

        [Test]
        public void Analyze_OperatorIllegalLHS_ThrowsException()
        {
            OperatorNode node = new OperatorNode(Operator.Addition)
            {
                Left = new IdentifierNode("x"),
                Right = new NumberNode(2)
            };

            Assert.Throws<UndefinedSymbolException>(() => SemanticAnalyzer.Analyze(node, new Environment()));
        }

        [Test]
        public void Analyze_OperatorIllegalRHS_ThrowsException()
        {
            OperatorNode node = new OperatorNode(Operator.Addition)
            {
                Left = new NumberNode(2),
                Right = new IdentifierNode("x")
            };

            Assert.Throws<UndefinedSymbolException>(() => SemanticAnalyzer.Analyze(node, new Environment()));
        }

        [Test]
        public void Analyze_Operator_ReturnsOperatorNode()
        {
            SyntaxNode node = new OperatorNode(Operator.Addition)
            {
                Left = new NumberNode(2),
                Right = new NumberNode(2)
            };

            node = SemanticAnalyzer.Analyze(node, new Environment());

            Assert.AreEqual("2 + 2", node.ToString());
        }

        [Test]
        public void Analyze_FunctionUndefinedName_ThrowsException()
        {
            FunctionNode node = new FunctionNode
            {
                Left = new IdentifierNode("f"),
                Right = new NumberNode(2)
            };

            Assert.Throws<UndefinedSymbolException>(() => SemanticAnalyzer.Analyze(node, new Environment()));
        }

        [Test]
        public void Analyze_FunctionIllegalCallExpression_ThrowsException()
        {
            FunctionNode node = new FunctionNode
            {
                Left = new IdentifierNode("ln"),
                Right = new IdentifierNode("x")
            };

            Assert.Throws<UndefinedSymbolException>(() => SemanticAnalyzer.Analyze(node, new Environment()));
        }

        [Test]
        public void Analyze_Function_ReturnsFunctionNode()
        {
            SyntaxNode root = new FunctionNode
            {
                Left = new IdentifierNode("ln"),
                Right = new IdentifierNode("e")
            };

            root = SemanticAnalyzer.Analyze(root, new Environment());

            Assert.AreEqual("ln(e)", root.ToString());
        }
    }
}
