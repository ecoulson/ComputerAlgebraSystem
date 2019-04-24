using System;
using System.Collections.Generic;
using ExpressionParser.Lex;

namespace ExpressionParser.SyntaxTree
{
    public static class SyntaxTreeBuilder
    {
        private static int cursor;
        private static List<Token> tokens;

        public static SyntaxNode BuildTree(List<Token> tokens)
        {
            cursor = 0;
            SyntaxTreeBuilder.tokens = tokens;

            return ReadExpression();
        }

        private static SyntaxNode ReadExpression()
        {
            AssertNotEndOfStream();
            SyntaxNode rootNode = ReadTerm();
            return rootNode;
        }

        private static SyntaxNode ReadTerm()
        {
            SyntaxNode rootNode = ReadFactor();
            return rootNode;
        }

        private static SyntaxNode ReadFactor()
        {
            SyntaxNode rootNode = ReadFormal();
            return rootNode;
        }

        private static SyntaxNode ReadFormal()
        {
            Token nextToken = NextToken();
            switch (nextToken.Type)
            {
                case TokenType.LeftParentheses:
                    throw new NotImplementedException();

                case TokenType.Identifier:
                    throw new NotImplementedException();

                case TokenType.Number:
                    return new NumberNode(nextToken.Value);

                default:
                    List<TokenType> possibleTypes = new List<TokenType>()
                    {
                        TokenType.LeftParentheses,
                        TokenType.Identifier,
                        TokenType.Number
                    };
                    throw new UnexpectedTokenException(possibleTypes, nextToken.Type);
            }
        }

        private static SyntaxNode ReadFunction()
        {
            AssertNotEndOfStream();
            Token nameToken = NextToken();
            AssertIsTypeOf(nameToken, TokenType.Identifier);

            AssertNotEndOfStream();
            AssertIsTypeOf(NextToken(), TokenType.LeftParentheses);

            SyntaxNode functionExpression = ReadExpression();

            AssertNotEndOfStream();
            AssertIsTypeOf(NextToken(), TokenType.RightParentheses);
            return new FunctionNode(new IdentifierNode(nameToken), functionExpression);
        }

        private static void AssertNotEndOfStream()
        {
            if (!HasTokens())
            {
                throw new EndOfTokenStreamException();
            }
        }

        private static bool HasTokens()
        {
            return cursor < tokens.Count;
        }

        private static Token NextToken()
        {
            return tokens[cursor++];
        }

        private static Token PeekToken()
        {
            return tokens[cursor];
        }

        private static bool IsTypeOf(Token token, TokenType type)
        {
            return token.Type == type;
        }

        private static void AssertIsTypeOf(Token token, TokenType type)
        {
            if (!IsTypeOf(token, type))
            {
                throw new UnexpectedTokenException(type, token.Type);
            }
        }
    }
}
