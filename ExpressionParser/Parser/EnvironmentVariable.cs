using System;
namespace ExpressionParser.Parser
{
    public class EnvironmentVariable
    {
        public EnvironmentVariableType Type { get; }
        public double? Value { get; }
        public Expression Expression { get; }
        public string Symbol { get; }

        public EnvironmentVariable(double value)
        {
            Type = EnvironmentVariableType.Number;
            Value = value;
        }

        public EnvironmentVariable(Expression expression)
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
