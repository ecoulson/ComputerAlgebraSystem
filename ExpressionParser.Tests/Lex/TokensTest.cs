using System;
using System.Collections.Generic;
using ExpressionParser.Lex;
using ExpressionParser.SyntaxTree;
using NUnit.Framework;

namespace ExpressionParser.Tests.LexerTest
{
    [TestFixture]
    public class TokensTest
    {
        [Test]
        public void CanRead_NoTokens_ReturnsFalse()
        {
            Assert.False(new Tokens().CanRead());
        }

        [Test]
        public void CanRead_Tokens_ReturnsFalse()
        {
            Tokens tokens = new Tokens(new List<Token>
            {
                new Token(TokenType.Addition)
            });

            tokens.Add(new Token(TokenType.Addition));

            Assert.True(tokens.CanRead());
        }

        [Test]
        public void Add_AddsToken_AddsTokenToList()
        {
            Tokens tokens = new Tokens();

            tokens.Add(new Token(TokenType.Addition));

            Assert.True(tokens.CanRead());
        }

        [Test]
        public void Next_NoTokens_ThrowsException()
        {
            Tokens tokens = new Tokens();

            Assert.Throws<IndexOutOfRangeException>(() => tokens.Next());
        }

        [Test]
        public void Next_HasTokens_ReturnsToken()
        {
            Tokens tokens = new Tokens(new List<Token>
            {
                new Token(TokenType.Addition)
            });

            Token token = tokens.Next();

            Assert.AreEqual(TokenType.Addition, token.Type);
        }

        [Test]
        public void Next_HasTokensAndReadsOutOfBounds_ReturnsTokenThenThrows()
        {
            Tokens tokens = new Tokens(new List<Token>
            {
                new Token(TokenType.Addition)
            });

            Token token = tokens.Next();

            Assert.AreEqual(TokenType.Addition, token.Type);
            Assert.Throws<IndexOutOfRangeException>(() => tokens.Next());
        }

        [Test]
        public void Peek_NoTokens_ThrowsException()
        {
            Tokens tokens = new Tokens();

            Assert.Throws<IndexOutOfRangeException>(() => tokens.Peek());
        }

        [Test]
        public void Peek_HasTokens_ReturnsToken()
        {
            Tokens tokens = new Tokens(new List<Token> {
                new Token(TokenType.Addition)
            });

            Token token = tokens.Peek();

            Assert.AreEqual(TokenType.Addition, token.Type);
        }

        [Test]
        public void AssertCanRead_NoTokens_ThrowsException()
        {
            Assert.Throws<EndOfTokenStreamException>(() => new Tokens().AssertCanRead());
        }

        [Test]
        public void IsNextTypeOf_AdditionToken_ReturnsTrue()
        {
            Tokens tokens = new Tokens(new List<Token> {
                new Token(TokenType.Addition)
            });

            Assert.True(tokens.IsNextTypeOf(TokenType.Addition));
        }

        [Test]
        public void IsPeekTypeOf_AdditionToken_ReturnsTrue()
        {
            Tokens tokens = new Tokens(new List<Token> {
                new Token(TokenType.Addition)
            });

            Assert.True(tokens.IsPeekTypeOf(TokenType.Addition));
        }

        [Test]
        public void AssertNextIsTypeOf_AdditionToken_ThrowsException()
        {
            Tokens tokens = new Tokens(new List<Token> {
                new Token(TokenType.Addition)
            });

            Assert.Throws<UnexpectedTokenException>(() => tokens.AssertNextIsTypeOf(TokenType.Subtraction));
        }

        [Test]
        public void AssertPeekIsTypeOf_AdditionToken_ThrowsException()
        {
            Tokens tokens = new Tokens(new List<Token> {
                new Token(TokenType.Addition)
            });

            Assert.Throws<UnexpectedTokenException>(() => tokens.AssertPeekIsTypeOf(TokenType.Subtraction));
        }

        [Test]
        public void IsNextTypeOfOne_AdditionToken_ReturnsTrue()
        {
            Tokens tokens = new Tokens(new List<Token> {
                new Token(TokenType.Addition)
            });

            Assert.True(tokens.IsNextTypeOfOne(new List<TokenType>
            {
                TokenType.Addition
            }));
        }

        [Test]
        public void IsPeekTypeOfOne_AdditionToken_ReturnsTrue()
        {
            Tokens tokens = new Tokens(new List<Token> {
                new Token(TokenType.Addition)
            });

            Assert.True(tokens.IsPeekTypeOfOne(new List<TokenType>
            {
                TokenType.Addition
            }));
        }

        [Test]
        public void AssertNextIsTypeOfOne_AdditionToken_ThrowsException()
        {
            Tokens tokens = new Tokens(new List<Token> {
                new Token(TokenType.Addition)
            });

            Assert.Throws<UnexpectedTokenException>(() => tokens.AssertNextIsTypeOfOne(
                new List<TokenType> { TokenType.Subtraction }
            ));
        }

        [Test]
        public void AssertPeekIsTypeOfOne_AdditionToken_ThrowsException()
        {
            Tokens tokens = new Tokens(new List<Token> {
                new Token(TokenType.Addition)
            });

            Assert.Throws<UnexpectedTokenException>(() => tokens.AssertPeekIsTypeOfOne(
                new List<TokenType> { TokenType.Subtraction }
            ));
        }
    }
}
