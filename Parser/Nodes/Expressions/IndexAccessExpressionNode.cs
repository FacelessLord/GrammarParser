using Parser.Grammars.LangGrammar;
using Parser.Grammars.Tokens;
using Parser.Nodes.Variables;

namespace Parser.Nodes.Expressions;

public class IndexAccessExpressionNode : INode
{
    public TokenType Type { get; } = NLangRules.Expressions.IndexAccessExpression;
    
    public INode Source { get; }
    public List<INode> Index { get; }

    public IndexAccessExpressionNode(INode source, INode? index=null)
    {
        Source = source;
        Index = index is null ? new List<INode>() : ((ArgumentListNode) index).Args;
    }

    protected bool Equals(IndexAccessExpressionNode other)
    {
        return Type.Equals(other.Type) && Source.Equals(other.Source) && Index.SequenceEqual(other.Index);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((IndexAccessExpressionNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Source, Index);
    }
}