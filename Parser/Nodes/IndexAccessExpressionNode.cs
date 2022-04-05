using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class IndexAccessExpressionNode : INode
{
    public TokenType Type { get; } = NLangRules.IndexAccessExpression;
    
    public INode Source { get; }
    public List<INode> Index { get; }

    public IndexAccessExpressionNode(INode source, INode? index=null)
    {
        Source = source;
        Index = index is null ? new List<INode>() : ((ArgumentListNode) index).Args;
    }
}