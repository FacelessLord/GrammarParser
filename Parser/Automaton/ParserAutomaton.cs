using System.Collections.Generic;
using Parser.Grammars.tokens;
using Parser.Nodes;
using Parser.Utils;

namespace Parser.Automaton
{
    public class ParserAutomaton
    {
        private readonly AutomatonTable _table;
        private readonly AutomatonInfo _info;
        private readonly Stack<(Token, AutomatonState)> _automatonStack;
        public ParserAutomaton(AutomatonTable table, AutomatonInfo info, Stack<(Token, AutomatonState)> automatonStack)
        {
            _table = table;
            _info = info;
            _automatonStack = automatonStack;
        }

        public INode Parse(BufferedEnumerable<Token> tokenStream)
        {
            return null;
        }
    }
}