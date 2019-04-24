using System;
namespace ExpressionParser.SyntaxTree
{
    public class EndOfTokenStreamException : Exception
    {
        public EndOfTokenStreamException() : 
            base("Unexpected end of token stream")
        {
        }
    }
}
