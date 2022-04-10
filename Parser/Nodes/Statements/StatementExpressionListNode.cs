using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class StatementExpressionListNode : INode
{
    public TokenType Type { get; } = NLangRules.Statements.StatementExpressionList;

    public List<INode> Statements { get; }

    public StatementExpressionListNode(INode firstType)
    {
        Statements = new List<INode>() { firstType };
    }

    public StatementExpressionListNode(INode list, INode nextType)
    {
        var statements = ((StatementExpressionListNode) list).Statements;
        statements.Add(nextType);
        Statements = statements;
    }
    protected bool Equals(StatementExpressionListNode other)
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
        return Equals((StatementExpressionListNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Statements);
    }
}