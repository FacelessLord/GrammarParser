using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;
using Parser.Utils;

namespace Parser.Nodes;

public class LocalVariableDeclaratorNode : INode
{
    public TokenType Type { get; } = NLangRules.Variables.LocalVariableDeclarator;

    public string Name { get; }
    public INode? Initializer { get; }

    public LocalVariableDeclaratorNode(INode name, INode? initializer = null)
    {
        Name = name.Match();
        Initializer = initializer;
    }

    protected bool Equals(LocalVariableDeclaratorNode other)
    {
        return Type.Equals(other.Type) && Name == other.Name && Equals(Initializer, other.Initializer);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((LocalVariableDeclaratorNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Name, Initializer);
    }
}