using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class WrapperNode : INode
{
    public TokenType Type { get; }
    public INode Node { get; }

    public WrapperNode(TokenType type, INode node)
    {
        Type = type;
        Node = node;
    }
}