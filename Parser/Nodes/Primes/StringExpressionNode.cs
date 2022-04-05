using Parser.Grammars.tokens;

namespace Parser.Nodes.Primes;

public class StringExpressionNode : LiteralExpressionNode
{
    public string Token { get; }

    public StringExpressionNode(INode id)
    {
        Token = ((TerminalNode) id).Match;
    }
}