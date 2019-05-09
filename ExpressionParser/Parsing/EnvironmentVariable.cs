using System;
using ExpressionParser.SyntaxTree;

namespace ExpressionParser.Parsing
{
    public class EnvironmentVariable
    {
        public EnvironmentVariableType Type { get; }
        public double? Value { get; }
        public SyntaxNode Expression { get; }
        public string Symbol { get; }

        public EnvironmentVariable(double value)
        {
            Type = EnvironmentVariableType.Number;
            Value = value;
        }

        public EnvironmentVariable(SyntaxNode expression)
        {
            Type = EnvironmentVariableType.Function;
            Expression = expression;
        }

        public EnvironmentVariable(string symbol)
        {
            Type = EnvironmentVariableType.Symbol;
            Symbol = symbol;
        }

        public bool IsTypeOf(EnvironmentVariableType type)
        {
            return Type == type;
        }
    }
}
