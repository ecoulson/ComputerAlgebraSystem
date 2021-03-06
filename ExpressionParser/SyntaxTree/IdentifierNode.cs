﻿using System;
using ExpressionParser.Lex;

namespace ExpressionParser.SyntaxTree
{
    public class IdentifierNode : SyntaxNode
    {
        public string Value { get; }

        public IdentifierNode(Token token) : base(SyntaxNodeType.Identifier)
        {
            Value = token.Value;
        }

        public IdentifierNode(string value) : base(SyntaxNodeType.Identifier)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }

        public override SyntaxNode Copy()
        {
            return new IdentifierNode(Value);
        }
    }
}
