using ExpressionParser.Parsing;
using ExpressionParser.SyntaxTree;
using NUnit.Framework;

namespace ExpressionParser.Tests.ParserTests
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void ParseExpression_Addition_ReturnsString()
        {
            TestExpression(
                "2 + 2",
                "2 + 2",
                new Environment()
            );
        }

        [Test]
        public void ParseExpression_Subtraction_ReturnString()
        {
            TestExpression(
                "2 - 2",
                "2 + -2",
                new Environment()
            );
        }

        [Test]
        public void ParseExpression_Exponentiation_ReturnsString()
        {
            TestExpression(
                "2 ^ 3",
                "2 ^ 3",
                new Environment()
            );
        }
        [Test]
        public void ParseExpression_Multiplication_ReturnsString()
        {
            TestExpression(
                "2 * 3", 
                "2 * 3",
                new Environment()
            );
        }

        [Test]
        public void ParseExpression_Division_ReturnsString()
        {
            TestExpression(
                "2 / 3",
                "2 * 3 ^ -1", 
                new Environment()
            );
        }

        [Test]
        public void ParseExpression_ArithmeticExpression_ReturnsString()
        {
            TestExpression(
                "1 + 2 * 3 / 4 ^ 5",
                "1 + 2 * 3 * 4 ^ 5 ^ -1",
                new Environment()
            );
        }

        [Test]
        public void ParseExpression_ComplexExpression_ReturnsString()
        {
            TestExpression(
                "(1 + 2) * (3 / 4) ^ 5",
                "(1 + 2) * (3 * 4 ^ -1) ^ 5",
                new Environment()
            );
        }

        [Test]
        public void ParseExpression_KeywordExpression_ReturnsString()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");

            TestExpression(
                "ln(x^2 + x - 3)", 
                "ln(x ^ 2 + x + -3)", 
                environment
            );
        }

        [Test]
        public void ParseExpression_DistributiveExpession_ReturnsString()
        {
            TestExpression(
                "2(3 + 4)", 
                "2 * (3 + 4)", 
                new Environment()
            );
        }

        private void TestExpression(string rawExpression, string expected, Environment environment)
        {
            SyntaxNode expression = Parser.ParseExpression(rawExpression, environment);
            Assert.AreEqual(expected, expression.ToString());
        }
    }
}
