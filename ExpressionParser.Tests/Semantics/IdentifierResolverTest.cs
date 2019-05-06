using System;
using System.Collections.Generic;
using ExpressionParser.Semantics;
using NUnit.Framework;

namespace ExpressionParser.Tests.Semantics
{
    [TestFixture]
    public class IdentifierResolverTest
    {
        [Test]
        public void Resolve_TwoSymbols_ReturnsListOfString()
        {
            List<string> symbols = new List<string>
            {
                "x", "y"
            };

            List<IdentifierResolution> combinations = IdentifierResolver.Resolve(symbols, "xy");

            Assert.AreEqual(1, combinations.Count);
            Assert.AreEqual("x, y", combinations.ToString());
        }
    }
}
