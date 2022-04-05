using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class UnaryExpressionNode : INode
{
    public TokenType Type { get; } = NLangRules.UnaryExpression;
    public Operation Operation { get; }
    public INode Target { get; }

    public UnaryExpressionNode(INode operation, INode target)
    {
        Target = target;
        Operation = ((OperationTerminalType) operation.Type).Operation;
    }
}