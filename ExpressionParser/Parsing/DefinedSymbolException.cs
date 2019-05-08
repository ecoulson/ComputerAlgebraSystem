using System;
namespace ExpressionParser.Parsing
{
    public class DefinedSymbolException : Exception
    {
        public DefinedSymbolException(string symbol) : base($"Symbol {symbol} has already been defined")
        {
        }
    }
}
