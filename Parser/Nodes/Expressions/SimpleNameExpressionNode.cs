using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;
using Parser.Utils;

namespace Parser.Nodes;

public class SimpleNameExpressionNode : INode
{
    public TokenType Type { get; } = NLangRules.Expressions.SimpleNameExpression;

    public string Name { get; }
    public List<TypeNode> TypeArgs { get; }

    public SimpleNameExpressionNode(INode name, INode? typeArgs = null)
    {
        Name = name.Match();
        TypeArgs = typeArgs is null ? new List<TypeNode>() : ((TypeListNode) typeArgs).Types;
    }

    protected bool Equals(SimpleNameExpressionNode other)
    {
        return Type.Equals(other.Type) && Name == other.Name && TypeArgs.SequenceEqual(other.TypeArgs);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((SimpleNameExpressionNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Name, TypeArgs);
    }
}