
using System.Collections.Generic;
using ExpressionParser.Parsing;
using ExpressionParser.SyntaxTree;
using Mathematics;
using NUnit.Framework;

namespace Mathematics.Tests
{
    [TestFixture]
    public class FunctionTest
    {
        [Test]
        public void ToString_DisplaysFunction_ReturnsString()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            Expression expression = new Expression(Parser.ParseExpression("1/2x+2", environment), environment);
            Function f = new Function("f", new HashSet<string> { "x" }, expression);
            Assert.AreEqual(f.ToString(), "f(x) = 1 / 2 * x + 2");
        }

        [Test]
        public void ToString_DisplaysFunction_ReturnsCachedString()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            Expression expression = new Expression(Parser.ParseExpression("1/2x+2", environment), environment);
            Function f = new Function("f", new HashSet<string> { "x" }, expression);
            Assert.AreEqual(f.ToString(), "f(x) = 1 / 2 * x + 2");
            Assert.AreEqual(f.ToString(), "f(x) = 1 / 2 * x + 2");
        }
    }
}
