using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;
using Parser.Utils;

namespace Parser.Nodes;

public class MemberAccessExpressionNode : INode
{
    public TokenType Type { get; } = NLangRules.TypeExpression;
    public INode Source { get; }
    public string Name;
    public List<TypeExpressionNode> TypeArgs { get; }

    public MemberAccessExpressionNode(INode source, INode name, INode? typeArgs = null)
    {
        Source = source;
        Name = name.Match();
        TypeArgs = typeArgs is null ? new List<TypeExpressionNode>() : ((TypeExpressionListNode) typeArgs).Types;
    }
}