using System;
using System.Collections.Generic;
using ExpressionParser.SyntaxTree;

namespace ExpressionParser.Parsing
{
    public class Environment
    {
        private static readonly Dictionary<string, EnvironmentVariable> PredefinedFunctions = new Dictionary<string, EnvironmentVariable>()
        {
            { "sin", new EnvironmentVariable(new Expression(null)) },
            { "cos", new EnvironmentVariable(new Expression(null)) },
            { "tan", new EnvironmentVariable(new Expression(null)) },
            { "csc", new EnvironmentVariable(new Expression(null)) },
            { "sec", new EnvironmentVariable(new Expression(null)) },
            { "cot", new EnvironmentVariable(new Expression(null)) },
            { "arcsin", new EnvironmentVariable(new Expression(null)) },
            { "arccos", new EnvironmentVariable(new Expression(null)) },
            { "arctan", new EnvironmentVariable(new Expression(null)) },
            { "arccsc", new EnvironmentVariable(new Expression(null)) },
            { "arcsec", new EnvironmentVariable(new Expression(null)) },
            { "arccot", new EnvironmentVariable(new Expression(null)) },
            { "ln", new EnvironmentVariable(new Expression(null)) },
            { "log", new EnvironmentVariable(new Expression(null)) },
            { "sqrt", new EnvironmentVariable(new Expression(null)) }
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
            foreach (KeyValuePair<string, EnvironmentVariable> pair in PredefinedFunctions)
            {
                mapping.Add(pair.Key, pair.Value);
            }
        }

        public void AddSymbol(string symbol)
        {
            if (mapping.ContainsKey(symbol))
                throw new DefinedSymbolException(symbol);
            mapping[symbol] = new EnvironmentVariable(symbol);
        }

        public void AddFunction(string symbol, Expression expression)
        {
            if (mapping.ContainsKey(symbol))
                throw new DefinedSymbolException(symbol);
            mapping[symbol] = new EnvironmentVariable(expression);
        }

        public void AddValue(string symbol, double value)
        {
            if (mapping.ContainsKey(symbol))
                throw new DefinedSymbolException(symbol);
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
            return PredefinedFunctions.ContainsKey(symbol);
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
