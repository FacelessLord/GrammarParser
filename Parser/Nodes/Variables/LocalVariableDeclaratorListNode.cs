using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class LocalVariableDeclaratorListNode : INode
{
    public TokenType Type { get; } = NLangRules.Variables.LocalVariableDeclaratorList;

    public List<INode> VariableDeclarators { get; }

    public LocalVariableDeclaratorListNode(INode firstType)
    {
        VariableDeclarators = new List<INode>() { firstType };
    }

    public LocalVariableDeclaratorListNode(INode list, INode nextType)
    {
        var declarators = ((LocalVariableDeclaratorListNode) list).VariableDeclarators;
        declarators.Add(nextType);
        VariableDeclarators = declarators;
    }

    protected bool Equals(LocalVariableDeclaratorListNode other)
    {
        return Type.Equals(other.Type) && VariableDeclarators.SequenceEqual(other.VariableDeclarators);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((LocalVariableDeclaratorListNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, VariableDeclarators);
    }
}