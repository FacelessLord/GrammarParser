using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;
using Parser.Utils;

namespace Parser.Nodes;

public class TypeExpressionNode : INode
{
    public TokenType Type { get; } = NLangRules.TypeExpression;
    public string Name;
    public List<TypeExpressionNode> TypeArgs { get; }

    public TypeExpressionNode(INode name, INode? typeArgs = null)
    {
        Name = name.Match();
        TypeArgs = typeArgs is null ? new List<TypeExpressionNode>() : ((TypeExpressionListNode) typeArgs).Types;
    }
}