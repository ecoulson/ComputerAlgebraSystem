using System;
namespace ExpressionParser.SyntaxTree
{
    public class FunctionOrDistributionNode : SyntaxNode
    {
        public IdentifierNode Identifier { get; }
        public SyntaxNode Expression { get; }

        public FunctionOrDistributionNode() : base(SyntaxNodeType.AmbigiousFunctionOrShortHandMultiplication)
        {

        }

        public FunctionOrDistributionNode(IdentifierNode identifier, SyntaxNode expression) : base(SyntaxNodeType.AmbigiousFunctionOrShortHandMultiplication)
        {
            Identifier = identifier;
            Expression = expression;
        }

        public override string ToString()
        {
            return $"{Identifier.ToString()}({Expression.ToString()})";
        }

        public override SyntaxNode Copy()
        {
            return new FunctionOrDistributionNode((IdentifierNode)Identifier.Copy(), Expression.Copy());
        }
    }
}
