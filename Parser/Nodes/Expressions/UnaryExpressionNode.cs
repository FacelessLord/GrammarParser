using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class UnaryExpressionNode : INode
{
    public TokenType Type { get; } = NLangRules.Expressions.UnaryExpression;
    public Operation Operation { get; }
    public INode Target { get; }

    public UnaryExpressionNode(INode operation, INode target)
    {
        Target = target;
        Operation = ((OperationTerminalType) operation.Type).Operation;
    }

    protected bool Equals(UnaryExpressionNode other)
    {
        return Type.Equals(other.Type) && Operation == other.Operation && Target.Equals(other.Target);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((UnaryExpressionNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, (int) Operation, Target);
    }
}