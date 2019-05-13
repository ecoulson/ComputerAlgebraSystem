using System;
using System.Collections.Generic;
using ExpressionParser.Lex;
using NUnit.Framework;

namespace ExpressionParser.Tests.LexerTest
{
    [TestFixture]
    public class LexerTest
    {
        [Test]
        public void Lex_Number1_ReturnsNumberToken()
        {
            Tokens tokens = Lexer.Lex(new RawExpression("1"));

            Assert.True(tokens.CanRead());
            Assert.AreEqual(TokenType.Number, tokens[0].Type);
            Assert.AreEqual("1", tokens[0].Value);
        }

        [Test]
        public void Lex_Number2_ReturnsNumberToken()
        {
            Tokens tokens = Lexer.Lex(new RawExpression("2"));

            Assert.True(tokens.CanRead());
            Assert.AreEqual(TokenType.Number, tokens[0].Type);
            Assert.AreEqual("2", tokens[0].Value);
        }

        [Test]
        public void Lex_DecimalNumber_ReturnsNumberToken()
        {
            Tokens tokens = Lexer.Lex(new RawExpression("1.1"));

            Assert.True(tokens.CanRead());
            Assert.AreEqual(TokenType.Number, tokens[0].Type);
            Assert.AreEqual("1.1", tokens[0].Value);
        }

        [Test]
        public void Lex_DecimalTensNumber_ReturnsNumberToken()
        {
            Tokens tokens = Lexer.Lex(new RawExpression("11.1"));

            Assert.True(tokens.CanRead());
            Assert.AreEqual(TokenType.Number, tokens[0].Type);
            Assert.AreEqual("11.1", tokens[0].Value);
        }

        [Test]
        public void Lex_IllegallyFormattedDecimalNumber_ThrowsException()
        {
            Assert.Throws<FormatException>(
                delegate
                {
                    Lexer.Lex(new RawExpression("11..1"));
                }
            );
        }

        [Test]
        public void Lex_IdentifierX_ReturnsIdentifierToken()
        {
            Tokens tokens = Lexer.Lex(new RawExpression("x"));

            Assert.True(tokens.CanRead());
            Assert.AreEqual(TokenType.Identifier, tokens[0].Type);
            Assert.AreEqual("x", tokens[0].Value);
        }

        [Test]
        public void Lex_IdentifierY_ReturnsIdentifierToken()
        {
            Tokens tokens = Lexer.Lex(new RawExpression("y"));

            Assert.True(tokens.CanRead());
            Assert.AreEqual(TokenType.Identifier, tokens[0].Type);
            Assert.AreEqual("y", tokens[0].Value);
        }

        [Test]
        public void Lex_IdentifierFoo_ReturnsIdentifierToken()
        {
            Tokens tokens = Lexer.Lex(new RawExpression("foo"));
            
            Assert.True(tokens.CanRead());
            Assert.AreEqual(TokenType.Identifier, tokens[0].Type);
            Assert.AreEqual("foo", tokens[0].Value);
        }

        [Test]
        public void Lex_IdentifierBackSlash_ReturnsIdentifierToken()
        {
            Tokens tokens = Lexer.Lex(new RawExpression("\\asdf"));

            Assert.True(tokens.CanRead());
            Assert.AreEqual(TokenType.Identifier, tokens[0].Type);
            Assert.AreEqual("\\asdf", tokens[0].Value);
        }

        [Test]
        public void Lex_WhiteSpace_ReturnsWhiteSpaceToken()
        {
            Tokens tokens = Lexer.Lex(new RawExpression("\t"));

            Assert.False(tokens.CanRead());
        }

        [Test]
        public void Lex_DivideOperator_ReturnsDivideToken()
        {
            Tokens tokens = Lexer.Lex(new RawExpression("/"));

            Assert.True(tokens.CanRead());
            Assert.AreEqual(TokenType.Divide, tokens[0].Type);
            Assert.AreEqual(null, tokens[0].Value);
        }

        [Test]
        public void Lex_MultiplyOperator_ReturnsMultiplyToken()
        {
            Tokens tokens = Lexer.Lex(new RawExpression("*"));

            Assert.True(tokens.CanRead());
            Assert.AreEqual(TokenType.Multiply, tokens[0].Type);
            Assert.AreEqual(null, tokens[0].Value);
        }

