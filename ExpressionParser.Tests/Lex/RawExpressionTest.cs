using System;
using ExpressionParser.Lex;
using NUnit.Framework;

namespace ExpressionParser.Tests.LexerTest
{
    [TestFixture]
    public class RawExpressionTest
    {
        [Test]
        public void CanRead_EmptyString_ReturnsFalse() 
        {
            RawExpression expression = new RawExpression("");

            Assert.False(expression.CanRead());
        }

        [Test]
        public void CanRead_NonEmptyString_ReturnsTrue()
        {
            RawExpression expression = new RawExpression("foo");

            Assert.True(expression.CanRead());
        }

        [Test]
        public void NextCharacter_EmptyString_ThrowsException()
        {
            RawExpression expression = new RawExpression("");

            Assert.Throws<IndexOutOfRangeException>(() => expression.Next());
        }

        [Test]
        public void NextCharacter_NonEmptyString_ReturnsCharacter()
        {
            RawExpression expression = new RawExpression("foo");

            Assert.AreEqual('f', expression.Next());
        }

        [Test]
        public void NextCharacter_NonEmptyStringAllChars_ReturnsCharactersThenThrows()
        {
            RawExpression expression = new RawExpression("foo");

            Assert.AreEqual('f', expression.Next());
            Assert.AreEqual('o', expression.Next());
            Assert.AreEqual('o', expression.Next());
            Assert.Throws<IndexOutOfRangeException>(() => expression.Next());
        }

        [Test]
        public void PeekCharacter_EmptyString_ThrowsException()
        {
            RawExpression expression = new RawExpression("");

            Assert.Throws<IndexOutOfRangeException>(() => expression.Peek());
        }

        [Test]
        public void PeekCharacter_NonEmptyString_ReturnsCharacter()
        {
            RawExpression expression = new RawExpression("foo");

            Assert.AreEqual('f', expression.Peek());
        }
    }
}
