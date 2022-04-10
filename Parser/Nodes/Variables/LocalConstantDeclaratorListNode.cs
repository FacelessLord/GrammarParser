using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class LocalConstantDeclaratorListNode : INode
{
    public TokenType Type { get; } = NLangRules.Variables.LocalConstantDeclaratorList;

    public List<INode> ConstantDeclarators { get; }

    public LocalConstantDeclaratorListNode(INode firstType)
    {
        ConstantDeclarators = new List<INode>() { firstType };
    }

    public LocalConstantDeclaratorListNode(INode list, INode nextType)
    {
        var declarators = ((LocalConstantDeclaratorListNode) list).ConstantDeclarators;
        declarators.Add(nextType);
        ConstantDeclarators = declarators;
    }

    protected bool Equals(LocalConstantDeclaratorListNode other)
    {
        return Type.Equals(other.Type) && ConstantDeclarators.SequenceEqual(other.ConstantDeclarators);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((LocalConstantDeclaratorListNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, ConstantDeclarators);
    }
}