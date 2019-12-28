using ExpressionParser.Parsing;
using ExpressionParser.SyntaxTree;
using Mathematics.ExpressionSimplification;
using NUnit.Framework;

namespace Mathematics.Tests.ExpressionSimplification
{
    [TestFixture]
    public class ConstantSimplifierTest
    {
        [Test]
        public void Simplify_MultiplyBy1_X()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            ConstantSimplifier simplifier = new ConstantSimplifier(environment);
            SyntaxNode node = Parser.ParseExpression("x * 1", environment);
            Assert.AreEqual("x", simplifier.Simplify(node).ToString());
        }

        [Test]
        public void Simplify_MultiplyBy1ManyTimes_X()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            ConstantSimplifier simplifier = new ConstantSimplifier(environment);
            SyntaxNode node = Parser.ParseExpression("x * 1 * 1 * 1", environment);
            Assert.AreEqual("x", simplifier.Simplify(node).ToString());
        }

        //[Test]
        //public void Simplify_ExponentiationBy1_X()
        //{
        //    Environment environment = new Environment();
        //    environment.AddSymbol("x");
        //    ConstantSimplifier simplifier = new ConstantSimplifier(environment);
        //    SyntaxNode node = Parser.ParseExpression("x ^ 1", environment);
        //    Assert.AreEqual("x", simplifier.Simplify(node).ToString());
        //}

        //[Test]
        //public void Simplify_Add0_X()
        //{
        //    Environment environment = new Environment();
        //    environment.AddSymbol("x");
        //    ConstantSimplifier simplifier = new ConstantSimplifier(environment);
        //    SyntaxNode node = Parser.ParseExpression("x + 0", environment);
        //    Assert.AreEqual("x", simplifier.Simplify(node).ToString());
        //}

        //[Test]
        //public void Simplify_Subtract0_X()
        //{
        //    Environment environment = new Environment();
        //    environment.AddSymbol("x");
        //    ConstantSimplifier simplifier = new ConstantSimplifier(environment);
        //    SyntaxNode node = Parser.ParseExpression("x - 0", environment);
        //    Assert.AreEqual("x", simplifier.Simplify(node).ToString());
        //}

        [Test]
        public void Simplify_Addition_3()
        {
            Environment environment = new Environment();
            ConstantSimplifier simplifier = new ConstantSimplifier(environment);
            SyntaxNode node = Parser.ParseExpression("2 + 1", environment);
            Assert.AreEqual("3", simplifier.Simplify(node).ToString());
        }

        [Test]
        public void Simplify_Subtraction_1()
        {
            Environment environment = new Environment();
            ConstantSimplifier simplifier = new ConstantSimplifier(environment);
            SyntaxNode node = Parser.ParseExpression("2 - 1", environment);
            Assert.AreEqual("1", simplifier.Simplify(node).ToString());
        }

        [Test]
        public void Simplify_Multiplication_6()
        {
            Environment environment = new Environment();
            ConstantSimplifier simplifier = new ConstantSimplifier(environment);
            SyntaxNode node = Parser.ParseExpression("2 * 3", environment);
            Assert.AreEqual("6", simplifier.Simplify(node).ToString());
        }

        [Test]
        public void Simplify_Division_1()
        {
            Environment environment = new Environment();
            ConstantSimplifier simplifier = new ConstantSimplifier(environment);
            SyntaxNode node = Parser.ParseExpression("2 * 2 ^ -1", environment);
            Assert.AreEqual("1", simplifier.Simplify(node).ToString());
        }

        [Test]
        public void Simplify_Exponentiation_8()
        {
            Environment environment = new Environment();
            ConstantSimplifier simplifier = new ConstantSimplifier(environment);
            SyntaxNode node = Parser.ParseExpression("2 ^ 3", environment);
            Assert.AreEqual("8", simplifier.Simplify(node).ToString());
        }

        [Test]
        public void Simplify_ComplexAlgebraExpression_SimplifiesMultiplication()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            ConstantSimplifier simplifier = new ConstantSimplifier(environment);
            SyntaxNode node = Parser.ParseExpression("3 * x * 2 + 12", environment);
            Assert.AreEqual("x * 6 + 12", simplifier.Simplify(node).ToString());
        }

        [Test]
        public void Simplify_ComplexArithmeticExpression_SimplifiesDivision()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            ConstantSimplifier simplifier = new ConstantSimplifier(environment);
            SyntaxNode node = Parser.ParseExpression("3 * 6 / 3 + 12", environment);
            Assert.AreEqual("18", simplifier.Simplify(node).ToString());
        }

        [Test]
        public void Simplify_ComplexAlgebraExpression_SimplifiesDivision()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");
            ConstantSimplifier simplifier = new ConstantSimplifier(environment);
            SyntaxNode node = Parser.ParseExpression("3 * x / 3 + 12", environment);
            Assert.AreEqual("x + 12", simplifier.Simplify(node).ToString());
        }
    }
}
