using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class ExpressionNode : INode
{
    public TokenType Type { get; } = NLangRules.Expression;

    public List<INode> Nodes { get; }

    public ExpressionNode(INode node)
    {
        if (node is ExpressionNode expr)
        {
            Nodes = new List<INode> { ExpressionTreeNode.BuildNode(expr.Nodes) };
        }
        else
        {
            Nodes = new List<INode> { node };
        }
    }
    public ExpressionNode(INode[] args)
    {
        Nodes = new List<INode>();
        foreach (var node in args)
        {
            if (node is ExpressionNode expr)
            {
                Nodes.AddRange(expr.Nodes);
            }
            else
                Nodes.Add(node);
        }
    }
}