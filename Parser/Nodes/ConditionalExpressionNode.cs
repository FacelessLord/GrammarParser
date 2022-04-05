using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class ConditionalExpressionNode : INode
{
    public TokenType Type { get; } = NLangRules.ConditionalExpression;
    public INode Condition { get; }
    public INode True { get; }
    public INode False { get; }

    public ConditionalExpressionNode(INode condition, INode @true, INode @false)
    {
        Condition = condition;
        True = @true;
        False = @false;
    }
}