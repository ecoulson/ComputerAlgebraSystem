using ExpressionParser.Parsing;
using ExpressionParser.SyntaxTree;
using Mathematics.ExpressionSimplification;
using NUnit.Framework;

namespace Mathematics.Tests.ExpressionSimplification
{
    [TestFixture]
    public class OperatorSimplificationTest
    {
        [Test]
        public void Simplify_OperatorChildrenAreBothNumbers_ReturnsNumberNode() 
        {
            SyntaxNode node = GetExpression("2 + 2", new Environment());
            node = OperatorSimplification.Simplify((OperatorNode)node, new Environment());

            Assert.NotNull(node);
            Assert.AreEqual("4", node.ToString());
        }

        [Test]
        public void Simplify_OperatorChildrenAreBothTheSameIdentifier_ReturnsOperatorNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = GetExpression("x + x", environment);
            node = OperatorSimplification.Simplify((OperatorNode)node, environment);

            Assert.NotNull(node);
            Assert.AreEqual("2 * x", node.ToString());
        }

        [Test]
        public void Simplify_OperatorWithNumberAndFunction_ReturnsOperatorNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            environment.AddFunction("f", GetExpression("x", environment));
            SyntaxNode node = GetExpression("2 + f(x)", environment);
            node = OperatorSimplification.Simplify((OperatorNode)node, environment);

            Assert.NotNull(node);
            Assert.AreEqual("2 + f(x)", node.ToString());
        }

        [Test]
        public void Simplify_LeftNestedAlgebraicOperator_ReturnsOperatorNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = GetExpression("2 * x + x", environment);
            node = OperatorSimplification.Simplify((OperatorNode)node, environment);

            Assert.NotNull(node);
            Assert.AreEqual("3 * x", node.ToString());
        }

        [Test]
        public void Simplify_RightNestedAlgebraicOperator_ReturnsOperatorNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = GetExpression("x + 2 * x", environment);
            node = OperatorSimplification.Simplify((OperatorNode)node, environment);

            Assert.NotNull(node);
            Assert.AreEqual("3 * x", node.ToString());
        }

        [Test]
        public void Simplify_NestedAlgebraicOperator_ReturnsOperatorNode()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            SyntaxNode node = GetExpression("3 * x + 2 * x", environment);
            node = OperatorSimplification.Simplify((OperatorNode)node, environment);

            Assert.NotNull(node);
            Assert.AreEqual("5 * x", node.ToString());
        }

        private SyntaxNode GetExpression(string expression, Environment environment)
        {
            return Parser.ParseExpression(expression, environment);
        }
    }
}
