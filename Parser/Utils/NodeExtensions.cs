using Parser.Grammars.tokens;
using Parser.Nodes;

namespace Parser.Utils;

public static class NodeExtensions
{
    public static INode Unwrap(this INode node)
    {
        if (node is WrapperNode wrapper)
            return wrapper.Node;
        return node;
    }

    public static string Match(this INode node)
    {
        return ((TerminalNode) node).Match;
    }
}