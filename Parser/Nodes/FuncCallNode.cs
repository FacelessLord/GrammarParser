using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class FuncCallNode : INode
{
    public TokenType Type { get; } = NLangRules.FuncCall;

    public ExpressionNode Source { get; }
    public ExpressionListNode Args { get; }
    
    public FuncCallNode(INode source, INode args)
    {
        Source = (ExpressionNode) source;
        if (args is ExpressionListNode list)
            Args = list;
        else
            Args = new ExpressionListNode(args);
    }
}