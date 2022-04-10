using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class VariableInitializerListNode : INode
{

    public TokenType Type { get; } = NLangRules.Variables.VariableInitializerList;
    
    public List<INode> Intializers { get; }
    
    public VariableInitializerListNode(INode firstType)
    {
        Intializers = new List<INode>() { firstType };
    }
    
    public VariableInitializerListNode(INode list, INode nextType)
    {
        var types = ((VariableInitializerListNode) list).Intializers;
        types.Add(nextType);
        Intializers = types;
    }

    protected bool Equals(VariableInitializerListNode other)
    {
        return Type.Equals(other.Type) && Intializers.SequenceEqual(other.Intializers);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((VariableInitializerListNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Intializers);
    }
}