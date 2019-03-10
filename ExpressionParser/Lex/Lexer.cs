using System;
using System.Collections.Generic;

namespace ExpressionParser.Lex
{
    public static class Lexer
    {
        public static List<Token> Lex(string expression)
        {
            List<Token> tokens = new List<Token>();
            int cursor = 0;
            while (cursor < expression.Length)
            {
                Token token = LexCursor(expression, ref cursor);
                if (token.GetType() != TokenType.WhiteSpace)
                {
                    tokens.Add(token);
                }
                cursor++;
            }
            return tokens;
        }

        private static Token LexCursor(string expression, ref int cursor)
        {
            char currentCharacter = expression[cursor];
            switch (currentCharacter)
            {
                case TokenConstants.Divide:
                    return new Token(TokenType.Divide, "");
                case TokenConstants.Multiply:
                    return new Token(TokenType.Multiply, "");
                case TokenConstants.Addition:
                    return new Token(TokenType.Addition, "");
                case TokenConstants.Subtraction:
                    return new Token(TokenType.Subtraction, "");
                case TokenConstants.Exponent:
                    return new Token(TokenType.Exponent, "");
                case TokenConstants.RightParentheses:
                    return new Token(TokenType.RightParentheses, "");
                case TokenConstants.LeftParentheses:
                    return new Token(TokenType.LeftParentheses, "");

            }
            if (TokenConstants.NumberPattern.IsMatch(currentCharacter.ToString()))
            { 
                return ReadNumber(expression, ref cursor);
            }
            else if (TokenConstants.WhiteSpacePattern.IsMatch(currentCharacter.ToString()))
            {
                return new Token(TokenType.WhiteSpace, "");
            }
            else
            {
                return ReadIdentifier(expression, ref cursor);
            }
        }

        private static Token ReadNumber(string expression, ref int cursor)
        {
            string number = "";
            while (ShouldReadNumber(expression, cursor))
            {
                number += expression[cursor++].ToString();
            }
            CheckForIllegallyFormatedNumber(number);
            cursor--;
            return new Token(TokenType.Number, number);
        }

        private static bool ShouldReadNumber(string expression, int cursor) 
        {
            return CanRead(expression, cursor) && 
                    (TokenConstants.NumberPattern.IsMatch(expression[cursor].ToString()) || expression[cursor] == '.');
        }


        private static bool CanRead(string expression, int cursor)
        {
            return cursor < expression.Length;
        }

        private static void CheckForIllegallyFormatedNumber(string number)
        {
            if (number.IndexOf('.') != number.LastIndexOf('.'))
            {
                throw new FormatException("Illegally formatted decimal number");
            }
        }

        private static Token ReadIdentifier(string expression, ref int cursor)
        {
            string identifier = "";
            while (ShouldReadIdentifier(expression, cursor))
            {
                identifier += expression[cursor++].ToString();
            }
            cursor--;
            return new Token(TokenType.Identifier, identifier);
        }

        private static bool ShouldReadIdentifier(string expression, int cursor)
        {
            return CanRead(expression, cursor) &&
                    TokenConstants.IdentifierPattern.IsMatch(expression[cursor].ToString());
        }
    }
}
