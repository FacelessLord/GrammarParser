using Parser.Grammars.Tokens;

namespace Parser.Nodes.Primes;

public class NumberExpressionNode : LiteralExpressionNode
{
    public string Number { get; }

    public NumberExpressionNode(INode id)
    {
        Number = ((TerminalNode) id).Match;
    }

    protected bool Equals(NumberExpressionNode other)
    {
        return Number == other.Number;
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((NumberExpressionNode) obj);
    }
    public override int GetHashCode()
    {
        return Number.GetHashCode();
    }
}