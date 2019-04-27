using System;

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
    }
}
