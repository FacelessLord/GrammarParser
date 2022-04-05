using Parser.Grammars.tokens;

namespace Parser.Nodes.Primes;

public class NumberExpressionNode : LiteralExpressionNode
{
    public string Number { get; }

    public NumberExpressionNode(INode id)
    {
        Number = ((TerminalNode) id).Match;
    }
}