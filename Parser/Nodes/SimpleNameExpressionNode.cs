using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;
using Parser.Utils;

namespace Parser.Nodes;

public class SimpleNameExpressionNode : INode
{
    public TokenType Type { get; } = NLangRules.SimpleNameExpression;

    public string Name { get; }
    public List<TypeExpressionNode> TypeArgs { get; }

    public SimpleNameExpressionNode(INode name, INode? typeArgs = null)
    {
        Name = name.Match();
        TypeArgs = typeArgs is null ? new List<TypeExpressionNode>() : ((TypeExpressionListNode) typeArgs).Types;
    }
}