using System;
using System.Collections.Generic;
using ExpressionParser.Lex;

namespace ExpressionParser.SyntaxTree
{
    public static class SyntaxTreeBuilder
    {
        private static readonly List<TokenType> FormalTokenTypes = new List<TokenType>()
        {
            TokenType.LeftParentheses,
            TokenType.Identifier,
            TokenType.Number
        };

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

        #region Formal Nonterminal

        private static SyntaxNode ReadFormal()
        {
            Token nextToken = NextToken();
            switch (nextToken.Type)
            {
                case TokenType.LeftParentheses:
                    return ReadParenthesizedExpression();

                case TokenType.Identifier:
                    return HandleFormalIdentifierAmbiguity(nextToken);

                case TokenType.Number:
                    return new NumberNode(nextToken);

                default:
                    throw new UnexpectedTokenException(FormalTokenTypes, nextToken.Type);
            }
        }

        private static SyntaxNode ReadParenthesizedExpression()
        {
            SyntaxNode expression = ReadExpression();
            AssertNotEndOfStream();
            AssertIsTypeOf(PeekToken(), TokenType.RightParentheses);
            return expression;
        }

        private static SyntaxNode HandleFormalIdentifierAmbiguity(Token nextToken)
        {
            if (HasTokens() && IsTypeOf(PeekToken(), TokenType.LeftParentheses))
                return ReadFunction(nextToken);
            else
                return new IdentifierNode(nextToken);
        }

        private static SyntaxNode ReadFunction(Token nameToken)
        {
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

        #endregion

        #region Utility

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

        #endregion
    }
}
