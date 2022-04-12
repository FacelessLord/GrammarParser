using Parser.Grammars.Tokens;

namespace Parser.Nodes.Expressions;

public abstract class ExpressionNode : INode
{
    public TokenType Type { get; set; }
}