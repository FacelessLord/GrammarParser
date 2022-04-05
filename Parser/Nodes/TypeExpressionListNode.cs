using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class TypeExpressionListNode : INode
{

    public TokenType Type { get; } = NLangRules.TypeExpressionList;

    public List<TypeExpressionNode> Types { get; }
    
    public TypeExpressionListNode(INode firstType)
    {
        Types = new List<TypeExpressionNode>() { (TypeExpressionNode)firstType };
    }
    
    public TypeExpressionListNode(INode list, INode nextType)
    {
        var types = ((TypeExpressionListNode) list).Types;
        types.Add((TypeExpressionNode)nextType);
        Types = types;
    }
}