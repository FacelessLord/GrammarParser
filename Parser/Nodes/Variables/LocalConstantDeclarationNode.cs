using Parser.Grammars.LangGrammar;
using Parser.Grammars.Tokens;

namespace Parser.Nodes.Variables;

public class LocalConstantDeclarationNode : INode
{

    public TokenType Type { get; } = NLangRules.Variables.LocalConstantDeclaration;
    
    public INode TypeDeclaration { get; }
    public INode Declarators { get; }

    public LocalConstantDeclarationNode(INode typeDeclaration, INode declarators)
    {
        TypeDeclaration = typeDeclaration;
        Declarators = declarators;
    }

    protected bool Equals(LocalConstantDeclarationNode other)
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
        return Equals((LocalConstantDeclarationNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, TypeDeclaration, Declarators);
    }
}