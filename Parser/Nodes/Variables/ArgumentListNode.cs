using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class ArgumentListNode : INode
{
    public TokenType Type { get; } = NLangRules.Variables.ArgumentList;

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

    protected bool Equals(ArgumentListNode other)
    {
        return Type.Equals(other.Type) && Args.SequenceEqual(other.Args);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((ArgumentListNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Args);
    }
}