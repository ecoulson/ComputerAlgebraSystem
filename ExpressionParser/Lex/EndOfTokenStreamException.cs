using System;
namespace ExpressionParser.Lex
{
    public class EndOfTokenStreamException : Exception
    {
        public EndOfTokenStreamException() : 
            base("Unexpected end of token stream")
        {
        }
    }
}
