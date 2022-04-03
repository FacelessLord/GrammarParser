using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class IndexAccessNode : INode
{
    public TokenType Type { get; } = NLangRules.IndexAccess;

    public ExpressionNode Source { get; }
    public ExpressionNode Index { get; }
    
    public IndexAccessNode(INode source, INode index)
    {
        Source = (ExpressionNode) source;
        Index = (ExpressionNode) index;
    }
}