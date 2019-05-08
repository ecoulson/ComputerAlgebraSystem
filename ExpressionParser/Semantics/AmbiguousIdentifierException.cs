using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionParser.Semantics
{
    public class AmbiguousIdentifierException : Exception
    {
        public AmbiguousIdentifierException(string toResolve, List<IdentifierResolution> resolutions) 
            : base($"Ambiguous identifier '{toResolve}' can be made from {GetCombinations(resolutions)}")
        {
        }

        private static string GetCombinations(List<IdentifierResolution> resolutions)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < resolutions.Count; i++)
            {
                builder.Append($"'{resolutions[i].ToString()}'");
                if (i < resolutions.Count - 1)
                {
                    builder.Append(", ");
                }
            }
            return builder.ToString();
        }
    }
}
