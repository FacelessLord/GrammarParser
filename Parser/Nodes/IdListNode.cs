using System.Collections.Generic;
using System.Linq;
using Parser.Grammars.LangGrammar;
using Parser.Grammars.Paths;
using Parser.Grammars.tokens;

namespace Parser.Nodes
{
    public class IdListNode : INode
    {
        public TokenType Type { get; } = NLangRules.IdList;
        public List<string> Ids;

        public IdListNode(INode node)
        {
            Ids = new List<string> { ((TerminalNode) node).Match };
        }
        public IdListNode(INode prev, INode next)
        {
            Ids = ((IdListNode) prev).Ids;
            Ids.Add(((TerminalNode) next).Match);
        }
    }
}