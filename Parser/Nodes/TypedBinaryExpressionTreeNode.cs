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
}