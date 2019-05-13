using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ExpressionParser.Lex
{
    public static class Lexer
    {
        private static RawExpression expression;

        public static List<Token> Lex(RawExpression expression)
        {
            Lexer.expression = expression;
            List<Token> tokens = new List<Token>();

            while (expression.CanRead())
            {
                Token token = LexCursor();
                if (token.Type != TokenType.WhiteSpace)
                {
                    tokens.Add(token);
                }
            }
            return tokens;
        }

        private static Token LexCursor()
        {
            char currentCharacter = expression.Peek();
            switch (currentCharacter)
            {
                case TokenConstants.Divide:
                    expression.Next();
                    return new Token(TokenType.Divide);
                case TokenConstants.Multiply:
                    expression.Next();
                    return new Token(TokenType.Multiply);
                case TokenConstants.Addition:
                    expression.Next();
                    return new Token(TokenType.Addition);
                case TokenConstants.Subtraction:
                    expression.Next();
                    return new Token(TokenType.Subtraction);
                case TokenConstants.Exponent:
                    expression.Next();
                    return new Token(TokenType.Exponent);
                case TokenConstants.RightParentheses:
                    expression.Next();
                    return new Token(TokenType.RightParentheses);
                case TokenConstants.LeftParentheses:
                    expression.Next();
                    return new Token(TokenType.LeftParentheses);

            }

            if (MatchesPattern(TokenConstants.NumberPattern, currentCharacter))
            { 
                return ReadNumber();
            }
            if (MatchesPattern(TokenConstants.WhiteSpacePattern, currentCharacter))
            {
                expression.Next();
                return new Token(TokenType.WhiteSpace);
            }
            return ReadIdentifier();
        }

        public static bool MatchesPattern(Regex pattern, char ch)
        {
            return pattern.IsMatch(ch.ToString());
        }

        private static Token ReadNumber()
        {
            string number = "";
            while (ShouldReadNumber())
            {
                number += expression.Next();
            }
            IsLegallyFormatedNumber(number);
            return new Token(TokenType.Number, number);
        }

        private static bool ShouldReadNumber() 
        {
            return expression.CanRead() && IsNumberCharacter();
        }

        private static bool IsNumberCharacter()
        {
            return MatchesPattern(TokenConstants.NumberPattern, expression.Peek()) ||
                expression.Peek() == TokenConstants.Dot;
        }

        private static void IsLegallyFormatedNumber(string number)
        {
            if (number.IndexOf(TokenConstants.Dot) != number.LastIndexOf(TokenConstants.Dot))
            {
                throw new FormatException("Illegally formatted decimal number");
            }
        }

        private static Token ReadIdentifier()
        {
            string identifier = "";
            while (ShouldReadIdentifier())
            {
                identifier += expression.Next();
            }
            return new Token(TokenType.Identifier, identifier);
        }

        private static bool ShouldReadIdentifier()
        {
            return expression.CanRead() &&
                    MatchesPattern(TokenConstants.IdentifierPattern, expression.Peek());
        }
    }
}
