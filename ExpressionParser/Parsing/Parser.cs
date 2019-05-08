﻿using System.Collections.Generic;
using ExpressionParser.Lex;
using ExpressionParser.Semantics;
using ExpressionParser.SyntaxTree;

namespace ExpressionParser.Parsing
{
    public static class Parser
    {
        public static Expression ParseExpression(string expression, Environment environment)
        {
            SyntaxNode root = SyntaxTreeBuilder.BuildTree(Lexer.Lex(expression));
            root = SemanticAnalyzer.Analyze(root, environment);
            return new Expression(root);
        }
    }
}