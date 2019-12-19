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
            Console.WriteLine("This is my derivative calculator which will incorporate multivariable derivatives eventually. This ended up being more work than expected. The derivatives can be verified by the expression trees that are printed after the derivatives are printed. Note: printed derivatives are missing parentheses.\n");
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
                Console.WriteLine("Expression Tree:");
                PrintTree(derivative.Derive().Tree, "", true);
                Console.WriteLine();
            }
        }

        public static void PrintTree(SyntaxNode tree, string indent, bool last)
        {
            if (tree.Type == SyntaxNodeType.Operator)
            {
                Console.WriteLine(indent + "+- " + ((OperatorNode)tree).Operator);
            }
            else
            {
                Console.WriteLine(indent + "+- " + tree);
            }
            indent += last ? "   " : "|  ";

            if (tree.Left != null)
            {
                PrintTree(tree.Left, indent, false);
            }
            if (tree.Right != null)
            {
                PrintTree(tree.Right, indent, true);
            }
        }
    }
}
