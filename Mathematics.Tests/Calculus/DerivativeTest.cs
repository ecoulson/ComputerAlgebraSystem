using ExpressionParser.Parsing;
using ExpressionParser.SyntaxTree;
using Mathematics.Calculus;
using NUnit.Framework;

namespace Mathematics.Tests.Calculus
{
    [TestFixture]
    public class DerivativeTest
    {
        [Test]
        public void Derive_Constant_Returns0()
        {
            Environment environment = new Environment();
            SyntaxNode node = Parser.ParseExpression("42", environment);
            Expression expression = new Expression(node, environment);
            Derivative derivative = new Derivative(expression, "x");

            Assert.AreEqual(derivative.Derive().ToString(), "0");
        }

        [Test]
        public void Derive_EulersConstant_Returns0()
        {
            Environment environment = new Environment();
            SyntaxNode node = Parser.ParseExpression("e", environment);
            Expression expression = new Expression(node, environment);
            Derivative derivative = new Derivative(expression, "x");

            Assert.AreEqual(derivative.Derive().ToString(), "0");
        }

        [Test]
        public void Derive_ComplexConstant_Returns0()
        {
            Environment environment = new Environment();
            SyntaxNode node = Parser.ParseExpression("(2 * 2) + 4 ^ 2 - 5 / 5", environment);
            Expression expression = new Expression(node, environment);
            Derivative derivative = new Derivative(expression, "x");

            Assert.AreEqual(derivative.Derive().ToString(), "0");
        }

        [Test]
        public void Derive_x_Returns1()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = Parser.ParseExpression("x", environment);
            Expression expression = new Expression(node, environment);
            Derivative derivative = new Derivative(expression, "x");

            Assert.AreEqual(derivative.Derive().ToString(), "1");
        }

        [Test]
        public void Derive_2x_Returns2()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = Parser.ParseExpression("2x", environment);
            Expression expression = new Expression(node, environment);
            Derivative derivative = new Derivative(expression, "x");

