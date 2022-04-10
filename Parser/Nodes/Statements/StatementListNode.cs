using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class StatementListNode : INode
{

    public TokenType Type { get; } = NLangRules.Statements.StatementList;

    public List<INode> Statements { get; }

    public StatementListNode(INode first)
    {
        Statements = new List<INode>() { first };
    }

    public StatementListNode(INode list, INode nextType)
    {
        var statements = ((StatementListNode) list).Statements;
        statements.Add(nextType);
        Statements = statements;
    }

    protected bool Equals(StatementListNode other)
    {
        return Type.Equals(other.Type) && Statements.SequenceEqual(other.Statements);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((StatementListNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Statements);
    }
}