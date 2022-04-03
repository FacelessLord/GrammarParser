using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class ExpressionListNode : INode
{
    public TokenType Type { get; } = NLangRules.ExpressionList;

    public List<ExpressionNode> Args;

    public ExpressionListNode(INode prev, INode next)
    {
        if (prev is ExpressionListNode list)
        {
            Args = list.Args;
            Args.Add((ExpressionNode) next);
        }
        else
        {
            Args = new List<ExpressionNode>
            {
                (ExpressionNode) prev,
                (ExpressionNode) next
            };
        }
    }

    public ExpressionListNode(INode single)
    {
        Args = new List<ExpressionNode>
        {
            (ExpressionNode) single,
        };
    }
}