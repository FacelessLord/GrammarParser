using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes.Statements;

public class ForStatementNode : INode
{
    public TokenType Type { get; } = NLangRules.Statements.ForStatement;

    public INode? Initializer { get; }
    public INode? Condition { get; }
    public INode? Iterator { get; }
    public INode Statement { get; }

    public ForStatementNode(INode statement, INode? initializer = null, INode? condition = null, INode? iterator = null)
    {
        Statement = statement;
        Initializer = initializer;
        Condition = condition;
        Iterator = iterator;
    }

    protected bool Equals(ForStatementNode other)
    {
        return Type.Equals(other.Type) && Equals(Initializer, other.Initializer) && Equals(Condition, other.Condition) && Equals(Iterator, other.Iterator) && Statement.Equals(other.Statement);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((ForStatementNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Initializer, Condition, Iterator, Statement);
    }
}