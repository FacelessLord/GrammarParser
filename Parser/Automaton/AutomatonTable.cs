using System.Collections.Generic;
using Parser.Grammars.tokens;

namespace Parser.Automaton
{
    public class AutomatonTable
    {
        public AutomatonTable(Dictionary<AutomatonState, Dictionary<TokenType, IAutomatonAction>> table)
        {
            _table = table;
        }

        public IAutomatonAction this[AutomatonState state, TokenType tokenType]
        {
            get => _table[state][tokenType];
            set
            {
                if (!_table.ContainsKey(state))
                    _table[state] = new Dictionary<TokenType, IAutomatonAction>();
                _table[state][tokenType] = value;
            }
        }

        private readonly Dictionary<AutomatonState, Dictionary<TokenType, IAutomatonAction>> _table;
    }
}