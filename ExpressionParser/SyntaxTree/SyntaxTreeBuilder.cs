using System;
using System.Collections.Generic;
using ExpressionParser.Lex;

namespace ExpressionParser.SyntaxTree
{
    public class SyntaxTreeBuilder
    {
        private int cursor;

        public SyntaxTreeBuilder()
        {
            cursor = 0;
        }

        public SyntaxNode BuildTree(List<Token> tokens)
        {
            SyntaxNode root = new SyntaxNode();
            while (cursor < tokens.Count)
            {
                cursor++;
            }
            return root;
        }
    }
}
