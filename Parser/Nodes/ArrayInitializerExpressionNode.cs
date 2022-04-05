using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class ArrayInitializerExpressionNode : INode
{
    public TokenType Type { get; } = NLangRules.ArrayInitializerExpression;
    
    public List<INode> Initializers { get; }

    public ArrayInitializerExpressionNode(INode? initializers = null)
    {
        Initializers = initializers is null ? new List<INode>() : ((VariableIntializerListExpressionNode) initializers).Intializers;
    }
    
}