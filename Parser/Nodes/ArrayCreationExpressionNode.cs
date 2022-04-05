using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class ArrayCreationExpressionNode : INode
{
    public TokenType Type { get; } = NLangRules.ArrayCreationExpression;

    public TypeExpressionNode ArrayType { get; }
    public List<INode> Dimensions { get; }
    public ArrayInitializerExpressionNode? Initializer { get; }

    public ArrayCreationExpressionNode(INode type, INode dimensions, INode? initializer = null)
    {
        ArrayType = (TypeExpressionNode) type;
        Dimensions = ((ArgumentListNode) dimensions).Args;
        Initializer = (ArrayInitializerExpressionNode?) initializer;
    }
}