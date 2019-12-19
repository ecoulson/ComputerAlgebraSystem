using System;
namespace ExpressionParser.Semantics
{
    public class IllegalNodeTypeException : Exception
    {
        public IllegalNodeTypeException(string message) : base(message)
        {
        }
    }
}
