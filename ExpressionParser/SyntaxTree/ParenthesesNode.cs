﻿using System;
namespace ExpressionParser.SyntaxTree
{
    public class ParenthesesNode : SyntaxNode
    {
        public ParenthesesNode(SyntaxNode expression) : base(SyntaxNodeType.Parentheses)
        {
            Left = expression;
        }

        public override string ToString()
        {
            return $"({Left.ToString()})";
        }
    }
}