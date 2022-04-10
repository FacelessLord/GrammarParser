using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes.Statements;

public class IfStatementNode : INode
{
    public TokenType Type { get; } = NLangRules.Statements.IfStatement;
    
    public INode Condition { get; }
    public INode Statement { get; }
    public INode? ElseStatement { get; }

    public IfStatementNode(INode condition, INode statement, INode? elseStatement = null)
    {
        Condition = condition;
        Statement = statement;
        ElseStatement = elseStatement;
    }
    protected bool Equals(IfStatementNode other)
    {
        return Type.Equals(other.Type) && Condition.Equals(other.Condition) && Statement.Equals(other.Statement) && Equals(ElseStatement, other.ElseStatement);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((IfStatementNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Condition, Statement, ElseStatement);
    }
}