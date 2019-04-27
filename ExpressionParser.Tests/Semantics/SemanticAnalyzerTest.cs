using ExpressionParser.Lex;
using ExpressionParser.Parser;
using ExpressionParser.Semantics;
using ExpressionParser.SyntaxTree;
using NUnit.Framework;

namespace ExpressionParser.Tests.Semantics
{
    [TestFixture]
    public class SemanticAnalyzerTest
    {
        [Test]
        public void Analyze_Identifier_ThrowsUndefinedIdentifier()
        {
            SyntaxNode node = new IdentifierNode(new Token(TokenType.Identifier, "x"));
            Environment environment = new Environment();

            UndefinedSymbolException exception = Assert.Throws<UndefinedSymbolException>(() =>
            {
                SemanticAnalyzer.Analyze(node, environment);
            });

            Assert.NotNull(exception);
            Assert.AreEqual("Undefined symbol 'x'", exception.Message);
        }
    }
}
