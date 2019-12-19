using System;
using System.Collections.Generic;
using ExpressionParser.SyntaxTree;

namespace ExpressionParser.Parsing
{
    public class Environment
    {
        private static readonly Dictionary<string, EnvironmentVariable> PredefinedFunctions = new Dictionary<string, EnvironmentVariable>()
        {
            { "sin", new EnvironmentVariable(new FunctionNode()) },
            { "cos", new EnvironmentVariable(new FunctionNode()) },
            { "tan", new EnvironmentVariable(new FunctionNode()) },
            { "csc", new EnvironmentVariable(new FunctionNode()) },
            { "sec", new EnvironmentVariable(new FunctionNode()) },
            { "cot", new EnvironmentVariable(new FunctionNode()) },
            { "arcsin", new EnvironmentVariable(new FunctionNode()) },
            { "arccos", new EnvironmentVariable(new FunctionNode()) },
            { "arctan", new EnvironmentVariable(new FunctionNode()) },
            { "arccsc", new EnvironmentVariable(new FunctionNode()) },
            { "arcsec", new EnvironmentVariable(new FunctionNode()) },
            { "arccot", new EnvironmentVariable(new FunctionNode()) },
            { "ln", new EnvironmentVariable(new FunctionNode()) },
            { "log", new EnvironmentVariable(new FunctionNode()) },
            { "sqrt", new EnvironmentVariable(new FunctionNode()) }
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

        public void AddFunction(string symbol, SyntaxNode expression)
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

        public bool HasFunction(string name)
        {
            return mapping.ContainsKey(name) && mapping[name].IsTypeOf(EnvironmentVariableType.Function);
        }

        public bool HasVariable(string variable)
        {
            return mapping.ContainsKey(variable);
        }

        public bool HasConstant(string constant)
        {
            return mapping.ContainsKey(constant) && mapping[constant].IsTypeOf(EnvironmentVariableType.Number);
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
