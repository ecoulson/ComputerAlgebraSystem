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

        public void AddFunction(string symbol, Expression expression)
        {
            mapping[symbol] = new EnvironmentVariable(expression);
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

        public EnvironmentVariable Get(string symbol)
        {
            if (mapping.ContainsKey(symbol))
                return mapping[symbol];
            throw new ArgumentException($"Unknown variable '{symbol}' not found in environment");
        }


        public bool HasVariable(string symbol)
        {
            return mapping.ContainsKey(symbol);
        }
    }
}
