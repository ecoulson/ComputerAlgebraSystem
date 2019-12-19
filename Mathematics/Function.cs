using System;
using System.Collections.Generic;
using System.Text;

namespace Mathematics
{
    public class Function
    {
        private string name;
        private HashSet<string> arguments;
        private Expression expression;
        private string cachedString;

        public Function(string name, HashSet<string> arguments, Expression expression)
        {
            this.name = name;
            this.arguments = arguments;
            this.expression = expression;
        }

        public override string ToString()
        {
            return GetDisplayText();
        }

        private string GetDisplayText()
        {
            if (cachedString == null)
            {
                string argumentList = "";
                foreach (string argument in arguments)
                {
                    argumentList += argument + ", ";
                }
                argumentList = argumentList.Substring(0, argumentList.Length - 2);
                cachedString = $"{name}({argumentList}) = {expression.ToString()}";
            }
            return cachedString;
        }
    }
}
