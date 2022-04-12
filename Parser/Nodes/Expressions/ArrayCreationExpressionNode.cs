using Parser.Grammars.LangGrammar;
using Parser.Grammars.Tokens;
using Parser.Nodes.Variables;

namespace Parser.Nodes.Expressions;

public class ArrayCreationExpressionNode : INode
{
    public TokenType Type { get; } = NLangRules.Expressions.ArrayCreationExpression;

    public TypeNode ArrayType { get; }
    public List<INode> Dimensions { get; }
    public ArrayInitializerNode? Initializer { get; }

    public ArrayCreationExpressionNode(INode type, INode dimensions, INode? initializer = null)
    {
        ArrayType = (TypeNode) type;
        Dimensions = ((ArgumentListNode) dimensions).Args;
        Initializer = (ArrayInitializerNode?) initializer;
    }

    protected bool Equals(ArrayCreationExpressionNode other)
    {
        return Type.Equals(other.Type) && ArrayType.Equals(other.ArrayType) && Dimensions.SequenceEqual(other.Dimensions) && Equals(Initializer, other.Initializer);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((ArrayCreationExpressionNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, ArrayType, Dimensions, Initializer);
    }
}