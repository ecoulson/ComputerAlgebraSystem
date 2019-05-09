using ExpressionParser.Parsing;
using ExpressionParser.SyntaxTree;
using NUnit.Framework;

namespace Mathematics.Tests
{
    [TestFixture]
    public class ExpressionTest
    {
        [Test]
        public void Simplify_SimpleAddition_ReturnsExpression()
        {
            Expression expression = GetExpression("2 + 3", new Environment());

            expression = expression.Simplify();

            Assert.AreEqual("5", expression.ToString());
        }

        [Test]
        public void Simplify_SimpleSubtraction_ReturnsExpression()
        {
            Expression expression = GetExpression("2 - 3", new Environment());

            expression = expression.Simplify();

            Assert.AreEqual("-1", expression.ToString());
        }

        [Test]
        public void Simplify_SimpleMultiplication_ReturnsExpression()
        {
            Expression expression = GetExpression("3 * 4", new Environment());

            expression = expression.Simplify();

            Assert.AreEqual("12", expression.ToString());
        }

        [Test]
        public void Simplify_SimpleDivision_ReturnsExpression()
        {
            Expression expression = GetExpression("3 / 4", new Environment());

            expression = expression.Simplify();

            Assert.AreEqual("0.75", expression.ToString());
        }

        [Test]
        public void Simplify_SimpleExponentiation_ReturnsExpression()
        {
            Expression expression = GetExpression("3 ^ 4", new Environment());

            expression = expression.Simplify();

            Assert.AreEqual("81", expression.ToString());
        }

        [Test]
        public void Simplify_SimpleUnsimplifiableExpression_ReturnsExpression()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            Expression expression = GetExpression("2 + x", environment);

            expression = expression.Simplify();

            Assert.AreEqual("2 + x", expression.ToString());
        }

        private Expression GetExpression(string expression, Environment environment)
        {
            SyntaxNode root = Parser.ParseExpression(expression, environment);
            return new Expression(root, environment);
        }
    }
}
