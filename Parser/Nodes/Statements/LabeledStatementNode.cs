using Parser.Grammars.LangGrammar;
using Parser.Grammars.Tokens;
using Parser.Utils;

namespace Parser.Nodes.Statements;

public class LabeledStatementNode : INode
{

    public TokenType Type { get; } = NLangRules.Statements.LabeledStatement;

    public INode Statement { get; }
    public string Label { get; }

    public LabeledStatementNode(INode label, INode statement)
    {
        Statement = statement;
        Label = label.Match();
    }

    protected bool Equals(LabeledStatementNode other)
    {
        return Type.Equals(other.Type) && Statement.Equals(other.Statement) && Label == other.Label;
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((LabeledStatementNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Statement, Label);
    }
}