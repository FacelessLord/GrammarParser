using System.Collections.Generic;
using Parser.Grammars.tokens;

namespace Parser.Nodes
{
    public interface INode
    {
        public TokenType Type { get; }
    }
}