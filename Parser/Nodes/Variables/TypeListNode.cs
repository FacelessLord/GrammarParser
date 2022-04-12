using Parser.Grammars.LangGrammar;
using Parser.Grammars.Tokens;

namespace Parser.Nodes.Variables;

public class TypeListNode : INode
{

    public TokenType Type { get; } = NLangRules.Variables.TypeList;

    public List<TypeNode> Types { get; }
    
    public TypeListNode(INode firstType)
    {
        Types = new List<TypeNode>() { (TypeNode)firstType };
    }
    
    public TypeListNode(INode list, INode nextType)
    {
        var types = ((TypeListNode) list).Types;
        types.Add((TypeNode)nextType);
        Types = types;
    }

    protected bool Equals(TypeListNode other)
    {
        return Type.Equals(other.Type) && Types.SequenceEqual(other.Types);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((TypeListNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Types);
    }
}