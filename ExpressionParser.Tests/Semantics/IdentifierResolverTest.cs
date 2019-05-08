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
        public void Resolve_OneVariableOneCombination_ReturnsListOfString()
        {
            List<string> symbols = new List<string>
            {
                "x"
            };

            List<IdentifierResolution> combinations = IdentifierResolver.Resolve(symbols, "x");

            Assert.AreEqual(1, combinations.Count);
            Assert.AreEqual("x", combinations[0].ToString());
        }

        [Test]
        public void Resolve_TwoVariablesOneCombination_ReturnsListOfString()
        {
            List<string> symbols = new List<string>
            {
                "x", "y"
            };

            List<IdentifierResolution> combinations = IdentifierResolver.Resolve(symbols, "xy");

            Assert.AreEqual(1, combinations.Count);
            Assert.AreEqual("x, y", combinations[0].ToString());
        }

        [Test]
        public void Resolve_ManyVariablesOneCombination_ReturnsListOfString()
        {
            List<string> symbols = new List<string>
            {
                "x", "a", "y", "b", "c", "d", "ef", "gh"
            };

            List<IdentifierResolution> combinations = IdentifierResolver.Resolve(symbols, "xy");

            Assert.AreEqual(1, combinations.Count);
            Assert.AreEqual("x, y", combinations[0].ToString());
        }

        [Test]
        public void Resolve_ManyVariablesTwoCombinations_ReturnsListOfString()
        {
            List<string> symbols = new List<string>
            {
                "x", "xy", "y", "b", "c", "d", "ef", "gh"
            };

            List<IdentifierResolution> combinations = IdentifierResolver.Resolve(symbols, "xy");

            Assert.AreEqual(2, combinations.Count);
            Assert.AreEqual("x, y", combinations[0].ToString());
            Assert.AreEqual("xy", combinations[1].ToString());
        }

        [Test]
        public void Resolve_ThreeVariablesOneCombination_ReturnsListOfString()
        {
            List<string> symbols = new List<string>
            {
                "x", "y", "z"
            };

            List<IdentifierResolution> combinations = IdentifierResolver.Resolve(symbols, "xyz");

            Assert.AreEqual(1, combinations.Count);
            Assert.AreEqual("x, y, z", combinations[0].ToString());
        }

        [Test]
        public void Resolve_ManyVariablesThreeCombinations_ReturnsListOfString()
        {
            List<string> symbols = new List<string>
            {
                "x", "xy", "y", "b", "c", "z", "d", "yz", "ef", "gh"
            };

            List<IdentifierResolution> combinations = IdentifierResolver.Resolve(symbols, "xyz");

            Assert.AreEqual(3, combinations.Count);
            Assert.AreEqual("x, y, z", combinations[0].ToString());
            Assert.AreEqual("x, yz", combinations[1].ToString());
            Assert.AreEqual("xy, z", combinations[2].ToString());
        }
    }
}
