using Parser.Nodes;

namespace Parser.Grammars.Tokens
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

        public override string ToString()
        {
            return $"{Type}[{Match}]";
        }

        protected bool Equals(TerminalNode other)
        {
            return Type.Equals(other.Type) && Match == other.Match;
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((TerminalNode) obj);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Match);
        }
    }
}