using ExpressionParser.Parsing;
using NUnit.Framework;

namespace ExpressionParser.Tests.ParserTests
{
    [TestFixture]
    public class EnvironmentTests
    {
        [Test]
        public void GetVariable_Symbol_ReturnsEnvironmentVariable()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");

            EnvironmentVariable variable = environment.Get("x");

            Assert.AreEqual(EnvironmentVariableType.Symbol, variable.Type);
        }

        [Test]
        public void GetVariable_Number_ReturnsEnvironmentVariable()
        {
            Environment environment = new Environment();
            environment.AddValue("x", 1);

            EnvironmentVariable variable = environment.Get("x");

            Assert.AreEqual(EnvironmentVariableType.Number, variable.Type);
            Assert.AreEqual(1, variable.Value);
        }

        [Test]
        public void GetVariable_Function_ReturnsEnvironmentVariable()
        {
            Environment environment = new Environment();
            environment.AddFunction("x", new Expression(null));

            EnvironmentVariable variable = environment.Get("x");

            Assert.AreEqual(EnvironmentVariableType.Function, variable.Type);
        }

        [Test]
        public void GetVariable_Function_ThrowsException()
        {
            Environment environment = new Environment();

            Assert.Throws<System.ArgumentException>(() => { 
                EnvironmentVariable variable = environment.Get("x");
            });
        }

        [Test]
        public void HasVariable_SymbolExists_ReturnsTrue()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");

            Assert.True(environment.HasVariable("x"));
        }

        [Test]
        public void HasVariable_SymbolDoesNotExists_ReturnsFalse()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");

            Assert.True(environment.HasVariable("x"));
        }

        [Test]
        public void IsPredefinedFunction_SinFunction_ReturnsTrue()
        {
            Environment environment = new Environment();

            Assert.True(environment.IsPredefinedFunction("sin"));
        }

        [Test]
        public void IsPredefinedFunction_AFunction_ReturnsFalse()
        {
            Environment environment = new Environment();

            Assert.False(environment.IsPredefinedFunction("x"));
        }

        [Test]
        public void IsPredefinedSymbol_PI_ReturnsTrue()
        {
            Environment environment = new Environment();

            Assert.True(environment.IsPredefinedSymbol("pi"));
        }

        [Test]
        public void IsPredefinedSymbol_ASymbol_ReturnsFalse()
        {
            Environment environment = new Environment();

            Assert.False(environment.IsPredefinedSymbol("x"));
        }

        [Test]
        public void IsKeyword_SinFunction_ReturnsTrue()
        {
            Environment environment = new Environment();

            Assert.True(environment.IsKeyword("sin"));
        }

        [Test]
        public void IsKeyword_PI_ReturnsTrue()
        {
            Environment environment = new Environment();

            Assert.True(environment.IsKeyword("pi"));
        }

        [Test]
        public void IsKeyword_PI_ReturnsFalse()
        {
            Environment environment = new Environment();

            Assert.False(environment.IsKeyword("x"));
        }

        [Test]
        public void AddSymbol_AlreadyDefinedSymbol_ThrowsException()
        {
            Environment environment = new Environment();
            environment.AddSymbol("x");

            DefinedSymbolException exception = Assert.Throws<DefinedSymbolException>(() => {
                environment.AddSymbol("x");
            });

            Assert.AreEqual("Symbol x has already been defined", exception.Message);
        }

        [Test]
        public void AddValue_AlreadyDefinedSymbol_ThrowsException()
        {
            Environment environment = new Environment();
            environment.AddValue("x", 2);

            DefinedSymbolException exception = Assert.Throws<DefinedSymbolException>(() => {
                environment.AddSymbol("x");
            });

            Assert.AreEqual("Symbol x has already been defined", exception.Message);
        }

        [Test]
        public void AddFunction_AlreadyDefinedSymbol_ThrowsException()
        {
            Environment environment = new Environment();

            DefinedSymbolException exception = Assert.Throws<DefinedSymbolException>(() => {
                environment.AddFunction("sin", null);
            });

            Assert.AreEqual("Symbol sin has already been defined", exception.Message);
        }
    }
}
