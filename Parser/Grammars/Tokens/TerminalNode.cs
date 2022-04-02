using Parser.Nodes;

namespace Parser.Grammars.tokens
{
    public class TerminalNode : INode
    {
        public TokenType Type { get; }
        public string Match { get; }

        public TerminalNode(TokenType type, string match)
        {
            Type = type;
            Match = match;
        }
    }
}