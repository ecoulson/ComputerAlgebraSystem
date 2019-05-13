using System;
using System.Collections.Generic;
using ExpressionParser.SyntaxTree;

namespace ExpressionParser.Lex
{
    public class Token
    {
        public string Value { get; }
        public TokenType Type { get; }

        public Token(TokenType type)
        {
            this.Type = type;
        }

        public Token(TokenType type, string value)
        {
            this.Value = value;
            this.Type = type;
        }

        public override string ToString()
        {
            return $"{Type} ({Value})";
        }

        public bool IsTypeOf(TokenType type)
        {
            return Type == type;
        }

        public bool IsTypeOfOne(List<TokenType> types)
        {
            return types.Contains(Type);
        }

        public void AssertIsTypeOf(TokenType type)
        {
            if (!IsTypeOf(type))
            {
                throw new UnexpectedTokenException(Type, type);
            }
        }

        public void AssertIsTypeOfOne(List<TokenType> types)
        {
            if (!IsTypeOfOne(types))
            {
                throw new UnexpectedTokenException(types, Type);
            }
        }
    }
}
