using Parser.Grammars.LangGrammar;
using Parser.Grammars.Tokens;
using Parser.Utils;

namespace Parser.Nodes.Variables;

public class LocalConstantDeclaratorNode : INode
{
    public TokenType Type { get; } = NLangRules.Variables.LocalConstantDeclarator;

    public string Name { get; }
    public INode Initializer { get; }

    public LocalConstantDeclaratorNode(INode name, INode initializer)
    {
        Name = name.Match();
        Initializer = initializer;
    }

    protected bool Equals(LocalConstantDeclaratorNode other)
    {
        return Type.Equals(other.Type) && Name == other.Name && Initializer.Equals(other.Initializer);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((LocalConstantDeclaratorNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Name, Initializer);
    }
}