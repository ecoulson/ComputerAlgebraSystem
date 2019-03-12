using System;
using System.Text.RegularExpressions;

namespace ExpressionParser.Lex
{
    public class TokenConstants
    {
        public const char Divide = '/';
        public const char Multiply = '*';
        public const char Addition = '+';
        public const char Subtraction = '-';
        public const char LeftParentheses = '(';
        public const char RightParentheses = ')';
        public const char Exponent = '^';

        public static readonly Regex NumberPattern = new Regex("\\d");
        public static readonly Regex WhiteSpacePattern = new Regex("\\s");
        public static readonly Regex IdentifierPattern = new Regex("[A-Za-z_\\]");
    }
}
