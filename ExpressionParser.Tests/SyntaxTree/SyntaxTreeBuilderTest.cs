using System;
using System.Collections.Generic;
using ExpressionParser.Lex;
using ExpressionParser.SyntaxTree;
using NUnit.Framework;

namespace ExpressionParser.Tests.SyntaxTree
{
    [TestFixture]
    public class SyntaxTreeBuilderTest
    {
        #region Expression Nonterminal Tests

        [Test]
        public void BuildTree_SimpleExpression_ReturnsIdentifierNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Identifier, "a"),
            };

            SyntaxNode node = SyntaxTreeBuilder.BuildTree(tokens);

            Assert.AreEqual("a", node.ToString());
        }

        [Test]
        public void BuildTree_Addition_ReturnsOperatorNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Addition),
                new Token(TokenType.Number, "2")
            };

            SyntaxNode node = SyntaxTreeBuilder.BuildTree(tokens);

            Assert.AreEqual("1 + 2", node.ToString());
        }

        [Test]
        public void BuildTree_Subtraction_ReturnsOperatorNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Subtraction),
                new Token(TokenType.Number, "2")
            };

            SyntaxNode node = SyntaxTreeBuilder.BuildTree(tokens);

            Assert.AreEqual("1 - 2", node.ToString());
        }

        #endregion

        #region Term Nonterminal Tests

        [Test]
        public void BuildTree_SingleFactor_ReturnsNumberNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "1")
            };

            SyntaxNode node = SyntaxTreeBuilder.BuildTree(tokens);

            Assert.AreEqual("1", node.ToString());
        }

        [Test]
        public void BuildTree_Multiplication_ReturnsOperatorNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Multiply),
                new Token(TokenType.Number, "2")
            };

            SyntaxNode node = SyntaxTreeBuilder.BuildTree(tokens);;

            Assert.AreEqual("1 * 2", node.ToString());
        }

        [Test]
        public void BuildTree_ShorthandMultiplicationWithVariables_ReturnsOperatorNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Identifier, "x")
            };

            SyntaxNode node = SyntaxTreeBuilder.BuildTree(tokens);

            Assert.AreEqual("2 * x", node.ToString());
        }

        [Test]
        public void BuildTree_ShorthandMultiplicationParenthesized_ReturnsOperatorNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "2"),
                new Token(TokenType.LeftParentheses),
                new Token(TokenType.Identifier, "x"),
                new Token(TokenType.RightParentheses)
            };

            SyntaxNode node = SyntaxTreeBuilder.BuildTree(tokens);

            Assert.AreEqual("2 * (x)", node.ToString());
        }

        [Test]
        public void BuildTree_Division_ReturnsOperatorNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Divide),
                new Token(TokenType.Number, "2")
            };

            SyntaxNode node = SyntaxTreeBuilder.BuildTree(tokens);

            Assert.AreEqual("1 / 2", node.ToString());
        }

        #endregion

        #region Factor Nonterminal Tests

        [Test]
        public void BuildTree_NoExponentiation_ReturnsNumberNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "1"),
            };

            SyntaxNode node = SyntaxTreeBuilder.BuildTree(tokens);

            Assert.AreEqual("1", node.ToString());
        }

        [Test]
        public void BuildTree_Exponentiation_ReturnsOperatorNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Exponent),
                new Token(TokenType.Number, "2")
            };

            SyntaxNode node = SyntaxTreeBuilder.BuildTree(tokens);

            Assert.AreEqual("1 ^ 2", node.ToString());
        }

        #endregion

        #region Formal Nonterminal Tests

        [Test]
        public void BuildTree_IllegalTokenInFormal_ThrowsException()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Addition)
            };

            UnexpectedTokenException exception = Assert.Throws<UnexpectedTokenException>(
                () => SyntaxTreeBuilder.BuildTree(tokens)
            );

            Assert.AreEqual("Expected one of these 'LeftParentheses, Identifier, Number' tokens, but got 'Addition' instead.", exception.Message);
        }

        #endregion

        #region Parenthesized Expression

        [Test]
        public void BuildTree_IllegallyParenthesizedExpression_ThrowsException()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.LeftParentheses),
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Number, "1")
            };

            UnexpectedTokenException exception = Assert.Throws<UnexpectedTokenException>(
                () => SyntaxTreeBuilder.BuildTree(tokens)
            );

            Assert.AreEqual("Expected 'RightParentheses' token, but got 'Number' instead.", exception.Message);
        }

        [Test]
        public void BuildTree_IllegalEndOfStream_ThrowsException()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.LeftParentheses),
                new Token(TokenType.Number, "1"),
            };

            EndOfTokenStreamException exception = Assert.Throws<EndOfTokenStreamException>(
                () => SyntaxTreeBuilder.BuildTree(tokens)
            );

            Assert.AreEqual("Unexpected end of token stream", exception.Message);
        }

        [Test]
        public void BuildTree_ParenthesizedExpression_ThrowsException()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.LeftParentheses),
                new Token(TokenType.Number, "1"),
                new Token(TokenType.RightParentheses)
            };

            SyntaxNode node = SyntaxTreeBuilder.BuildTree(tokens);

            Assert.AreEqual("(1)", node.ToString());
        }

        #endregion

        #region Number Terminal Tests

        [Test]
        public void BuildTree_NumberToken_ReturnsNumberNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "1")
            };

            SyntaxNode node = SyntaxTreeBuilder.BuildTree(tokens);

            Assert.AreEqual("1", node.ToString());
        }

        [Test]
        public void BuildTree_NegativeNumber_ReturnsOperatorNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Subtraction),
                new Token(TokenType.Number, "2")
            };

            SyntaxNode node = SyntaxTreeBuilder.BuildTree(tokens);

            Assert.AreEqual("-1 * 2", node.ToString());
        }

        #endregion

        #region Identifier Terminal Tests

        [Test]
        public void BuildTree_IdentifierToken_ReturnsIdentifierNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Identifier, "x")
            };

            SyntaxNode node = SyntaxTreeBuilder.BuildTree(tokens);

            Assert.AreEqual("x", node.ToString());
        }

        #endregion

        #region Function Nonterminal Tests

        [Test]
        public void BuildTree_ValidTokens_ReturnsFunctionNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Identifier, "sin"),
                new Token(TokenType.LeftParentheses),
                new Token(TokenType.Number, "1"),
                new Token(TokenType.RightParentheses),
            };

            SyntaxNode node = SyntaxTreeBuilder.BuildTree(tokens);

            Assert.AreEqual("sin(1)", node.ToString());
        }

        [Test]
        public void BuildTree_UnexpectedTokenAtRightParen_ThrowsException()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Identifier, "sin"),
                new Token(TokenType.LeftParentheses),
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Number, "1"),
            };

            UnexpectedTokenException exception = Assert.Throws<UnexpectedTokenException>(
                () => SyntaxTreeBuilder.BuildTree(tokens)
            );

            Assert.AreEqual($"Expected 'RightParentheses' token, but got 'Number' instead.", exception.Message);
        }

        #endregion
    }
}
