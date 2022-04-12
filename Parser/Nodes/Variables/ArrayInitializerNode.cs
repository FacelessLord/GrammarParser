using Parser.Grammars.LangGrammar;
using Parser.Grammars.Tokens;

namespace Parser.Nodes.Variables;

public class ArrayInitializerNode : INode
{
    public TokenType Type { get; } = NLangRules.Variables.ArrayInitializer;
    
    public List<INode> Initializers { get; }

    public ArrayInitializerNode(INode? initializers = null)
    {
        Initializers = initializers is null ? new List<INode>() : ((VariableInitializerListNode) initializers).Intializers;
    }

    protected bool Equals(ArrayInitializerNode other)
    {
        return Type.Equals(other.Type) && Initializers.SequenceEqual(other.Initializers);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((ArrayInitializerNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Initializers);
    }
}