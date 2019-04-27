using System;
using ExpressionParser.SyntaxTree;

namespace ExpressionParser.Semantics
{
    public class UndefinedSymbolException : Exception
    {
        public UndefinedSymbolException(IdentifierNode node) :
            base($"Undefined symbol '{node.Value}'")
        {
        }
    }
}
