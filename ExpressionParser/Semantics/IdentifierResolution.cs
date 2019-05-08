using System;
using System.Collections;
using System.Collections.Generic;
using ExpressionParser.SyntaxTree;

namespace ExpressionParser.Semantics
{
    public class IdentifierResolution
    {
        private Stack<string> symbols;

        public IdentifierResolution()
        {
            symbols = new Stack<string>();
        }

        public IdentifierResolution(IdentifierResolution resolution)
        {
            symbols = new Stack<string>(resolution.symbols);
        }

        public void PushSymbol(string symbol) 
        {
            symbols.Push(symbol);
        }

        public string PopSymbol()
        {
            return symbols.Pop();
        }

        public bool HasSymbols()
        {
            return symbols.Count != 0;
        }

        public SyntaxNode ToSyntaxTree()
        {
            SyntaxNode root = new IdentifierNode(PopSymbol());
            while (HasSymbols())
            {
                OperatorNode multiplicationNode = new OperatorNode(Operator.Multiplication)
                {
                    Left = root
                };
                root = multiplicationNode;
                root.Right = ToSyntaxTree();
            }
            return root;
        }

        public string ToShortHandMultiplication()
        {
            return string.Join("", ReverseSymbols());
        }

        private IEnumerable<string> ReverseSymbols()
        {
            List<string> reverseSymbols = new List<string>();
            foreach (string symbol in symbols)
            {
                reverseSymbols.Insert(0, symbol);
            }
            return reverseSymbols;
        }

        public override string ToString()
        {
            return string.Join(", ", symbols);
        }
    }
}
