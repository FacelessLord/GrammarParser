using Parser.Grammars.LangGrammar;
using Parser.Grammars.Tokens;
using Parser.Utils;

namespace Parser.Nodes.Variables;

public class TypeNode : INode
{
    public TokenType Type { get; } = NLangRules.Variables.Type;
    public string Name;
    public List<TypeNode> TypeArgs { get; }

    public TypeNode(INode name, INode? typeArgs = null)
    {
        Name = name.Match();
        TypeArgs = typeArgs is null ? new List<TypeNode>() : ((TypeListNode) typeArgs).Types;
    }

    protected bool Equals(TypeNode other)
    {
        return Name == other.Name && Type.Equals(other.Type) && TypeArgs.SequenceEqual(other.TypeArgs);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((TypeNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Type, TypeArgs);
    }
}