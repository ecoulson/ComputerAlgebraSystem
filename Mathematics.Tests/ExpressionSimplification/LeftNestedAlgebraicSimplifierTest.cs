using ExpressionParser.Parsing;
using ExpressionParser.SyntaxTree;
using Mathematics.ExpressionSimplification;
using NUnit.Framework;

namespace Mathematics.Tests.ExpressionSimplification
{
    [TestFixture]
    public class LeftNestedAlgebraicSimplifierTest
    {
        [Test]
        public void Simplify_LeftNestedTerm_ReturnsOperatorNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode root = GetExpression("2 * x + x", environment);

            root = LeftNestedAlgebraicSimplifier.Simplify((OperatorNode)root, environment);

            Assert.AreEqual("3 * x", root.ToString());
        }

        [Test]
        public void Simplify_LeftNestedReverseTerm_ReturnsOperatorNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode root = GetExpression("x * 2 + x", environment);

            root = LeftNestedAlgebraicSimplifier.Simplify((OperatorNode)root, environment);

            Assert.AreEqual("3 * x", root.ToString());
        }

        [Test]
        public void Simplify_LeftNestedTermDifferentIdentifiers_ReturnsOperatorNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            environment.AddSymbol("y");
            SyntaxNode root = GetExpression("2 * x + y", environment);

            root = LeftNestedAlgebraicSimplifier.Simplify((OperatorNode)root, environment);

            Assert.AreEqual("2 * x + y", root.ToString());
        }

        [Test]
        public void Simplify_LeftNestedReverseTermDifferentIdentifiers_ReturnsOperatorNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            environment.AddSymbol("y");
            SyntaxNode root = GetExpression("x * 2 + y", environment);

            root = LeftNestedAlgebraicSimplifier.Simplify((OperatorNode)root, environment);

            Assert.AreEqual("x * 2 + y", root.ToString());
        }

        [Test]
        public void Simplify_LeftNestedIdentifiers_ReturnsOperatorNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            environment.AddSymbol("y");
            SyntaxNode root = GetExpression("x + y + x", environment);

            root = LeftNestedAlgebraicSimplifier.Simplify((OperatorNode)root, environment);

            Assert.AreEqual("2 * x + y", root.ToString());
        }

        [Test]
        public void Simplify_LeftNestedIdentifiersSwapped_ReturnsOperatorNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            environment.AddSymbol("y");
            environment.AddSymbol("z");
            SyntaxNode root = GetExpression("y + x + z", environment);

            root = LeftNestedAlgebraicSimplifier.Simplify((OperatorNode)root, environment);

            Assert.AreEqual("y + x + z", root.ToString());
        }

        [Test]
        public void Simplify_LeftNestedDifferentIdentifiers_ReturnsOperatorNode()
        {

        }

        private SyntaxNode GetExpression(string expression, Environment environment)
        {
            return Parser.ParseExpression(expression, environment);
        }
    }
}
