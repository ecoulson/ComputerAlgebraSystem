using ExpressionParser.Parsing;
using NUnit.Framework;

namespace ExpressionParser.Tests.ParserTests
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void ParseExpression_SimpleAddition_ReturnExpression()
        {
            Expression expression = Parser.ParseExpression("2 + 2", new Environment());

            Assert.AreEqual("2 + 2", expression.ToString());
        }

        [Test]
        public void ParseExpression_SimpleSubtraction_ReturnExpression()
        {
            Expression expression = Parser.ParseExpression("2 - 2", new Environment());

            Assert.AreEqual("2 - 2", expression.ToString());
        }
    }
}
