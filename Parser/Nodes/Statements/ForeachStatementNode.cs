using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;
using Parser.Utils;

namespace Parser.Nodes.Statements;

public class ForeachStatementNode : INode
{
    public TokenType Type { get; } = NLangRules.Statements.ForeachStatement;

    public INode VariableType { get; }
    public string VariableName { get; }
    public INode Source { get; }
    public INode Statement { get; }

    public ForeachStatementNode(INode variableType, INode variableName, INode source, INode statement)
    {
        VariableType = variableType;
        VariableName = variableName.Match();
        Source = source;
        Statement = statement;
    }

    protected bool Equals(ForeachStatementNode other)
    {
        return VariableType.Equals(other.VariableType) &&
               VariableName == other.VariableName &&
               Source.Equals(other.Source) &&
               Statement.Equals(other.Statement);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((ForeachStatementNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, VariableType, VariableName, Source, Statement);
    }
}