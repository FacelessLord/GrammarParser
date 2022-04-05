using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class VariableIntializerExpressionNode : INode
{
    public TokenType Type { get; } = NLangRules.VariableIntializerExpression;

    public INode Initializer { get; }

    public VariableIntializerExpressionNode(INode initializer)
    {
        Initializer = initializer;
    }
}