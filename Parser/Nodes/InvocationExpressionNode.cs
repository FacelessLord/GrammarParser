using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class InvocationExpressionNode : INode
{
    public TokenType Type { get; } = NLangRules.InvocationExpression;

    public INode Source { get; }

    public List<INode> Args { get;}
    
    public InvocationExpressionNode(INode source, INode? args = null)
    {
        Source = source;
        Args = args is null ? new List<INode>() : ((ArgumentListNode) args).Args;
    }
}