using Parser.Grammars.LangGrammar;
using Parser.Grammars.Tokens;

namespace Parser.Nodes.Statements;

public class WhileStatementNode : INode
{
    public TokenType Type { get; } = NLangRules.Statements.WhileStatement;

    public INode Condition { get; }
    public INode Statement { get; }

    public WhileStatementNode(INode condition, INode statement)
    {
        Condition = condition;
        Statement = statement;
    }

    protected bool Equals(WhileStatementNode other)
    {
        return Type.Equals(other.Type) && Condition.Equals(other.Condition) && Statement.Equals(other.Statement);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((WhileStatementNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Condition, Statement);
    }
}