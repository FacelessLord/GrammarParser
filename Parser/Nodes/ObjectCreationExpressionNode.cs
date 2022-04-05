using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class ObjectCreationExpressionNode : INode
{

    public TokenType Type { get; } = NLangRules.ObjectCreationExpression;

    public TypeExpressionNode ObjectType { get; }
    public List<INode> Args { get; }
    
    public ObjectCreationExpressionNode(INode type, INode? args = null)
    {
        ObjectType = (TypeExpressionNode) type;
        Args = args is null ? new List<INode>() : ((ArgumentListNode) args).Args;
    }
}