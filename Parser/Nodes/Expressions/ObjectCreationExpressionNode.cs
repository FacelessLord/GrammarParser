using Parser.Grammars.LangGrammar;
using Parser.Grammars.Tokens;
using Parser.Nodes.Variables;

namespace Parser.Nodes.Expressions;

public class ObjectCreationExpressionNode : INode
{

    public TokenType Type { get; } = NLangRules.Expressions.ObjectCreationExpression;

    public TypeNode ObjectType { get; }
    public List<INode> Args { get; }
    
    public ObjectCreationExpressionNode(INode type, INode? args = null)
    {
        ObjectType = (TypeNode) type;
        Args = args is null ? new List<INode>() : ((ArgumentListNode) args).Args;
    }

    protected bool Equals(ObjectCreationExpressionNode other)
    {
        return Type.Equals(other.Type) && ObjectType.Equals(other.ObjectType) && Args.SequenceEqual(other.Args);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((ObjectCreationExpressionNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, ObjectType, Args);
    }
}