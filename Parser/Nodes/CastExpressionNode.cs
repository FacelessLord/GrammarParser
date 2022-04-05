using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class CastExpressionNode : INode
{
    public TokenType Type { get; } = NLangRules.CastExpression;
    public TypeExpressionNode TargetType { get; }
    public INode Target { get; }

    public CastExpressionNode(INode targetType, INode target)
    {
        TargetType = (TypeExpressionNode) targetType;
        Target = target;
    }
}