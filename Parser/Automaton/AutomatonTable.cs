using System;
using System.Collections.Generic;
using Parser.Grammars.Tokens;

namespace Parser.Automaton
{
    public class AutomatonTable
    {
        public AutomatonTable(Dictionary<AutomatonState, Func<TokenType, TokenType, IAutomatonAction>> actionSelectorTable)
        {
            _actionSelectorTable = actionSelectorTable;
        }

        public IAutomatonAction this[AutomatonState state, TokenType tokenType, TokenType lookahead] => _actionSelectorTable[state](tokenType, lookahead);

        private readonly Dictionary<AutomatonState, Func<TokenType, TokenType, IAutomatonAction>> _actionSelectorTable;
    }
}