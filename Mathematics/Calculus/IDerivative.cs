using System;
using ExpressionParser.SyntaxTree;

namespace Mathematics.Calculus
{
    public interface IDerivative
    {
        Expression Derive();
    }
}
