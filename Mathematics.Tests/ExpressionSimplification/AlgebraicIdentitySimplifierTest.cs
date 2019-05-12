using ExpressionParser.Parsing;
using ExpressionParser.SyntaxTree;
using Mathematics.ExpressionSimplification;
using NUnit.Framework;

namespace Mathematics.Tests.ExpressionSimplification
{
    [TestFixture]
    public class AlgebraicIdentitySimplifierTest
    {
        [Test]
        public void Simplify_AddingTwoOfTheSameVariables_ReturnsOperatorNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = GetExpression("x + x", environment);

            node = AlgebraicIdentitySimplifier.Simplify((OperatorNode)node, environment);

            Assert.AreEqual("2 * x", node.ToString());
        }

        [Test]
        public void Simplify_SubtractingTwoOfTheSameVariables_ReturnsOperatorNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = GetExpression("x - x", environment);

            node = AlgebraicIdentitySimplifier.Simplify((OperatorNode)node, environment);

            Assert.AreEqual("0", node.ToString());
        }

        [Test]
        public void Simplify_MultiplicationTwoOfTheSameVariables_ReturnsOperatorNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = GetExpression("x * x", environment);

            node = AlgebraicIdentitySimplifier.Simplify((OperatorNode)node, environment);

            Assert.AreEqual("x ^ 2", node.ToString());
        }

        [Test]
        public void Simplify_DivisionTwoOfTheSameVariables_ReturnsOperatorNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = GetExpression("x / x", environment);

            node = AlgebraicIdentitySimplifier.Simplify((OperatorNode)node, environment);

            Assert.AreEqual("1", node.ToString());
        }

        [Test]
        public void Simplify_ExponentiationTwoOfTheSameVariables_ReturnsOperatorNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = GetExpression("x ^ x", environment);

            node = AlgebraicIdentitySimplifier.Simplify((OperatorNode)node, environment);

            Assert.AreEqual("x ^ x", node.ToString());
        }

        [Test]
        public void Simplify_AddingTwoDifferentVariables_ReturnsOperatorNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            environment.AddSymbol("y");
            SyntaxNode node = GetExpression("x + y", environment);

            node = AlgebraicIdentitySimplifier.Simplify((OperatorNode)node, environment);

            Assert.AreEqual("x + y", node.ToString());
        }


        private SyntaxNode GetExpression(string expression, Environment environment)
        {
            return Parser.ParseExpression(expression, environment);
        }
    }
}
