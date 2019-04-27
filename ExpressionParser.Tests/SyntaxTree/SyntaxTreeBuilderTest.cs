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

            IdentifierNode node = (IdentifierNode)SyntaxTreeBuilder.BuildTree(tokens);

            Assert.NotNull(node);
            Assert.AreEqual(SyntaxNodeType.Identifier, node.Type);
            Assert.AreEqual("a", node.Value);
        }

        [Test]
        public void BuildTree_Addition_ReturnsOperatorNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Addition, ""),
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

            Assert.AreEqual(Operator.Addition, node.Operator);
            Assert.AreEqual(1, lhsNode.Value);
            Assert.AreEqual(2, rhsNode.Value);
        }

        [Test]
        public void BuildTree_Subtraction_ReturnsOperatorNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Subtraction, ""),
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

            Assert.AreEqual(Operator.Subtraction, node.Operator);
            Assert.AreEqual(1, lhsNode.Value);
            Assert.AreEqual(2, rhsNode.Value);
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
        public void BuildTree_ShorthandMultiplicationWithVariables_ReturnsOperatorNode()
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
        public void BuildTree_ShorthandMultiplicationParenthesized_ReturnsOperatorNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "2"),
                new Token(TokenType.LeftParentheses, ""),
                new Token(TokenType.Identifier, "x"),
                new Token(TokenType.RightParentheses, "")
            };

            OperatorNode node = (OperatorNode)SyntaxTreeBuilder.BuildTree(tokens);
            NumberNode n = (NumberNode)node.Left;
            IdentifierNode i = (IdentifierNode)node.Right;

            Assert.NotNull(node);
            Assert.NotNull(n);
            Assert.NotNull(i);

            Assert.AreEqual(SyntaxNodeType.Operator, node.Type);
            Assert.AreEqual(SyntaxNodeType.Number, n.Type);
            Assert.AreEqual(SyntaxNodeType.Identifier, i.Type);

            Assert.AreEqual(Operator.Multiplication, node.Operator);
            Assert.AreEqual(2, n.Value);
            Assert.AreEqual("x", i.Value);
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

        #region Factor Nonterminal Tests

        [Test]
        public void BuildTree_NoExponentiation_ReturnsNumberNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "1"),
            };

            NumberNode node = (NumberNode)SyntaxTreeBuilder.BuildTree(tokens);

            Assert.NotNull(node);
            Assert.AreEqual(SyntaxNodeType.Number, node.Type);
            Assert.AreEqual(1, node.Value);
        }

        [Test]
        public void BuildTree_Exponentiation_ReturnsOperatorNode()
        {
            List<Token> tokens = new List<Token>
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Exponent, ""),
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

            Assert.AreEqual(Operator.Exponentiation, node.Operator);
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
        public void BuildTree_NumberToken_ReturnsNumberNode()
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
        public void BuildTree_IdentifierToken_ReturnsIdentifierNode()
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
        public void BuildTree_ValidTokens_ReturnsFunctionNode()
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
