using ExpressionParser.Parsing;
using ExpressionParser.SyntaxTree;
using Mathematics.ExpressionSimplification;
using NUnit.Framework;

namespace Mathematics.Tests.ExpressionSimplification
{
    [TestFixture]
    public class RightNestedAlgebraicSimplifierTest
    {
        [Test]
        public void Simplify_ComplexAlgebraicAddition3_ReturnsExpression()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode root = GetExpression("x + 3 * x", environment);

            root = RightNestedAlgebraicSimplifier.Simplify((OperatorNode)root, environment);

            Assert.AreEqual("4 * x", root.ToString());
        }

        private SyntaxNode GetExpression(string expression, Environment environment)
        {
            return Parser.ParseExpression(expression, environment);
        }
    }
}
