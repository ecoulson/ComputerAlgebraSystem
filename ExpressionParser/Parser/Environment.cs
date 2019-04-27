using System;
using System.Collections.Generic;
using ExpressionParser.SyntaxTree;

namespace ExpressionParser.Parser
{
    public class Environment
    {
        private readonly Dictionary<string, EnvironmentVariable> mapping;

        public Environment()
        {
            mapping = new Dictionary<string, EnvironmentVariable>();
        }

        public void AddSymbol(string symbol)
        {
            mapping[symbol] = new EnvironmentVariable(symbol);
        }

        public void AddFunction(string name, Expression expression)
        {
            mapping[name] = new EnvironmentVariable(expression);
        }

        public void AddValue(string symbol, double value)
        {
            mapping[symbol] = new EnvironmentVariable(value);
        }

        public List<string> Symbols()
        {
            List<string> symbols = new List<string>();
            foreach (KeyValuePair<string, EnvironmentVariable> variable in mapping)
            {
                if (variable.Value.Type != EnvironmentVariableType.Function)
                {
                    symbols.Add(variable.Key);
                }
            }
            return symbols;
        }

        public bool IsSymbol(string symbol)
        {
            return mapping.ContainsKey(symbol) && 
                mapping[symbol].Type == EnvironmentVariableType.Symbol;
        }

        public bool IsValue(string symbol)
        {
            return mapping.ContainsKey(symbol) &&
                mapping[symbol].Type == EnvironmentVariableType.Number;
        }

        public bool IsFunction(string symbol)
        {
            return mapping.ContainsKey(symbol) &&
                mapping[symbol].Type == EnvironmentVariableType.Function;
        }
    }
}
