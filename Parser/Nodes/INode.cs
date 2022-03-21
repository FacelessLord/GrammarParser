using System.Collections.Generic;
using Parser.Grammars.tokens;

namespace Parser.Nodes
{
    public interface INode
    {
        public List<INode> Subnodes { get; }
        public TokenType Type { get; }
    }
}