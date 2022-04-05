using Parser.Grammars.tokens;

namespace Parser.Nodes;

public abstract class ExpressionNode : INode
{
    public TokenType Type { get; set; }
}