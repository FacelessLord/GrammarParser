using Parser.Grammars.Tokens;

namespace Parser.Nodes.Primes;

public class StringExpressionNode : LiteralExpressionNode
{
    public string Token { get; }

    public StringExpressionNode(INode id)
    {
        Token = ((TerminalNode) id).Match;
    }
    protected bool Equals(StringExpressionNode other)
    {
        return Token == other.Token;
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((StringExpressionNode) obj);
    }
    public override int GetHashCode()
    {
        return Token.GetHashCode();
    }
}