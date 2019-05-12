using ExpressionParser.Parsing;
using ExpressionParser.SyntaxTree;
using Mathematics.ExpressionSimplification;
using NUnit.Framework;

namespace Mathematics.Tests.ExpressionSimplification
{
    [TestFixture]
    public class NestedAlgebraicSimplifierTest
    {
        [Test]
        public void Simplify_NestedAddition_ReturnsOperatorNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("j");
            SyntaxNode root = GetExpression("6 * j + 9 * j", environment);

            root = NestedAlgebraicSimplifier.Simplify((OperatorNode)root, environment);

            Assert.AreEqual("15 * j", root.ToString());
        }

        private SyntaxNode GetExpression(string expression, Environment environment)
        {
            return Parser.ParseExpression(expression, environment);
        }
    }
}
