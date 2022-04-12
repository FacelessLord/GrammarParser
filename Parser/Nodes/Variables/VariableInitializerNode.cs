using Parser.Grammars.LangGrammar;
using Parser.Grammars.Tokens;

namespace Parser.Nodes.Variables;

public class VariableInitializerNode : INode
{
    public TokenType Type { get; } = NLangRules.Variables.VariableInitializer;

    public INode Initializer { get; }

    public VariableInitializerNode(INode initializer)
    {
        Initializer = initializer;
    }

    protected bool Equals(VariableInitializerNode other)
    {
        return Type.Equals(other.Type) && Initializer.Equals(other.Initializer);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((VariableInitializerNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Initializer);
    }
}