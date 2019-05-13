using System;
using System.Collections.Generic;
using ExpressionParser.Lex;

namespace ExpressionParser.SyntaxTree
{
    public static class SyntaxTreeBuilder
    {
        private static Tokens tokens;

        public static SyntaxNode BuildTree(Tokens tokens)
        {
            SyntaxTreeBuilder.tokens = tokens;

            return ReadExpression();
        }

        #region Expression Nonterminal

        private static SyntaxNode ReadExpression()
        {
            tokens.AssertCanRead();
            SyntaxNode root = ReadTerm();
            while (NextTokenIsTermOperator())
            {
                tokens.AssertCanRead();
                tokens.AssertPeekIsTypeOfOne(SyntaxTreeConstants.TermOperatorTypes);

                SyntaxNode operatorNode = new OperatorNode(tokens.Next());

                operatorNode.Left = root;
                root = operatorNode;
                root.Right = ReadTerm();
            }
            return root;
        }

        private static bool NextTokenIsTermOperator()
        {
            return tokens.CanRead() && tokens.IsPeekTypeOfOne(SyntaxTreeConstants.TermOperatorTypes);
        }

        #endregion

        #region Term Nonterminal

        private static SyntaxNode ReadTerm()
        {
            tokens.AssertCanRead();
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
            return tokens.CanRead() &&
                    tokens.IsPeekTypeOfOne(SyntaxTreeConstants.FactorOperatorTypes);
        }

        private static bool IsNextTokenAShorthandFactor()
        {
            return tokens.CanRead() && (
                tokens.IsPeekTypeOf(TokenType.Identifier) ||
                tokens.IsPeekTypeOf(TokenType.LeftParentheses)
            );
        }

        private static SyntaxNode ReadComplexFactor(SyntaxNode rootNode)
        {
            tokens.AssertCanRead();
            tokens.AssertPeekIsTypeOfOne(SyntaxTreeConstants.FactorOperatorTypes);

            SyntaxNode operatorNode = new OperatorNode(tokens.Next());

            operatorNode.Left = rootNode;
            rootNode = operatorNode;
            rootNode.Right = ReadFactor();
            return rootNode;
        }

        private static SyntaxNode ReadShortHandFactor(SyntaxNode root)
        {
            tokens.AssertCanRead();
            tokens.AssertPeekIsTypeOfOne(SyntaxTreeConstants.ShorthandFactorOperatorTypes);

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
            tokens.AssertCanRead();
            SyntaxNode root = ReadFormal();
            while (NextTokenIsExponentOperator()) {
                tokens.AssertCanRead();
                tokens.AssertPeekIsTypeOf(TokenType.Exponent);

                SyntaxNode operatorNode = new OperatorNode(tokens.Next());
                operatorNode.Left = root;
                root = operatorNode;
                root.Right = ReadFormal();
            }
            return root;
        }

        private static bool NextTokenIsExponentOperator()
        {
            return tokens.CanRead() && tokens.IsPeekTypeOf(TokenType.Exponent);
        }

        #endregion

        #region Formal Nonterminal

        private static SyntaxNode ReadFormal()
        {
            tokens.AssertCanRead();
            switch (tokens.Peek().Type)
            {
                case TokenType.LeftParentheses:
                    return ReadParenthesizedExpression();
                case TokenType.Identifier:
                    return HandleFormalIdentifierAmbiguity();
                case TokenType.Number:
                    return new NumberNode(tokens.Next());
                case TokenType.Subtraction:
                    return ReadNegativeFormal();
                default:
                    throw new UnexpectedTokenException(
                        SyntaxTreeConstants.FormalTokenTypes, 
                        tokens.Peek().Type
                    );
            }
        }

        private static SyntaxNode ReadParenthesizedExpression()
        {
            tokens.AssertCanRead();
            tokens.AssertNextIsTypeOf(TokenType.LeftParentheses);

            SyntaxNode expression = ReadExpression();

            tokens.AssertCanRead();
            tokens.AssertNextIsTypeOf(TokenType.RightParentheses);

            return new ParenthesesNode(expression);
        }

        private static SyntaxNode HandleFormalIdentifierAmbiguity()
        {
            tokens.AssertCanRead();

            Token identifer = tokens.Next();
            identifer.AssertIsTypeOf(TokenType.Identifier);

            if (IdentifierIsAFunction())
                return ReadFunction(identifer);

            return new IdentifierNode(identifer);
        }

        private static bool IdentifierIsAFunction()
        {
            return tokens.CanRead() && tokens.IsPeekTypeOf(TokenType.LeftParentheses);
        }

        private static SyntaxNode ReadFunction(Token nameToken)
        {
            nameToken.AssertIsTypeOf(TokenType.Identifier);
            tokens.AssertCanRead();
            tokens.AssertNextIsTypeOf(TokenType.LeftParentheses);

            SyntaxNode functionExpression = ReadExpression();

            tokens.AssertCanRead();
            tokens.AssertNextIsTypeOf(TokenType.RightParentheses);

            return new FunctionOrDistributionNode(new IdentifierNode(nameToken), functionExpression);
        }

        private static SyntaxNode ReadNegativeFormal()
        {
            tokens.AssertCanRead();
            tokens.AssertNextIsTypeOf(TokenType.Subtraction);

            OperatorNode operatorNode = new OperatorNode(Operator.Multiplication);
            operatorNode.Left = new NumberNode(-1);
            operatorNode.Right = ReadFormal();

            return operatorNode;
        }

        #endregion
    }
}