            Assert.AreEqual(derivative.Derive().ToString(), "2 * 1");
        }

        [Test]
        public void Derive_ComplexConstantTimesx_ReturnsComplexConstant()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = Parser.ParseExpression("(2 * 2 + 4 ^ 2 - 5 / 5 + pi)x", environment);
            Expression expression = new Expression(node, environment);
            Derivative derivative = new Derivative(expression, "x");

            Assert.AreEqual(derivative.Derive().ToString(), "(2 * 2 + 4 ^ 2 - 5 / 5 + pi) * 1");
        }

        [Test]
        public void Derive_ProductRuleOnXTimesX_ReturnsSyntaxNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = Parser.ParseExpression("x * x", environment);
            Expression expression = new Expression(node, environment);
            Derivative derivative = new Derivative(expression, "x");

            Assert.AreEqual(derivative.Derive().ToString(), "x * 1 + 1 * x");
        }

        [Test]
        public void Derive_ComplexProductRuleOn2xTimes6x_ReturnsSyntaxNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = Parser.ParseExpression("2 * x * 6 * x", environment);
            Expression expression = new Expression(node, environment);
            Derivative derivative = new Derivative(expression, "x");
            
            Assert.AreEqual(derivative.Derive().ToString(), "2 * x * 6 * 1 + 6 * 2 * 1 * x");
        }

        [Test]
        public void Derive_PowerRuleOnXSquared_ReturnsSyntaxNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = Parser.ParseExpression("x^2", environment);
            Expression expression = new Expression(node, environment);
            Derivative derivative = new Derivative(expression, "x");

            Assert.AreEqual(derivative.Derive().ToString(), "2 * x ^ 2 - 1 * 1");
        }

        [Test]
        public void Derive_ExponentialRuleOnCToThe2x_ReturnsSyntaxNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = Parser.ParseExpression("3 ^ (2 * x)", environment);
            Expression expression = new Expression(node, environment);
            Derivative derivative = new Derivative(expression, "x");

            Assert.AreEqual(derivative.Derive().ToString(), "ln(3) * 3 ^ (2 * x) * 2 * 1");
        }

        [Test]
        public void Derive_NaturalLog_ReturnsSyntaxNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = Parser.ParseExpression("ln(x)", environment);
            Expression expression = new Expression(node, environment);
            Derivative derivative = new Derivative(expression, "x");

            Assert.AreEqual(derivative.Derive().ToString(), "1 / x * 1");
        }

        [Test]
        public void Derive_TowerPowerRule_ReturnsSyntaxNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = Parser.ParseExpression("x ^ x", environment);
            Expression expression = new Expression(node, environment);
            Derivative derivative = new Derivative(expression, "x");

            Assert.AreEqual(derivative.Derive().ToString(), "x ^ x * x * 1 / x * 1 + 1 * ln(x)");
        }

        [Test]
        public void Derive_Addition_ReturnsSyntaxNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = Parser.ParseExpression("x ^ 2 + x + 3", environment);
            Expression expression = new Expression(node, environment);
            Derivative derivative = new Derivative(expression, "x");

            Assert.AreEqual(derivative.Derive().ToString(), "2 * x ^ 2 - 1 * 1 + 1 + 0");
        }

        [Test]
        public void Derive_Subtraction_ReturnsSyntaxNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = Parser.ParseExpression("x ^ 2 - x - 3", environment);
            Expression expression = new Expression(node, environment);
            Derivative derivative = new Derivative(expression, "x");

            Assert.AreEqual(derivative.Derive().ToString(), "2 * x ^ 2 - 1 * 1 - 1 - 0");
        }

        [Test]
        public void Derive_QuotientRule_ReturnsSyntaxNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = Parser.ParseExpression("x^2 / x", environment);
            Expression expression = new Expression(node, environment);
            Derivative derivative = new Derivative(expression, "x");

            Assert.AreEqual(derivative.Derive().ToString(), "x * 2 * x ^ 2 - 1 * 1 - x ^ 2 * 1 / x ^ 2");
        }

        [Test]
        public void Derive_DivisionWithConstantDenominator_ReturnsSyntaxNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = Parser.ParseExpression("x / 2", environment);
            Expression expression = new Expression(node, environment);
            Derivative derivative = new Derivative(expression, "x");

            Assert.AreEqual(derivative.Derive().ToString(), "1 / 2");
        }

        [Test]
        public void Derive_DivisionWithVariableDenominator_ReturnsSyntaxNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = Parser.ParseExpression("1 / x", environment);
            Expression expression = new Expression(node, environment);
            Derivative derivative = new Derivative(expression, "x");

            Assert.AreEqual(derivative.Derive().ToString(), "1 * -1 * x ^ -1 - 1 * 1");
        }

        [Test]
        public void Derive_Sin_ReturnsSyntaxNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = Parser.ParseExpression("sin(x)", environment);
            Expression expression = new Expression(node, environment);
            Derivative derivative = new Derivative(expression, "x");

            Assert.AreEqual(derivative.Derive().ToString(), "cos(x) * 1");
        }

        [Test]
        public void Derive_Cos_ReturnsSyntaxNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = Parser.ParseExpression("cos(x)", environment);
            Expression expression = new Expression(node, environment);
            Derivative derivative = new Derivative(expression, "x");

            Assert.AreEqual(derivative.Derive().ToString(), "-1 * sin(x) * 1");
        }

        [Test]
        public void Derive_Tan_ReturnsSyntaxNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = Parser.ParseExpression("tan(x)", environment);
            Expression expression = new Expression(node, environment);
            Derivative derivative = new Derivative(expression, "x");

            Assert.AreEqual(derivative.Derive().ToString(), "sec(x) ^ 2 * 1");
        }

        [Test]
        public void Derive_Sec_ReturnsSyntaxNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = Parser.ParseExpression("sec(x)", environment);
            Expression expression = new Expression(node, environment);
            Derivative derivative = new Derivative(expression, "x");

            Assert.AreEqual(derivative.Derive().ToString(), "sec(x) * tan(x) * 1");
        }
    }
}