        [Test]
        public void Lex_AdditionOperator_ReturnsAdditionToken()
        {
            Tokens tokens = Lexer.Lex(new RawExpression("+"));

            Assert.True(tokens.CanRead());
            Assert.AreEqual(TokenType.Addition, tokens[0].Type);
            Assert.AreEqual(null, tokens[0].Value);
        }

        [Test]
        public void Lex_SubtractionOperator_ReturnsSubstractionToken()
        {
            Tokens tokens = Lexer.Lex(new RawExpression("-"));

            Assert.True(tokens.CanRead());
            Assert.AreEqual(TokenType.Subtraction, tokens[0].Type);
            Assert.AreEqual(null, tokens[0].Value);
        }

        [Test]
        public void Lex_ExponentOperator_ReturnsExponentToken()
        {
            Tokens tokens = Lexer.Lex(new RawExpression("^"));

            Assert.True(tokens.CanRead());
            Assert.AreEqual(TokenType.Exponent, tokens[0].Type);
            Assert.AreEqual(null, tokens[0].Value);
        }

        [Test]
        public void Lex_LeftParentheses_ReturnsLeftParenthesesToken()
        {
            Tokens tokens = Lexer.Lex(new RawExpression("("));

            Assert.True(tokens.CanRead());
            Assert.AreEqual(TokenType.LeftParentheses, tokens[0].Type);
            Assert.AreEqual(null, tokens[0].Value);
        }

        [Test]
        public void Lex_RightParentheses_ReturnsRightParenthesesToken()
        {
            Tokens tokens = Lexer.Lex(new RawExpression(")"));

            Assert.True(tokens.CanRead());
            Assert.AreEqual(TokenType.RightParentheses, tokens[0].Type);
            Assert.AreEqual(null, tokens[0].Value);
        }

        [Test]
        public void Lex_AllTokens_ReturnsAllTokens()
        {
            Tokens tokens = Lexer.Lex(new RawExpression("1x*/+-()^"));

            Assert.True(tokens.CanRead());
            Assert.AreEqual(TokenType.Number, tokens[0].Type);
            Assert.AreEqual(TokenType.Identifier, tokens[1].Type);
            Assert.AreEqual(TokenType.Multiply, tokens[2].Type);
            Assert.AreEqual(TokenType.Divide, tokens[3].Type);
            Assert.AreEqual(TokenType.Addition, tokens[4].Type);
            Assert.AreEqual(TokenType.Subtraction, tokens[5].Type);
            Assert.AreEqual(TokenType.LeftParentheses, tokens[6].Type);
            Assert.AreEqual(TokenType.RightParentheses, tokens[7].Type);
            Assert.AreEqual(TokenType.Exponent, tokens[8].Type);

            Assert.AreEqual("1", tokens[0].Value);
            Assert.AreEqual("x", tokens[1].Value);
            Assert.AreEqual(null, tokens[2].Value);
            Assert.AreEqual(null, tokens[3].Value);
            Assert.AreEqual(null, tokens[4].Value);
            Assert.AreEqual(null, tokens[5].Value);
            Assert.AreEqual(null, tokens[6].Value);
            Assert.AreEqual(null, tokens[7].Value);
            Assert.AreEqual(null, tokens[8].Value);
        }

        [Test]
        public void Lex_SimpleExpression_ReturnsSimpleExpressionTokens()
        {
            Tokens tokens = Lexer.Lex(new RawExpression("1234.5x + 1.2345"));

            Assert.True(tokens.CanRead());

            Assert.AreEqual(TokenType.Number, tokens[0].Type);
            Assert.AreEqual(TokenType.Identifier, tokens[1].Type);
            Assert.AreEqual(TokenType.Addition, tokens[2].Type);
            Assert.AreEqual(TokenType.Number, tokens[3].Type);

            Assert.AreEqual("1234.5", tokens[0].Value);
            Assert.AreEqual("x", tokens[1].Value);
            Assert.AreEqual(null, tokens[2].Value);
            Assert.AreEqual("1.2345", tokens[3].Value);
        }
    }
}
