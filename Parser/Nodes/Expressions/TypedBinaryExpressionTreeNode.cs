using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class TypedBinaryExpressionTreeNode : ExpressionNode
{
    public Operation Operation { get; }
    public ExpressionNode Left { get; }
    public ExpressionNode Right { get; }

    public TypedBinaryExpressionTreeNode(TokenType type, INode operation, INode left, INode right)
    {
        Type = type;
        Operation = ((OperationTerminalType) operation.Type).Operation;
        Left = (ExpressionNode) left;
        Right = (ExpressionNode) right;
    }

    protected bool Equals(TypedBinaryExpressionTreeNode other)
    {
        return Operation == other.Operation && Left.Equals(other.Left) && Right.Equals(other.Right);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((TypedBinaryExpressionTreeNode) obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine((int) Operation, Left, Right);
    }
}