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

        #region Expression Nonterminal

        private static SyntaxNode ReadExpression()
        {
            AssertNotEndOfStream();
            SyntaxNode root = ReadTerm();
            while (NextTokenIsTermOperator())
            {
                AssertNotEndOfStream();
                AssertIsTypeOfOne(PeekToken(), SyntaxTreeConstants.TermOperatorTypes);

                SyntaxNode operatorNode = new OperatorNode(NextToken());

                operatorNode.Left = root;
                root = operatorNode;
                root.Right = ReadTerm();
            }
            return root;
        }

        private static bool NextTokenIsTermOperator()
        {
            return HasTokens() && IsTypeOfOne(PeekToken(), SyntaxTreeConstants.TermOperatorTypes);
        }

        #endregion

        #region Term Nonterminal

        private static SyntaxNode ReadTerm()
        {
            AssertNotEndOfStream();
            SyntaxNode root = ReadFactor();

            while (IsNextTokenFactorOperator() || IsNextTokenAShorthandFactor())
            {
                if (IsNextTokenFactorOperator())
                {
                    root = ReadComplexFactor(root);
                }
                else if (IsNextTokenAShorthandFactor())
                {
                    root = ReadShortHandFactor(root);
                }
            }

            return root;
        }

        private static bool IsNextTokenFactorOperator()
        {
            return HasTokens() &&
                    IsTypeOfOne(PeekToken(), SyntaxTreeConstants.FactorOperatorTypes);
        }

        private static bool IsNextTokenAShorthandFactor()
        {
            return HasTokens() && (
                IsTypeOf(PeekToken(), TokenType.Identifier) || 
                IsTypeOf(PeekToken(), TokenType.LeftParentheses)
            );
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

        private static SyntaxNode ReadShortHandFactor(SyntaxNode root)
        {
            AssertNotEndOfStream();
            AssertIsTypeOfOne(PeekToken(), SyntaxTreeConstants.ShorthandFactorOperatorTypes);

            SyntaxNode multiplyNode = new OperatorNode(SyntaxTreeConstants.MultiplyToken);

            multiplyNode.Left = root;
            root = multiplyNode;
            root.Right = ReadFactor();
            return root;
        }

        #endregion

        #region Factor Nonterminal

        private static SyntaxNode ReadFactor()
        {
            AssertNotEndOfStream();
            SyntaxNode root = ReadFormal();
            while (NextTokenIsExponentOperator()) {
                AssertNotEndOfStream();
                AssertIsTypeOf(PeekToken(), TokenType.Exponent);

                SyntaxNode operatorNode = new OperatorNode(NextToken());
                operatorNode.Left = root;
                root = operatorNode;
                root.Right = ReadFormal();
            }
            return root;
        }

        private static bool NextTokenIsExponentOperator()
        {
            return HasTokens() && IsTypeOf(PeekToken(), TokenType.Exponent);
        }

        #endregion

        #region Formal Nonterminal

        private static SyntaxNode ReadFormal()
        {
            AssertNotEndOfStream();
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

            if (IdentifierIsAFunction())
                return ReadFunction(identifer);

            return new IdentifierNode(identifer);
        }

        private static bool IdentifierIsAFunction()
        {
            return HasTokens() && IsTypeOf(PeekToken(), TokenType.LeftParentheses);
        }

        private static SyntaxNode ReadFunction(Token nameToken)
        {
            AssertIsTypeOf(nameToken, TokenType.Identifier);
            AssertNotEndOfStream();
            AssertIsTypeOf(NextToken(), TokenType.LeftParentheses);

            SyntaxNode functionExpression = ReadExpression();

            AssertNotEndOfStream();
            AssertIsTypeOf(NextToken(), TokenType.RightParentheses);

            return new FunctionOrDistributionNode(new IdentifierNode(nameToken), functionExpression);
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
