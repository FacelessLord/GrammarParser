using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class ExpressionTreeLeafNode : ExpressionTreeNode
{
    public INode Leaf { get; }

    public ExpressionTreeLeafNode(INode leaf) : base(Operation.PrimeOp)
    {
        Leaf = leaf;
    }
}