using System.Collections.Generic;
using Parser.Grammars.Tokens;

namespace Parser.Nodes
{
    public interface INode
    {
        public TokenType Type { get; }
    }
}