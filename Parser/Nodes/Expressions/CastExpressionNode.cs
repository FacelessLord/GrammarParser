using Parser.Grammars.LangGrammar;
using Parser.Grammars.Tokens;
using Parser.Nodes.Variables;

namespace Parser.Nodes.Expressions;

public class CastExpressionNode : INode
{
    public TokenType Type { get; } = NLangRules.Expressions.CastExpression;
    public TypeNode TargetType { get; }
    public INode Target { get; }

    public CastExpressionNode(INode targetType, INode target)
    {
        TargetType = (TypeNode) targetType;
        Target = target;
    }

    protected bool Equals(CastExpressionNode other)
    {
        return Type.Equals(other.Type) && TargetType.Equals(other.TargetType) && Target.Equals(other.Target);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((CastExpressionNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, TargetType, Target);
    }
}