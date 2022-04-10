using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class EmptyStatementNode : INode
{
    public TokenType Type { get; } = NLangRules.Statements.EmptyStatement;

    protected bool Equals(EmptyStatementNode other)
    {
        return Type.Equals(other.Type);
    }
    
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((EmptyStatementNode) obj);
    }
    public override int GetHashCode()
    {
        return Type.GetHashCode();
    }
}