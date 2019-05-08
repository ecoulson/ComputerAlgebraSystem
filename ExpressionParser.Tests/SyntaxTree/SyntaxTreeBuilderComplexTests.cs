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

            OperatorNode op1 = (OperatorNode)SyntaxTreeBuilder.BuildTree(tokens);
            NumberNode n3 = (NumberNode)op1.Right;
            OperatorNode op2 = (OperatorNode)op1.Left;
            NumberNode n2 = (NumberNode)op2.Right;
            NumberNode n1 = (NumberNode)op2.Left;

            Assert.NotNull(op1);
            Assert.NotNull(op2);
            Assert.NotNull(n1);
            Assert.NotNull(n2);
            Assert.NotNull(n3);

            Assert.AreEqual(SyntaxNodeType.Operator, op1.Type);
            Assert.AreEqual(SyntaxNodeType.Operator, op2.Type);
            Assert.AreEqual(SyntaxNodeType.Number, n1.Type);
            Assert.AreEqual(SyntaxNodeType.Number, n2.Type);
            Assert.AreEqual(SyntaxNodeType.Number, n3.Type);

            Assert.AreEqual(Operator.Addition, op1.Operator);
            Assert.AreEqual(Operator.Addition, op2.Operator);
            Assert.AreEqual(1, n1.Value);
            Assert.AreEqual(2, n2.Value);
            Assert.AreEqual(3, n3.Value);
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

            OperatorNode op1 = (OperatorNode)SyntaxTreeBuilder.BuildTree(tokens);
            NumberNode n3 = (NumberNode)op1.Right;
            OperatorNode op2 = (OperatorNode)op1.Left;
            NumberNode n2 = (NumberNode)op2.Right;
            NumberNode n1 = (NumberNode)op2.Left;

            Assert.NotNull(op1);
            Assert.NotNull(op2);
            Assert.NotNull(n1);
            Assert.NotNull(n2);
            Assert.NotNull(n3);

            Assert.AreEqual(SyntaxNodeType.Operator, op1.Type);
            Assert.AreEqual(SyntaxNodeType.Operator, op2.Type);
            Assert.AreEqual(SyntaxNodeType.Number, n1.Type);
            Assert.AreEqual(SyntaxNodeType.Number, n2.Type);
            Assert.AreEqual(SyntaxNodeType.Number, n3.Type);

            Assert.AreEqual(Operator.Subtraction, op1.Operator);
            Assert.AreEqual(Operator.Subtraction, op2.Operator);
            Assert.AreEqual(1, n1.Value);
            Assert.AreEqual(2, n2.Value);
            Assert.AreEqual(3, n3.Value);
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

            OperatorNode op1 = (OperatorNode)SyntaxTreeBuilder.BuildTree(tokens);
            NumberNode n3 = (NumberNode)op1.Right;
            OperatorNode op2 = (OperatorNode)op1.Left;
            NumberNode n2 = (NumberNode)op2.Right;
            NumberNode n1 = (NumberNode)op2.Left;

            Assert.NotNull(op1);
            Assert.NotNull(op2);
            Assert.NotNull(n1);
            Assert.NotNull(n2);
            Assert.NotNull(n3);

            Assert.AreEqual(SyntaxNodeType.Operator, op1.Type);
            Assert.AreEqual(SyntaxNodeType.Operator, op2.Type);
            Assert.AreEqual(SyntaxNodeType.Number, n1.Type);
            Assert.AreEqual(SyntaxNodeType.Number, n2.Type);
            Assert.AreEqual(SyntaxNodeType.Number, n3.Type);

            Assert.AreEqual(Operator.Subtraction, op1.Operator);
            Assert.AreEqual(Operator.Addition, op2.Operator);
            Assert.AreEqual(1, n1.Value);
            Assert.AreEqual(2, n2.Value);
            Assert.AreEqual(3, n3.Value);
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

            OperatorNode op1 = (OperatorNode)SyntaxTreeBuilder.BuildTree(tokens);
            NumberNode n3 = (NumberNode)op1.Right;
            OperatorNode op2 = (OperatorNode)op1.Left;
            NumberNode n2 = (NumberNode)op2.Right;
            NumberNode n1 = (NumberNode)op2.Left;

            Assert.NotNull(op1);
            Assert.NotNull(op2);
            Assert.NotNull(n1);
            Assert.NotNull(n2);
            Assert.NotNull(n3);

            Assert.AreEqual(SyntaxNodeType.Operator, op1.Type);
            Assert.AreEqual(SyntaxNodeType.Operator, op2.Type);
            Assert.AreEqual(SyntaxNodeType.Number, n1.Type);
            Assert.AreEqual(SyntaxNodeType.Number, n2.Type);
            Assert.AreEqual(SyntaxNodeType.Number, n3.Type);

            Assert.AreEqual(Operator.Multiplication, op1.Operator);
            Assert.AreEqual(Operator.Multiplication, op2.Operator);
            Assert.AreEqual(1, n1.Value);
            Assert.AreEqual(2, n2.Value);
            Assert.AreEqual(3, n3.Value);
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

            OperatorNode op1 = (OperatorNode)SyntaxTreeBuilder.BuildTree(tokens);
            NumberNode n3 = (NumberNode)op1.Right;
            OperatorNode op2 = (OperatorNode)op1.Left;
            NumberNode n2 = (NumberNode)op2.Right;
            NumberNode n1 = (NumberNode)op2.Left;

            Assert.NotNull(op1);
            Assert.NotNull(op2);
            Assert.NotNull(n1);
            Assert.NotNull(n2);
            Assert.NotNull(n3);

            Assert.AreEqual(SyntaxNodeType.Operator, op1.Type);
            Assert.AreEqual(SyntaxNodeType.Operator, op2.Type);
            Assert.AreEqual(SyntaxNodeType.Number, n1.Type);
            Assert.AreEqual(SyntaxNodeType.Number, n2.Type);
            Assert.AreEqual(SyntaxNodeType.Number, n3.Type);

            Assert.AreEqual(Operator.Division, op1.Operator);
            Assert.AreEqual(Operator.Division, op2.Operator);
            Assert.AreEqual(1, n1.Value);
            Assert.AreEqual(2, n2.Value);
            Assert.AreEqual(3, n3.Value);
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

            OperatorNode op1 = (OperatorNode)SyntaxTreeBuilder.BuildTree(tokens);
            IdentifierNode i2 = (IdentifierNode)op1.Right;
            OperatorNode op2 = (OperatorNode)op1.Left;
            NumberNode n2 = (NumberNode)op2.Right;
            OperatorNode op3 = (OperatorNode)op2.Left;
            IdentifierNode i1 = (IdentifierNode)op3.Right;
            NumberNode n1 = (NumberNode)op3.Left;

            Assert.NotNull(op1);
            Assert.NotNull(op2);
            Assert.NotNull(op3);
            Assert.NotNull(n1);
            Assert.NotNull(n2);
            Assert.NotNull(i1);
            Assert.NotNull(i2);

            Assert.AreEqual(SyntaxNodeType.Operator, op1.Type);
            Assert.AreEqual(SyntaxNodeType.Operator, op2.Type);
            Assert.AreEqual(SyntaxNodeType.Operator, op3.Type);
            Assert.AreEqual(SyntaxNodeType.Number, n1.Type);
            Assert.AreEqual(SyntaxNodeType.Number, n2.Type);
            Assert.AreEqual(SyntaxNodeType.Identifier, i1.Type);
            Assert.AreEqual(SyntaxNodeType.Identifier, i2.Type);

            Assert.AreEqual(Operator.Multiplication, op1.Operator);
            Assert.AreEqual(Operator.Multiplication, op2.Operator);
            Assert.AreEqual(Operator.Multiplication, op3.Operator);
            Assert.AreEqual(1, n1.Value);
            Assert.AreEqual(2, n2.Value);
            Assert.AreEqual("x", i1.Value);
            Assert.AreEqual("y", i2.Value);
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

            OperatorNode op1 = (OperatorNode)SyntaxTreeBuilder.BuildTree(tokens);
            IdentifierNode i2 = (IdentifierNode)op1.Right;
            OperatorNode op2 = (OperatorNode)op1.Left;
            NumberNode n2 = (NumberNode)op2.Right;
            OperatorNode op3 = (OperatorNode)op2.Left;
            IdentifierNode i1 = (IdentifierNode)op3.Right;
            NumberNode n1 = (NumberNode)op3.Left;

            Assert.NotNull(op1);
            Assert.NotNull(op2);
            Assert.NotNull(op3);
            Assert.NotNull(n1);
            Assert.NotNull(n2);
            Assert.NotNull(i1);
            Assert.NotNull(i2);

            Assert.AreEqual(SyntaxNodeType.Operator, op1.Type);
            Assert.AreEqual(SyntaxNodeType.Operator, op2.Type);
            Assert.AreEqual(SyntaxNodeType.Operator, op3.Type);
            Assert.AreEqual(SyntaxNodeType.Number, n1.Type);
            Assert.AreEqual(SyntaxNodeType.Number, n2.Type);
            Assert.AreEqual(SyntaxNodeType.Identifier, i1.Type);
            Assert.AreEqual(SyntaxNodeType.Identifier, i2.Type);

            Assert.AreEqual(Operator.Multiplication, op1.Operator);
            Assert.AreEqual(Operator.Division, op2.Operator);
            Assert.AreEqual(Operator.Multiplication, op3.Operator);
            Assert.AreEqual(1, n1.Value);
            Assert.AreEqual(2, n2.Value);
            Assert.AreEqual("x", i1.Value);
            Assert.AreEqual("y", i2.Value);
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

            OperatorNode op1 = (OperatorNode)SyntaxTreeBuilder.BuildTree(tokens);
            NumberNode n3 = (NumberNode)op1.Right;
            OperatorNode op2 = (OperatorNode)op1.Left;
            NumberNode n2 = (NumberNode)op2.Right;
            NumberNode n1 = (NumberNode)op2.Left;

            Assert.NotNull(op1);
            Assert.NotNull(op2);
            Assert.NotNull(n1);
            Assert.NotNull(n2);
            Assert.NotNull(n3);

            Assert.AreEqual(SyntaxNodeType.Operator, op1.Type);
            Assert.AreEqual(SyntaxNodeType.Operator, op2.Type);
            Assert.AreEqual(SyntaxNodeType.Number, n1.Type);
            Assert.AreEqual(SyntaxNodeType.Number, n2.Type);
            Assert.AreEqual(SyntaxNodeType.Number, n3.Type);

            Assert.AreEqual(Operator.Exponentiation, op1.Operator);
            Assert.AreEqual(Operator.Exponentiation, op2.Operator);
            Assert.AreEqual(1, n1.Value);
            Assert.AreEqual(2, n2.Value);
            Assert.AreEqual(3, n3.Value);
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

            OperatorNode op1 = (OperatorNode)SyntaxTreeBuilder.BuildTree(tokens);
            OperatorNode op2 = (OperatorNode)op1.Left;
            OperatorNode op3 = (OperatorNode)op1.Right;
            NumberNode n1 = (NumberNode)op2.Left;
            OperatorNode op4 = (OperatorNode)op2.Right;
            IdentifierNode i1 = (IdentifierNode)op4.Left;
            NumberNode n2 = (NumberNode)op4.Right;
            OperatorNode op5 = (OperatorNode)op3.Left;
            ParenthesesNode p1 = (ParenthesesNode)op3.Right;
            OperatorNode op6 = (OperatorNode)p1.Left;
            IdentifierNode i2 = (IdentifierNode)op5.Left;
            IdentifierNode i3 = (IdentifierNode)op5.Right;
            FunctionOrDistributionNode f1 = (FunctionOrDistributionNode)op6.Left;
            IdentifierNode i4 = (IdentifierNode)op6.Right;
            IdentifierNode i5 = (IdentifierNode)f1.Left;
            FunctionOrDistributionNode f2 = (FunctionOrDistributionNode)f1.Right;
            IdentifierNode i6 = (IdentifierNode)f2.Left;
            IdentifierNode i7 = (IdentifierNode)f2.Right;

            Assert.NotNull(op1);
            Assert.NotNull(op2);
            Assert.NotNull(op3);
            Assert.NotNull(op4);
            Assert.NotNull(op5);
            Assert.NotNull(op6);
            Assert.NotNull(n1);
            Assert.NotNull(n2);
            Assert.NotNull(i1);
            Assert.NotNull(i2);
            Assert.NotNull(i3);
            Assert.NotNull(i4);
            Assert.NotNull(i5);
            Assert.NotNull(i6);
            Assert.NotNull(i7);
            Assert.NotNull(f1);
            Assert.NotNull(f2);
            Assert.NotNull(p1);

            Assert.AreEqual(SyntaxNodeType.Operator, op1.Type);
            Assert.AreEqual(SyntaxNodeType.Operator, op2.Type);
            Assert.AreEqual(SyntaxNodeType.Operator, op3.Type);
            Assert.AreEqual(SyntaxNodeType.Operator, op4.Type);
            Assert.AreEqual(SyntaxNodeType.Operator, op5.Type);
            Assert.AreEqual(SyntaxNodeType.Operator, op6.Type);
            Assert.AreEqual(SyntaxNodeType.Identifier, i1.Type);
            Assert.AreEqual(SyntaxNodeType.Identifier, i2.Type);
            Assert.AreEqual(SyntaxNodeType.Identifier, i3.Type);
            Assert.AreEqual(SyntaxNodeType.Identifier, i4.Type);
            Assert.AreEqual(SyntaxNodeType.Identifier, i5.Type);
            Assert.AreEqual(SyntaxNodeType.Identifier, i6.Type);
            Assert.AreEqual(SyntaxNodeType.Identifier, i7.Type);
            Assert.AreEqual(SyntaxNodeType.Number, n1.Type);
            Assert.AreEqual(SyntaxNodeType.Number, n2.Type);
            Assert.AreEqual(SyntaxNodeType.AmbigiousFunctionOrShortHandMultiplication, f1.Type);
            Assert.AreEqual(SyntaxNodeType.Parentheses, p1.Type);

            Assert.AreEqual(Operator.Addition, op1.Operator);
            Assert.AreEqual(Operator.Multiplication, op2.Operator);
            Assert.AreEqual(Operator.Multiplication, op3.Operator);
            Assert.AreEqual(Operator.Exponentiation, op4.Operator);
            Assert.AreEqual(Operator.Division, op5.Operator);
            Assert.AreEqual(Operator.Subtraction, op6.Operator);
            Assert.AreEqual("x", i1.Value);
            Assert.AreEqual("x", i2.Value);
            Assert.AreEqual("y", i3.Value);
            Assert.AreEqual("y", i4.Value);
            Assert.AreEqual("f", i5.Value);
            Assert.AreEqual("x", i6.Value);
            Assert.AreEqual("y", i7.Value);
            Assert.AreEqual(3, n1.Value);
            Assert.AreEqual(2, n2.Value);
        }

        #endregion
    }
}
