using System;
using System.Collections.Generic;
using ExpressionParser.Lex;
using ExpressionParser.SyntaxTree;
using NUnit.Framework;

namespace ExpressionParser.Tests.SyntaxTree
{
    [TestFixture]
    public class SyntaxTreeBuilderComplexTests
    {
        #region Tree Tests

        [Test]
        public void BuildTree_EmptyExpression_ReturnsEmptyExpressionTree()
        {
            List<Token> tokens = new List<Token>();

            EndOfTokenStreamException exception = Assert.Throws<EndOfTokenStreamException>(
                () => SyntaxTreeBuilder.BuildTree(tokens)
            );

            Assert.AreEqual("Unexpected end of token stream", exception.Message);
        }

        [Test]
        public void BuildTree_MultipleAdditions_ReturnsOperatorNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Addition),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Addition),
                new Token(TokenType.Number, "3"),
            };

            SyntaxNode node = SyntaxTreeBuilder.BuildTree(tokens);

            Assert.AreEqual("1 + 2 + 3", node.ToString());
        }

        [Test]
        public void BuildTree_MultipleSubtractions_ReturnsOperatorNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Subtraction),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Subtraction),
                new Token(TokenType.Number, "3"),
            };

            SyntaxNode node = SyntaxTreeBuilder.BuildTree(tokens);

            Assert.AreEqual("1 - 2 - 3", node.ToString());
        }

        [Test]
        public void BuildTree_MixedAdditionAndSubtraction_ReturnsOperatorNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Addition),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Subtraction),
                new Token(TokenType.Number, "3"),
            };

            SyntaxNode node = SyntaxTreeBuilder.BuildTree(tokens);

            Assert.AreEqual("1 + 2 - 3", node.ToString());
        }

        [Test]
        public void BuildTree_MultipleMultiplications_ReturnsOperatorNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Multiply),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Multiply),
                new Token(TokenType.Number, "3"),
            };

            SyntaxNode node = SyntaxTreeBuilder.BuildTree(tokens);

            Assert.AreEqual("1 * 2 * 3", node.ToString());
        }

        [Test]
        public void BuildTree_MultipleDivisions_ReturnsOperatorNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Divide),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Divide),
                new Token(TokenType.Number, "3"),
            };

            SyntaxNode node = SyntaxTreeBuilder.BuildTree(tokens);

            Assert.AreEqual("1 / 2 / 3", node.ToString());
        }

        [Test]
        public void BuildTree_MultipleMultiplicationShortHands_ReturnsOperatorNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Identifier, "x"),
                new Token(TokenType.Multiply),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Identifier, "y"),
            };

            SyntaxNode node = SyntaxTreeBuilder.BuildTree(tokens);

            Assert.AreEqual("1 * x * 2 * y", node.ToString());
        }

        [Test]
        public void BuildTree_MixedDivisionMultiplicationAndShortHandMultiplication_ReturnsOperatorNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Identifier, "x"),
                new Token(TokenType.Divide),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Multiply),
                new Token(TokenType.Identifier, "y"),
            };

            SyntaxNode node = SyntaxTreeBuilder.BuildTree(tokens);

            Assert.AreEqual("1 * x / 2 * y", node.ToString());
        }

        [Test]
        public void BuildTree_MultipleExponentiation_ReturnsOperatorNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Exponent),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Exponent),
                new Token(TokenType.Number, "3"),
            };

            SyntaxNode node = SyntaxTreeBuilder.BuildTree(tokens);

            Assert.AreEqual("1 ^ 2 ^ 3", node.ToString());
        }

        [Test]
        public void BuildTree_ManyOperationsExpression_ReturnsOperatorNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "3"),
                new Token(TokenType.Identifier, "x"),
                new Token(TokenType.Exponent),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Addition),
                new Token(TokenType.Identifier, "x"),
                new Token(TokenType.Divide),
                new Token(TokenType.Identifier, "y"),
                new Token(TokenType.Multiply),
                new Token(TokenType.LeftParentheses),
                new Token(TokenType.Identifier, "f"),
                new Token(TokenType.LeftParentheses),
                new Token(TokenType.Identifier, "x"),
                new Token(TokenType.LeftParentheses),
                new Token(TokenType.Identifier, "y"),
                new Token(TokenType.RightParentheses),
                new Token(TokenType.RightParentheses),
                new Token(TokenType.Subtraction),
                new Token(TokenType.Identifier, "y"),
                new Token(TokenType.RightParentheses)
            };

            SyntaxNode node = SyntaxTreeBuilder.BuildTree(tokens);

            Assert.AreEqual("3 * x ^ 2 + x / y * (f(x(y)) - y)", node.ToString());
        }

        #endregion
    }
}
