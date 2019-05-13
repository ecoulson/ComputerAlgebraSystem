using System;
namespace ExpressionParser.Lex
{
    public class RawExpression
    {
        private int cursor;
        private string rawExpression;

        public RawExpression(string expression)
        {
            cursor = 0;
            rawExpression = expression;
        }

        public bool CanRead()
        {
            return cursor < rawExpression.Length;
        }

        public char Next()
        {
            if (!CanRead())
            {
                throw new IndexOutOfRangeException();
            }
            return rawExpression[cursor++];
        }

        public char Peek()
        {
            if (!CanRead())
            {
                throw new IndexOutOfRangeException();
            }
            return rawExpression[cursor];
        }
    }
}
