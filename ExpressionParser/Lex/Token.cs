using System;

namespace ExpressionParser.Lex
{
    public class Token
    {
        private readonly string value;
        private readonly TokenType type;

        public Token(TokenType type, string value)
        {
            this.value = value;
            this.type = type;
        }

        public string GetValue()
        {
            return value;
        }

        public new TokenType GetType()
        {
            return type;
        }
    }
}
