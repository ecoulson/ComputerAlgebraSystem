using System;
namespace ExpressionParser.SyntaxTree
{
    public class FunctionNode : SyntaxNode
    {
        public string Name { get; }
        public SyntaxNode Expression { get; }

        public FunctionNode() : base (SyntaxNodeType.Function)
        {
        }

        public FunctionNode(IdentifierNode name, SyntaxNode expression) : base (SyntaxNodeType.Function)
        {
            Name = name.Value;
        }

        public override string ToString()
        {
            return $"{Left.ToString()}({Right.ToString()})";
        }
    }
}
