using Parser.Grammars.Paths;
using Parser.Nodes;

namespace Parser.Grammars.Tokens
{
    public class NodeWrapper
    {
        public NodeWrapper(INode node, ContentSpan tokenSpan)
        {
            Node = node;
            TokenSpan = tokenSpan;
        }
        public ContentSpan TokenSpan { get; }
        public INode Node { get; }
    }
}