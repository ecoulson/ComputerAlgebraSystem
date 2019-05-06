using System;
using System.Collections.Generic;

namespace ExpressionParser.Semantics
{
    public static class IdentifierResolver
    {
        public static List<IdentifierResolution> Resolve(List<string> symbols, string toResolve)
        {
            List<IdentifierResolution> resolutions = new List<IdentifierResolution>();
            Resolve(symbols, toResolve, resolutions);
            return resolutions;
        }

        private static void Resolve(List<string> symbols, string toResolve, List<IdentifierResolution> resolutions)
        {
            if (symbols.Count == 0)
            {

            }
            else
            {

            }
        }
    }
}
