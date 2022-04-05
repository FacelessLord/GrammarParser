using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class VariableIntializerListExpressionNode : INode
{

    public TokenType Type { get; } = NLangRules.VariableIntializerListExpression;
    
    public List<INode> Intializers { get; }
    
    public VariableIntializerListExpressionNode(INode firstType)
    {
        Intializers = new List<INode>() { firstType };
    }
    
    public VariableIntializerListExpressionNode(INode list, INode nextType)
    {
        var types = ((VariableIntializerListExpressionNode) list).Intializers;
        types.Add(nextType);
        Intializers = types;
    }
}