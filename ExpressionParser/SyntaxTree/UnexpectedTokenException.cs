using System;
using System.Collections.Generic;
using ExpressionParser.Lex;

namespace ExpressionParser.SyntaxTree
{
    public class UnexpectedTokenException : Exception
    {
        public UnexpectedTokenException(TokenType expected, TokenType actual) : 
            base($"Expected '{expected}' token, but got '{actual}' instead.")
        {

        }

        public UnexpectedTokenException(List<TokenType> possibleTypes, TokenType actual) :
            base($"Expected one of these '{possibleTypes}' tokesn, but got '{actual}' instead.")
        {

        }

    }
}
