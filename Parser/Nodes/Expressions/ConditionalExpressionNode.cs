using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class ConditionalExpressionNode : INode
{
    public TokenType Type { get; } = NLangRules.Expressions.ConditionalExpression;
    public INode Condition { get; }
    public INode True { get; }
    public INode False { get; }

    public ConditionalExpressionNode(INode condition, INode @true, INode @false)
    {
        Condition = condition;
        True = @true;
        False = @false;
    }

    protected bool Equals(ConditionalExpressionNode other)
    {
        return Type.Equals(other.Type) && Condition.Equals(other.Condition) && True.Equals(other.True) && False.Equals(other.False);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((ConditionalExpressionNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Condition, True, False);
    }
}