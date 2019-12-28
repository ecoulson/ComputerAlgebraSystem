using System.Collections.Generic;

namespace ExpressionParser.SyntaxTree
{
    public class FunctionNode : SyntaxNode
    {
        public IdentifierNode Name { get; set; }
        public List<SyntaxNode> Arguments { get; set; }

        public FunctionNode() : base (SyntaxNodeType.Function)
        {
            Arguments = new List<SyntaxNode>();
        }

        public FunctionNode(IdentifierNode name, List<SyntaxNode> arguments) : base (SyntaxNodeType.Function)
        {
            Name = name;
            Arguments = arguments;
        }

        public override string ToString()
        {
            string argumentsList = "";
            foreach(SyntaxNode argument in Arguments)
            {
                argumentsList += $"{argument.ToString()}, ";
            }
            argumentsList = argumentsList.Substring(0, argumentsList.Length - 2);
            return $"{Name.ToString()}({argumentsList})";
        }

        public override SyntaxNode Copy()
        {
            List<SyntaxNode> copiedArguments = new List<SyntaxNode>();
            foreach (SyntaxNode argument in Arguments)
            {
                copiedArguments.Add(argument.Copy());
            }
            return new FunctionNode((IdentifierNode)Name.Copy(), copiedArguments);
        }
    }
}
