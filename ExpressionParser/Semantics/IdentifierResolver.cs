using System;
using System.Collections.Generic;

namespace ExpressionParser.Semantics
{
    public static class IdentifierResolver
    {
        public static List<IdentifierResolution> Resolve(List<string> symbols, string toResolve)
        {
            List<IdentifierResolution> resolutions = new List<IdentifierResolution>();
            for (int i = 0; i < symbols.Count; i++)
            {
                IdentifierResolution resolution = new IdentifierResolution();
                string symbol = symbols[i];
                resolution.PushSymbol(symbol);

                if (symbol == toResolve)
                {
                    resolutions.Add(new IdentifierResolution(resolution));
                }
                else
                {
                    Resolve(symbols, toResolve, resolutions, resolution);
                }

                resolution.PopSymbol();
            }
            return resolutions;
        }

        private static void Resolve(List<string> symbols, string toResolve, List<IdentifierResolution> resolutions, IdentifierResolution currentResolution)
        {
            for (int i = 0; i < symbols.Count; i++)
            {
                string symbol = symbols[i];
                currentResolution.PushSymbol(symbol);
                string shorthandMultiplication = currentResolution.ToShortHandMultiplication();

                if (shorthandMultiplication == toResolve)
                {
                    resolutions.Add(new IdentifierResolution(currentResolution));
                } 
                else if (toResolve.StartsWith(shorthandMultiplication, StringComparison.Ordinal))
                {
                    Resolve(symbols, toResolve, resolutions, currentResolution);
                }

                currentResolution.PopSymbol();
            }
        }
    }
}
