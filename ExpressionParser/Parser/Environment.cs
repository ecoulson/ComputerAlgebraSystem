using System;
using System.Collections.Generic;
using ExpressionParser.SyntaxTree;

namespace ExpressionParser.Parser
{
    public class Environment
    {
        private static readonly ISet<string> PredefinedFunctions = new HashSet<string>()
        {
            "sin",
            "cos",
            "tan",
            "csc",
            "sec",
            "cot",
            "arcsin",
            "arccos",
            "arctan",
            "arccsc",
            "arcsec",
            "arctan",
            "ln",
            "log",
            "sqrt",
        };

        private static readonly Dictionary<string, EnvironmentVariable> PredefinedSymbols = new Dictionary<string, EnvironmentVariable>()
        {
            { "e", new EnvironmentVariable(Math.E) },
            { "pi", new EnvironmentVariable(Math.PI) },
            { "i", new EnvironmentVariable("i") }
        };

        private readonly Dictionary<string, EnvironmentVariable> mapping;

        public Environment()
        {
            mapping = new Dictionary<string, EnvironmentVariable>(PredefinedSymbols);
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
            foreach (KeyValuePair<string, EnvironmentVariable> pair in mapping)
            {
                if (!pair.Value.IsTypeOf(EnvironmentVariableType.Function))
                {
                    symbols.Add(pair.Key);
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

        public bool IsPredefinedFunction(string symbol)
        {
            return PredefinedFunctions.Contains(symbol);
        }

        public bool IsPredefinedSymbol(string symbol)
        {
            return PredefinedSymbols.ContainsKey(symbol);
        }

        public bool IsKeyword(string symbol)
        {
            return IsPredefinedSymbol(symbol) || IsPredefinedFunction(symbol);
        }
    }
}
