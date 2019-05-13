using System;
using System.Collections.Generic;
using ExpressionParser.SyntaxTree;

namespace ExpressionParser.Lex
{
    public class Tokens
    {
        private int cursor;
        private List<Token> tokens;

        public Tokens()
        {
            cursor = 0;
            tokens = new List<Token>();
        }

        public Tokens(List<Token> tokens)
        {
            cursor = 0;
            this.tokens = tokens;
        }

        public Token this[int index]
        {
            get
            {
                return tokens[index];
            }
            set
            {
                tokens[index] = value;
            }
        }

        public void Add(Token token)
        {
            tokens.Add(token);
        }

        public bool CanRead()
        {
            return cursor < tokens.Count;
        }

        public Token Next()
        {
            if (!CanRead())
                throw new IndexOutOfRangeException();
            return tokens[cursor++];
        }

        public Token Peek()
        {
            if (!CanRead())
                throw new IndexOutOfRangeException();
            return tokens[cursor];
        }

        public void AssertCanRead()
        {
            if (!CanRead())
                throw new EndOfTokenStreamException();
        }

        public bool IsNextTypeOf(TokenType type)
        {
            return Next().IsTypeOf(type);
        }

        public bool IsPeekTypeOf(TokenType type)
        {
            return Peek().IsTypeOf(type);
        }

        public void AssertNextIsTypeOf(TokenType type)
        {
            Token next = Next();
            if (!next.IsTypeOf(type))
            {
                throw new UnexpectedTokenException(type, next.Type);
            }
        }

        public void AssertPeekIsTypeOf(TokenType type)
        {
            if (!Peek().IsTypeOf(type))
            {
                throw new UnexpectedTokenException(type, Peek().Type);
            }
        }

        public bool IsNextTypeOfOne(List<TokenType> type)
        {
            return Next().IsTypeOfOne(type);
        }

        public bool IsPeekTypeOfOne(List<TokenType> type)
        {
            return Peek().IsTypeOfOne(type);
        }

        public void AssertNextIsTypeOfOne(List<TokenType> types)
        {
            Token next = Next();
            if (!next.IsTypeOfOne(types))
            {
                throw new UnexpectedTokenException(types, next.Type);
            }
        }

        public void AssertPeekIsTypeOfOne(List<TokenType> types)
        {
            if (!Peek().IsTypeOfOne(types))
            {
                throw new UnexpectedTokenException(types, Peek().Type);
            }
        }
    }
}
