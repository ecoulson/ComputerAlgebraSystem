using System;
using System.Collections.Generic;

namespace ExpressionParser.Semantics
{
    public class IdentifierResolution
    {
        private List<string> symbols;

        public IdentifierResolution(List<string> symbols)
        {
            this.symbols = symbols ?? throw new ArgumentNullException();
        }

        public override string ToString()
        {
            return string.Join(", ", symbols);
        }
    }
}
