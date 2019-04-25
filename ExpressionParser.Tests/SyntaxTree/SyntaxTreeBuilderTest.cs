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
        public void BuildTree_EmptyExpression_ReturnsEmptyExpressionTree()
        {
            List<Token> tokens = new List<Token>();

            EndOfTokenStreamException exception = Assert.Throws<EndOfTokenStreamException>(
                () => SyntaxTreeBuilder.BuildTree(tokens)
            );

            Assert.AreEqual("Unexpected end of token stream", exception.Message);
        }

        #endregion

        #region Factor Nonterminal Tests

        [Test]
        public void BuildTree_SingleFactor_ReturnsNumberNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "1")
            };

            NumberNode node = (NumberNode)SyntaxTreeBuilder.BuildTree(tokens);

            Assert.NotNull(node);
            Assert.AreEqual(SyntaxNodeType.Number, node.Type);
            Assert.AreEqual(1, node.Value);
        }

        [Test]
        public void BuildTree_Multiplication_ReturnsOperatorNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Multiply, ""),
                new Token(TokenType.Number, "2")
            };

            OperatorNode node = (OperatorNode)SyntaxTreeBuilder.BuildTree(tokens);
            NumberNode lhsNode = (NumberNode)node.Left;
            NumberNode rhsNode = (NumberNode)node.Right;

            Assert.NotNull(node);
            Assert.NotNull(lhsNode);
            Assert.NotNull(rhsNode);

            Assert.AreEqual(SyntaxNodeType.Operator, node.Type);
            Assert.AreEqual(SyntaxNodeType.Number, lhsNode.Type);
            Assert.AreEqual(SyntaxNodeType.Number, rhsNode.Type);

            Assert.AreEqual(Operator.Multiplication, node.Operator);
            Assert.AreEqual(1, lhsNode.Value);
            Assert.AreEqual(2, rhsNode.Value);
        }

        [Test]
        public void BuildTree_ShorthandMultiplicationWithVariables_ReturnsMultiplicationTree()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Identifier, "x")
            };

            OperatorNode node = (OperatorNode)SyntaxTreeBuilder.BuildTree(tokens);
            NumberNode lhsNode = (NumberNode)node.Left;
            IdentifierNode rhsNode = (IdentifierNode)node.Right;

            Assert.NotNull(node);
            Assert.NotNull(lhsNode);
            Assert.NotNull(rhsNode);

            Assert.AreEqual(SyntaxNodeType.Operator, node.Type);
            Assert.AreEqual(SyntaxNodeType.Number, lhsNode.Type);
            Assert.AreEqual(SyntaxNodeType.Identifier, rhsNode.Type);

            Assert.AreEqual(Operator.Multiplication, node.Operator);
            Assert.AreEqual(2, lhsNode.Value);
            Assert.AreEqual("x", rhsNode.Value);
        }

        [Test]
        public void BuildTree_Division_ReturnsOperatorNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Divide, ""),
                new Token(TokenType.Number, "2")
            };

            OperatorNode node = (OperatorNode)SyntaxTreeBuilder.BuildTree(tokens);
            NumberNode lhsNode = (NumberNode)node.Left;
            NumberNode rhsNode = (NumberNode)node.Right;

            Assert.NotNull(node);
            Assert.NotNull(lhsNode);
            Assert.NotNull(rhsNode);

            Assert.AreEqual(SyntaxNodeType.Operator, node.Type);
            Assert.AreEqual(SyntaxNodeType.Number, lhsNode.Type);
            Assert.AreEqual(SyntaxNodeType.Number, rhsNode.Type);

            Assert.AreEqual(Operator.Division, node.Operator);
            Assert.AreEqual(1, lhsNode.Value);
            Assert.AreEqual(2, rhsNode.Value);
        }

        #endregion

        #region Formal Nonterminal Tests

        [Test]
        public void BuildTree_IllegalTokenInFormal_ThrowsException()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Addition, "")
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
                new Token(TokenType.LeftParentheses, ""),
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Addition, "")
            };

            UnexpectedTokenException exception = Assert.Throws<UnexpectedTokenException>(
                () => SyntaxTreeBuilder.BuildTree(tokens)
            );

            Assert.AreEqual("Expected 'RightParentheses' token, but got 'Addition' instead.", exception.Message);
        }

        [Test]
        public void BuildTree_IllegalEndOfStream_ThrowsException()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.LeftParentheses, ""),
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
                new Token(TokenType.LeftParentheses, ""),
                new Token(TokenType.Number, "1"),
                new Token(TokenType.RightParentheses, "")
            };

            NumberNode node = (NumberNode)SyntaxTreeBuilder.BuildTree(tokens);

            Assert.NotNull(node);
            Assert.AreEqual(SyntaxNodeType.Number, node.Type);
            Assert.AreEqual(1, node.Value);
        }

        #endregion

        #region Number Terminal Tests

        [Test]
        public void BuildTree_NumberToken_ReturnsANumberNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "1")
            };

            NumberNode node = (NumberNode)SyntaxTreeBuilder.BuildTree(tokens);

            Assert.NotNull(node);
            Assert.AreEqual(SyntaxNodeType.Number, node.Type);
            Assert.AreEqual(1, node.Value);
        }

        #endregion

        #region Identifier Terminal Tests

        [Test]
        public void BuildTree_IdentifierToken_ReturnsAnIdentifierNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Identifier, "x")
            };

            IdentifierNode node = (IdentifierNode)SyntaxTreeBuilder.BuildTree(tokens);

            Assert.NotNull(node);
            Assert.AreEqual(SyntaxNodeType.Identifier, node.Type);
            Assert.AreEqual("x", node.Value);
        }

        #endregion

        #region Function Nonterminal Tests

        [Test]
        public void BuildTree_ValidTokens_ReturnsFunctionExpression()
        {
            List<Token> tokens = new List<Token>
        {
            new Token(TokenType.Identifier, "sin"),
            new Token(TokenType.LeftParentheses, ""),
            new Token(TokenType.Number, "1"),
            new Token(TokenType.RightParentheses, ""),
        };

            SyntaxNode functionNode = SyntaxTreeBuilder.BuildTree(tokens);
            IdentifierNode functionNameNode = (IdentifierNode)functionNode.Left;
            NumberNode functionCallValue = (NumberNode)functionNode.Right;

            Assert.AreEqual(SyntaxNodeType.Function, functionNode.Type);
            Assert.NotNull(functionNode.Left);
            Assert.AreEqual(SyntaxNodeType.Identifier, functionNameNode.Type);
            Assert.AreEqual("sin", functionNameNode.Value);
            Assert.NotNull(functionNode.Right);
            Assert.AreEqual(SyntaxNodeType.Number, functionCallValue.Type);
            Assert.AreEqual(1, functionCallValue.Value);
        }

        [Test]
        public void BuildTree_UnexpectedTokenAtRightParen_ThrowsException()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Identifier, "sin"),
                new Token(TokenType.LeftParentheses, ""),
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Addition, ""),
            };

            UnexpectedTokenException exception = Assert.Throws<UnexpectedTokenException>(
                () => SyntaxTreeBuilder.BuildTree(tokens)
            );

            Assert.AreEqual($"Expected 'RightParentheses' token, but got 'Addition' instead.", exception.Message);
        }

        #endregion
    }
}
