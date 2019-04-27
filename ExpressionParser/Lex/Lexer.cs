using System;
using System.Collections.Generic;

namespace ExpressionParser.Lex
{
    public static class Lexer
    {
        private static int cursor;
        private static string expression;

        public static List<Token> Lex(string expression)
        {
            Lexer.expression = expression;
            cursor = 0;

            List<Token> tokens = new List<Token>();

            while (cursor < expression.Length)
            {
                Token token = LexCursor();
                if (token.Type != TokenType.WhiteSpace)
                {
                    tokens.Add(token);
                }
                cursor++;
            }
            return tokens;
        }

        private static Token LexCursor()
        {
            char currentCharacter = expression[cursor];
            switch (currentCharacter)
            {
                case TokenConstants.Divide:
                    return new Token(TokenType.Divide);
                case TokenConstants.Multiply:
                    return new Token(TokenType.Multiply);
                case TokenConstants.Addition:
                    return new Token(TokenType.Addition);
                case TokenConstants.Subtraction:
                    return new Token(TokenType.Subtraction);
                case TokenConstants.Exponent:
                    return new Token(TokenType.Exponent);
                case TokenConstants.RightParentheses:
                    return new Token(TokenType.RightParentheses);
                case TokenConstants.LeftParentheses:
                    return new Token(TokenType.LeftParentheses);

            }

            if (TokenConstants.NumberPattern.IsMatch(currentCharacter.ToString()))
            { 
                return ReadNumber();
            }
            if (TokenConstants.WhiteSpacePattern.IsMatch(currentCharacter.ToString()))
            {
                return new Token(TokenType.WhiteSpace);
            }
            return ReadIdentifier();
        }

        private static Token ReadNumber()
        {
            string number = "";
            while (ShouldReadNumber())
            {
                number += expression[cursor++].ToString();
            }
            CheckForIllegallyFormatedNumber(number);
            cursor--;
            return new Token(TokenType.Number, number);
        }

        private static bool ShouldReadNumber() 
        {
            return CanRead() && (TokenConstants.NumberPattern.IsMatch(expression[cursor].ToString()) || expression[cursor] == '.');
        }


        private static bool CanRead()
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

        private static Token ReadIdentifier()
        {
            string identifier = "";
            while (ShouldReadIdentifier())
            {
                identifier += expression[cursor++].ToString();
            }
            cursor--;
            return new Token(TokenType.Identifier, identifier);
        }

        private static bool ShouldReadIdentifier()
        {
            return CanRead() &&
                    TokenConstants.IdentifierPattern.IsMatch(expression[cursor].ToString());
        }
    }
}
