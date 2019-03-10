using System;
using ExpressionParser.Parser;
using ExpressionParser;
using Input;
using Mathematics;

namespace EulerTangentApproximations
{
    class EulerTangentApproximationClient
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter a mathematical expression for the differential equation");
            Expression slopeExpression = Parser.ParseExpression(In.GetString());

            Console.WriteLine("Enter an initial point in the form: x,y");
            Point initialCondition = GetPoint();

            Console.WriteLine("Enter a value to estimate");
            double toEstimate = In.GetDouble();

            Console.WriteLine("Get the number of steps to calculate, or enter -1 if you want the best approximation");
            int steps = In.GetInt();

            Console.WriteLine($"Estimated value is : {Estimate(slopeExpression, initialCondition, toEstimate, steps)}");
        }

        private static Point GetPoint()
        {
            string rawPoint = In.GetString();
            string[] rawComponents = rawPoint.Split(',');
            double xComponent = double.Parse(rawComponents[0]);
            double yComponent = double.Parse(rawComponents[1]);
            return new Point(xComponent, yComponent);
        }

        public static double Estimate(Expression slopeExpression, Point initialCondition, double toEstimate, int steps)
        {
            double deltaX = getDeltaX(initialCondition, toEstimate, steps);
            double x = initialCondition.getX();
            double y = initialCondition.getY();
            while(x < toEstimate)
            {
                double slope = slopeExpression.Evaluate(x);
                y = slope * (x + deltaX - x) + y;
                x += deltaX;
            }
            return y;
        }

        private static double getDeltaX(Point initialCondition, double toEstimate, int steps)
        {
            if (steps == -1)
            {
                return double.Epsilon;
            }
            return (toEstimate - initialCondition.getX()) / steps;
        }
    }
}
