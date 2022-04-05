using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class ArgumentListNode : INode
{
    public TokenType Type { get; } = NLangRules.ArgumentListExpression;

    public List<INode> Args { get; }

    public ArgumentListNode(INode firstType)
    {
        Args = new List<INode>() { firstType };
    }

    public ArgumentListNode(INode nextType, INode? list = null)
    {
        var types = list is null ? new List<INode>() : ((ArgumentListNode) list).Args;
        types.Add((ArgumentListNode) nextType);
        Args = types;
    }
}