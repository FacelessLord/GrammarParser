using Parser.Grammars.Tokens;
using Parser.Utils;

namespace Parser.Nodes.Statements;

public class JumpStatementNode : INode
{
    public TokenType Type { get; }
    
    public INode? Expression { get; }
    public string? TargetLabel { get; }

    public JumpStatementNode(TokenType type, INode? expression = null, INode? targetLabel = null)
    {
        Type = type;
        Expression = expression;
        TargetLabel = targetLabel?.Match();
    }

    protected bool Equals(JumpStatementNode other)
    {
        return Type.Equals(other.Type) && Equals(Expression, other.Expression) && TargetLabel == other.TargetLabel;
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((JumpStatementNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Expression, TargetLabel);
    }
}