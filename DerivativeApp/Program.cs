using System;
using ExpressionParser.Parsing;
using ExpressionParser.SyntaxTree;
using Mathematics;
using Mathematics.Calculus;

namespace DerivativeApp
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Enter an expression to derive");
                string rawExpression = Console.ReadLine();
                ExpressionParser.Parsing.Environment environment = new ExpressionParser.Parsing.Environment();
                environment.AddSymbol("x");
                SyntaxNode node = Parser.ParseExpression(rawExpression, environment);
                Expression expression = new Expression(node, environment);
                Derivative derivative = new Derivative(expression, "x");
                Console.WriteLine(derivative.Derive());
                Console.WriteLine();
            }
        }
    }
}
