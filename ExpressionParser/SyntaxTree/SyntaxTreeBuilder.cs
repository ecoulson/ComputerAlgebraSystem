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

        #region Term Nonterminal

        private static SyntaxNode ReadTerm()
        {
            AssertNotEndOfStream();
            SyntaxNode rootNode = ReadFactor();

            if (IsNextTokenFactorOperator())
            {
                rootNode = ReadComplexFactor(rootNode);
            }
            else if (IsNextTokenAShorthandFactor())
            {
                rootNode = ReadShortHandFactor(rootNode);
            }

            return rootNode;
        }

        private static bool IsNextTokenFactorOperator()
        {
            return HasTokens() &&
                    IsTypeOfOne(PeekToken(), SyntaxTreeConstants.FactorOperatorTypes);
        }

        private static SyntaxNode ReadComplexFactor(SyntaxNode rootNode)
        {
            AssertNotEndOfStream();
            AssertIsTypeOfOne(PeekToken(), SyntaxTreeConstants.FactorOperatorTypes);

            SyntaxNode operatorNode = new OperatorNode(NextToken());

            operatorNode.Left = rootNode;
            rootNode = operatorNode;
            rootNode.Right = ReadFactor();
            return rootNode;
        }

        private static bool IsNextTokenAShorthandFactor()
        {
            return HasTokens() && IsTypeOf(PeekToken(), TokenType.Identifier);
        }

        private static SyntaxNode ReadShortHandFactor(SyntaxNode rootNode)
        {
            AssertNotEndOfStream();
            AssertIsTypeOf(PeekToken(), TokenType.Identifier);

            SyntaxNode multiplyNode = new OperatorNode(
                new Token(TokenType.Multiply, "")
            );

            multiplyNode.Left = rootNode;
            rootNode = multiplyNode;
            rootNode.Right = ReadFactor();
            return rootNode;
        }

        #endregion

        #region Factor Nonterminal

        private static SyntaxNode ReadFactor()
        {
            return ReadFormal();
        }

        #endregion

        #region Formal Nonterminal

        private static SyntaxNode ReadFormal()
        {
            switch (PeekToken().Type)
            {
                case TokenType.LeftParentheses:
                    return ReadParenthesizedExpression();

                case TokenType.Identifier:
                    return HandleFormalIdentifierAmbiguity();

                case TokenType.Number:
                    return new NumberNode(NextToken());

                default:
                    throw new UnexpectedTokenException(
                        SyntaxTreeConstants.FormalTokenTypes, 
                        PeekToken().Type
                    );
            }
        }

        private static SyntaxNode ReadParenthesizedExpression()
        {
            AssertNotEndOfStream();
            AssertIsTypeOf(NextToken(), TokenType.LeftParentheses);

            SyntaxNode expression = ReadExpression();

            AssertNotEndOfStream();
            AssertIsTypeOf(NextToken(), TokenType.RightParentheses);

            return expression;
        }

        private static SyntaxNode HandleFormalIdentifierAmbiguity()
        {
            AssertNotEndOfStream();
            Token identifer = NextToken();
            AssertIsTypeOf(identifer, TokenType.Identifier);

            if (HasTokens() && IsTypeOf(PeekToken(), TokenType.LeftParentheses))
                return ReadFunction(identifer);

            return new IdentifierNode(identifer);
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

        #endregion

        #region Utility

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

        private static bool IsTypeOfOne(Token token, List<TokenType> types)
        {
            return types.Contains(token.Type);
        }

        private static void AssertIsTypeOfOne(Token token, List<TokenType> types) 
        { 
            if (!IsTypeOfOne(token, types))
            {
                throw new UnexpectedTokenException(types, token.Type);
            }
        }


        #endregion
    }
}
