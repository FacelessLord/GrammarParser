using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;
using Parser.Utils;

namespace Parser.Nodes;

public class MemberAccessExpressionNode : INode
{
    public TokenType Type { get; } = NLangRules.Expressions.MemberAccessExpression;
    public INode Source { get; }
    public string Name;
    public List<TypeNode> TypeArgs { get; }

    public MemberAccessExpressionNode(INode source, INode name, INode? typeArgs = null)
    {
        Source = source;
        Name = name.Match();
        TypeArgs = typeArgs is null ? new List<TypeNode>() : ((TypeListNode) typeArgs).Types;
    }

    protected bool Equals(MemberAccessExpressionNode other)
    {
        return Name == other.Name && Type.Equals(other.Type) && Source.Equals(other.Source) && TypeArgs.SequenceEqual(other.TypeArgs);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((MemberAccessExpressionNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Type, Source, TypeArgs);
    }
}