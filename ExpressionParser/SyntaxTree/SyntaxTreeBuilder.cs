using System;
using System.Collections.Generic;
using ExpressionParser.Lex;

namespace ExpressionParser.SyntaxTree
{
    public class SyntaxTreeBuilder
    {
        private Tokens tokens;

        public SyntaxNode BuildTree(Tokens tokens)
        {
            this.tokens = tokens;

            return ReadExpression();
        }

        #region Expression Nonterminal

        private  SyntaxNode ReadExpression()
        {
            tokens.AssertCanRead();
            SyntaxNode firstTerm = ReadTerm();
            if (NextTokenIsATermOperator())
            {
                return new OperatorNode(Operator.Addition, ReadExpressionOperands(firstTerm));
            }
            return firstTerm;
        }

        private bool NextTokenIsATermOperator()
        {
            return tokens.CanRead() && tokens.IsPeekTypeOfOne(SyntaxTreeConstants.TermOperatorTypes);
        }

        private List<SyntaxNode> ReadExpressionOperands(SyntaxNode firstTerm)
        {
            List<SyntaxNode> operands = new List<SyntaxNode> { firstTerm };
            while (NextTokenIsATermOperator())
            {
                tokens.AssertCanRead();
                tokens.AssertPeekIsTypeOfOne(SyntaxTreeConstants.TermOperatorTypes);

                AddToExpressionOperands(tokens.Next(), operands);
            }
            return operands;
        }

        private void AddToExpressionOperands(Token operatorToken, List<SyntaxNode> operands)
        {
            if (operatorToken.IsTypeOf(TokenType.Addition))
            {
                operands.Add(ReadTerm());
            }
            else
            {
                operands.Add(new OperatorNode(Operator.Multiplication, new List<SyntaxNode>
                {
                    SyntaxTreeConstants.NegativeOne,
                    ReadTerm()
                }));
            }
        }

        #endregion

        #region Term Nonterminal

        private SyntaxNode ReadTerm()
        {
            tokens.AssertCanRead();
            SyntaxNode root = ReadFactor();
            if (NextTokenIsAFactorOperator() || NextTokenIsAShorthandFactor())
            {
                return new OperatorNode(Operator.Multiplication, ReadTermOperands(root));
            }
            return root;
        }

        private bool NextTokenIsAFactorOperator()
        {
            return tokens.CanRead() &&
                    tokens.IsPeekTypeOfOne(SyntaxTreeConstants.FactorOperatorTypes);
        }

        private bool NextTokenIsAShorthandFactor()
        {
            return tokens.CanRead() &&
                tokens.IsPeekTypeOfOne(SyntaxTreeConstants.ShorthandFactorOperatorTypes);
        }

        private List<SyntaxNode> ReadTermOperands(SyntaxNode firstTerm)
        {
            List<SyntaxNode> operands = new List<SyntaxNode> { firstTerm };
            while (NextTokenIsAFactorOperator() || NextTokenIsAShorthandFactor())
            {
                if (NextTokenIsAFactorOperator())
                {
                    AddComplexFactorToOperands(operands);
                }
                else if (NextTokenIsAShorthandFactor())
                {
                    AddShortHandFactorToOperands(operands);
                }
            }
            return operands;
        }

        private void AddComplexFactorToOperands(List<SyntaxNode> operands)
        {
            tokens.AssertCanRead();
            tokens.AssertPeekIsTypeOfOne(SyntaxTreeConstants.FactorOperatorTypes);
            Token operatorToken = tokens.Next();
            if (operatorToken.IsTypeOf(TokenType.Multiply))
            {
                operands.Add(ReadFactor());
            }
            else
            {
                operands.Add(new OperatorNode(Operator.Exponentiation, new List<SyntaxNode>
                {
                    ReadFactor(),
                    new NumberNode(-1)
                }));
            }
        }

        private void AddShortHandFactorToOperands(List<SyntaxNode> operands)
        {
            tokens.AssertCanRead();
            tokens.AssertPeekIsTypeOfOne(SyntaxTreeConstants.ShorthandFactorOperatorTypes);
            operands.Add(ReadFactor());
        }

        #endregion

        #region Factor Nonterminal

        private SyntaxNode ReadFactor()
        {
            tokens.AssertCanRead();
            SyntaxNode root = ReadFormal();
            if (NextTokenIsExponentOperator())
            {
                return new OperatorNode(Operator.Exponentiation, ReadFactorOperands(root));
            }
            return root;
        }

        private bool NextTokenIsExponentOperator()
        {
            return tokens.CanRead() && tokens.IsPeekTypeOf(TokenType.Exponent);
        }

        private List<SyntaxNode> ReadFactorOperands(SyntaxNode firstTerm)
        {
            List<SyntaxNode> operands = new List<SyntaxNode> { firstTerm };
            while (NextTokenIsExponentOperator())
            {
                tokens.AssertCanRead();
                tokens.AssertPeekIsTypeOf(TokenType.Exponent);
                tokens.Next();

                operands.Add(ReadFormal());
            }
            return operands;
        }

        #endregion

        #region Formal Nonterminal

        private SyntaxNode ReadFormal()
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

        private SyntaxNode ReadParenthesizedExpression()
        {
            tokens.AssertCanRead();
            tokens.AssertNextIsTypeOf(TokenType.LeftParentheses);

            SyntaxNode expression = ReadExpression();

            tokens.AssertCanRead();
            tokens.AssertNextIsTypeOf(TokenType.RightParentheses);

            return new ParenthesesNode(expression);
        }

        private SyntaxNode HandleFormalIdentifierAmbiguity()
        {
            tokens.AssertCanRead();

            Token identifer = tokens.Next();
            identifer.AssertIsTypeOf(TokenType.Identifier);

            if (IdentifierIsAFunction())
                return ReadFunction(identifer);

            return new IdentifierNode(identifer);
        }

        private bool IdentifierIsAFunction()
        {
            return tokens.CanRead() && tokens.IsPeekTypeOf(TokenType.LeftParentheses);
        }

        private SyntaxNode ReadFunction(Token nameToken)
        {
            nameToken.AssertIsTypeOf(TokenType.Identifier);
            tokens.AssertCanRead();
            tokens.AssertNextIsTypeOf(TokenType.LeftParentheses);

            SyntaxNode functionExpression = ReadExpression();

            tokens.AssertCanRead();
            tokens.AssertNextIsTypeOf(TokenType.RightParentheses);

            return new FunctionOrDistributionNode(new IdentifierNode(nameToken), functionExpression);
        }

        private SyntaxNode ReadNegativeFormal()
        {
            tokens.AssertCanRead();
            tokens.AssertNextIsTypeOf(TokenType.Subtraction);

            return new OperatorNode(Operator.Multiplication, new List<SyntaxNode>
            {
                new NumberNode(-1), ReadFormal()
            });
        }

        #endregion
    }
}
