using Parser.Grammars.LangGrammar;
using Parser.Grammars.Tokens;
using Parser.Nodes.Variables;

namespace Parser.Nodes.Expressions;

public class InvocationExpressionNode : INode
{
    public TokenType Type { get; } = NLangRules.Expressions.InvocationExpression;

    public INode Source { get; }

    public List<INode> Args { get;}
    
    public InvocationExpressionNode(INode source, INode? args = null)
    {
        Source = source;
        Args = args is null ? new List<INode>() : ((ArgumentListNode) args).Args;
    }

    protected bool Equals(InvocationExpressionNode other)
    {
        return Type.Equals(other.Type) && Source.Equals(other.Source) && Args.SequenceEqual(other.Args);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((InvocationExpressionNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Source, Args);
    }
}