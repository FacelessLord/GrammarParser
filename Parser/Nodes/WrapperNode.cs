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

    protected bool Equals(WrapperNode other)
    {
        return Type.Equals(other.Type) && Node.Equals(other.Node);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((WrapperNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Node);
    }
}