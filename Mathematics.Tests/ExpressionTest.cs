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

        //[Test]
        //public void Simplify_SimpleSubtraction_ReturnsExpression()
        //{
        //    Expression expression = GetExpression("2 - 3", new Environment());

        //    expression = expression.Simplify();

        //    Assert.AreEqual("-1", expression.ToString());
        //}

        //[Test]
        //public void Simplify_SimpleMultiplication_ReturnsExpression()
        //{
        //    Expression expression = GetExpression("3 * 4", new Environment());

        //    expression = expression.Simplify();

        //    Assert.AreEqual("12", expression.ToString());
        //}

        //[Test]
        //public void Simplify_SimpleDivision_ReturnsExpression()
        //{
        //    Expression expression = GetExpression("3 / 4", new Environment());

        //    expression = expression.Simplify();

        //    Assert.AreEqual("0.75", expression.ToString());
        //}

        //[Test]
        //public void Simplify_SimpleExponentiation_ReturnsExpression()
        //{
        //    Expression expression = GetExpression("3 ^ 4", new Environment());

        //    expression = expression.Simplify();

        //    Assert.AreEqual("81", expression.ToString());
        //}

        //[Test]
        //public void Simplify_UnsimplifiableExpression_ReturnsExpression()
        //{
        //    Environment environment = new Environment();
        //    environment.AddSymbol("x");
        //    Expression expression = GetExpression("2 + x", environment);

        //    expression = expression.Simplify();

        //    Assert.AreEqual("2 + x", expression.ToString());
        //}

        //[Test]
        //public void Simplify_SimpleAlgebraicAddition_ReturnsExpression()
        //{
        //    Environment environment = new Environment();
        //    environment.AddSymbol("x");
        //    Expression expression = GetExpression("x + x", environment);

        //    expression = expression.Simplify();

        //    Assert.AreEqual("2 * x", expression.ToString());
        //}

        //[Test]
        //public void Simplify_SimpleAlgebraicAdditionOtherVariable_ReturnsExpression()
        //{
        //    Environment environment = new Environment();
        //    environment.AddSymbol("g");
        //    Expression expression = GetExpression("g + g", environment);

        //    expression = expression.Simplify();

        //    Assert.AreEqual("2 * g", expression.ToString());
        //}

        //[Test]
        //public void Simplify_SimpleAlgebraicSubtraction_ReturnsExpression()
        //{
        //    Environment environment = new Environment();
        //    environment.AddSymbol("z");
        //    Expression expression = GetExpression("z - z", environment);

        //    expression = expression.Simplify();

        //    Assert.AreEqual("0", expression.ToString());
        //}

        //[Test]
        //public void Simplify_SimpleAlgebraicMultiplication_ReturnsExpression()
        //{
        //    Environment environment = new Environment();
        //    environment.AddSymbol("z");
        //    Expression expression = GetExpression("z * z", environment);

        //    expression = expression.Simplify();

        //    Assert.AreEqual("z ^ 2", expression.ToString());
        //}

        //[Test]
        //public void Simplify_SimpleAlgebraicMultiplicationOtherVariable_ReturnsExpression()
        //{
        //    Environment environment = new Environment();
        //    environment.AddSymbol("y");
        //    Expression expression = GetExpression("y * y", environment);

        //    expression = expression.Simplify();

        //    Assert.AreEqual("y ^ 2", expression.ToString());
        //}

        //[Test]
        //public void Simplify_SimpleAlgebraicDivision_ReturnsExpression()
        //{
        //    Environment environment = new Environment();
        //    environment.AddSymbol("z");
        //    Expression expression = GetExpression("z / z", environment);

        //    expression = expression.Simplify();

        //    Assert.AreEqual("1", expression.ToString());
        //}

        //[Test]
        //public void Simplify_SimpleAlgebraicExponentiation_ReturnsExpression()
        //{
        //    Environment environment = new Environment();
        //    environment.AddSymbol("l");
        //    Expression expression = GetExpression("l ^ l", environment);

        //    expression = expression.Simplify();

        //    Assert.AreEqual("l ^ l", expression.ToString());
        //}

        //[Test]
        //public void Simplify_UnsimplifiableAlgebraicExpression_ReturnsExpression()
        //{
        //    Environment environment = new Environment();
        //    environment.AddSymbol("z");
        //    environment.AddSymbol("y");

        //    Expression expression = GetExpression("z + y", environment);

        //    expression = expression.Simplify();

        //    Assert.AreEqual("z + y", expression.ToString());
        //}

        //[Test]
        //public void Simplify_ComplexAlgebraicAddition1_ReturnsExpression()
        //{
        //    Environment environment = new Environment();
        //    environment.AddSymbol("x");
        //    Expression expression = GetExpression("x + x + x", environment);

        //    expression = expression.Simplify();

        //    Assert.AreEqual("3 * x", expression.ToString());
        //}

        //[Test]
        //public void Simplify_ComplexAlgebraicAddition2_ReturnsExpression()
        //{
        //    Environment environment = new Environment();
        //    environment.AddSymbol("x");
        //    Expression expression = GetExpression("4 * x + x", environment);

        //    expression = expression.Simplify();

        //    Assert.AreEqual("5 * x", expression.ToString());
        //}

        //[Test]
        //public void Simplify_ComplexAlgebraicAddition3_ReturnsExpression()
        //{
        //    Environment environment = new Environment();
        //    environment.AddSymbol("x");
        //    Expression expression = GetExpression("x + 3 * x", environment);

        //    expression = expression.Simplify();

        //    Assert.AreEqual("4 * x", expression.ToString());
        //}

        //[Test]
        //public void Simplify_ComplexAlgebraicAddition4_ReturnsExpression()
        //{
        //    Environment environment = new Environment();
        //    environment.AddSymbol("j");
        //    Expression expression = GetExpression("6 * j + 9 * j", environment);

        //    expression = expression.Simplify();

        //    Assert.AreEqual("15 * j", expression.ToString());
        //}

        //[Test]
        //public void Simplify_ComplexAlgebraicAddition5_ReturnsExpression()
        //{
        //    Environment environment = new Environment();
        //    environment.AddSymbol("x");
        //    environment.AddSymbol("y");
        //    Expression expression = GetExpression("x + x + y", environment);

        //    expression = expression.Simplify();

        //    Assert.AreEqual("2 * x + y", expression.ToString());
        //}

        //[Test]
        //public void Simplify_ComplexAlgebraicAddition6_ReturnsExpression()
        //{
        //    Environment environment = new Environment();
        //    environment.AddSymbol("x");
        //    environment.AddSymbol("y");
        //    Expression expression = GetExpression("x * 2 + y", environment);

        //    expression = expression.Simplify();

        //    Assert.AreEqual("x * 2 + y", expression.ToString());
        //}

        //[Test]
        //public void Simplify_ComplexAlgebraicAddition7_ReturnsExpression()
        //{
        //    Environment environment = new Environment();
        //    environment.AddSymbol("x");
        //    environment.AddSymbol("y");
        //    Expression expression = GetExpression("x * 2 + x", environment);

        //    expression = expression.Simplify();

        //    Assert.AreEqual("3 * x", expression.ToString());
        //}

        //[Test]
        //public void Simplify_ComplexAlgebraicAddition8_ReturnsExpression()
        //{
        //    Environment environment = new Environment();
        //    environment.AddSymbol("x");
        //    environment.AddSymbol("y");
        //    Expression expression = GetExpression("x + y + x", environment);

        //    expression = expression.Simplify();

        //    Assert.AreEqual("2 * x + y", expression.ToString());
        //}

        //[Test]
        //public void Simplify_ComplexAlgebraicAddition9_ReturnsExpression()
        //{
        //    Environment environment = new Environment();
        //    environment.AddSymbol("k");
        //    environment.AddSymbol("g");
        //    Expression expression = GetExpression("g + k + k", environment);

        //    expression = expression.Simplify();

        //    Assert.AreEqual("2 * k + g", expression.ToString());
        //}

        private Expression GetExpression(string expression, Environment environment)
        {
            SyntaxNode root = Parser.ParseExpression(expression, environment);
            return new Expression(root, environment);
        }
    }
}
