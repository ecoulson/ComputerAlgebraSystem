using ExpressionParser.Parsing;
using ExpressionParser.SyntaxTree;
using Mathematics.ExpressionSimplification;
using NUnit.Framework;

namespace Mathematics.Tests.ExpressionSimplification
{
    [TestFixture]
    public class NumericalOperatorSimplifierTest
    {
        [Test]
        public void Simplify_Addition_ReturnsSyntaxNode()
        {
            SyntaxNode root = GetExpression("2 + 3", new Environment());

            root = NumericalOperatorSimplifier.Simplify((OperatorNode)root);

            Assert.AreEqual("5", root.ToString());
        }

        [Test]
        public void Simplify_Subtraction_ReturnsExpression()
        {
            SyntaxNode root = GetExpression("2 - 3", new Environment());

            root = NumericalOperatorSimplifier.Simplify((OperatorNode)root);

            Assert.AreEqual("-1", root.ToString());
        }

        [Test]
        public void Simplify_Multiplication_ReturnsExpression()
        {
            SyntaxNode root = GetExpression("2 * 3", new Environment());

            root = NumericalOperatorSimplifier.Simplify((OperatorNode)root);

            Assert.AreEqual("6", root.ToString());
        }

        [Test]
        public void Simplify_Division_ReturnsExpression()
        {
            SyntaxNode root = GetExpression("2 / 4", new Environment());

            root = NumericalOperatorSimplifier.Simplify((OperatorNode)root);

            Assert.AreEqual("0.5", root.ToString());
        }

        [Test]
        public void Simplify_Exponentiation_ReturnsExpression()
        {
            SyntaxNode root = GetExpression("2 ^ 4", new Environment());

            root = NumericalOperatorSimplifier.Simplify((OperatorNode)root);

            Assert.AreEqual("16", root.ToString());
        }

        private SyntaxNode GetExpression(string expression, Environment environment)
        {
            return Parser.ParseExpression(expression, environment);
        }
    }
}
