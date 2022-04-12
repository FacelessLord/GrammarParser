using Parser.Grammars.LangGrammar;
using Parser.Grammars.Tokens;

namespace Parser.Nodes.Variables;

public class LocalVariableDeclarationNode : INode
{

    public TokenType Type { get; } = NLangRules.Variables.LocalVariableDeclaration;
    
    public INode TypeDeclaration { get; }
    public INode Declarators { get; }

    public LocalVariableDeclarationNode(INode typeDeclaration, INode declarators)
    {
        TypeDeclaration = typeDeclaration;
        Declarators = declarators;
    }

    protected bool Equals(LocalVariableDeclarationNode other)
    {
        return Type.Equals(other.Type) && TypeDeclaration.Equals(other.TypeDeclaration) && Declarators.Equals(other.Declarators);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((LocalVariableDeclarationNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, TypeDeclaration, Declarators);
    }
}