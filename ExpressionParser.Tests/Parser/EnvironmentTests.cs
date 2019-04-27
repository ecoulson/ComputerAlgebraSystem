
using ExpressionParser.Parser;
using NUnit.Framework;

namespace ExpressionParser.Tests.Parser
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
            environment.AddFunction("x", new Expression());

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
    }
}
