using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class StatementNode : INode
{
    public TokenType Type { get; } = NLangRules.Statement;

    public ExpressionNode Statement { get; }

    public StatementNode(INode node)
    {
        Statement = (ExpressionNode) node;
    }
}