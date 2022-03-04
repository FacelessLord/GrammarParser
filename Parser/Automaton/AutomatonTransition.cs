using System.Collections.Generic;
using Parser.Grammars.tokens;

namespace Parser.Automaton
{
    public class AutomatonTransition
    {
        public AutomatonTransition(AutomatonState start, AutomatonState end, HashSet<TokenType> lookAheadTokens)
        {
            Start = start;
            End = end;
            LookAheadTokens = lookAheadTokens;
        }
        public AutomatonState Start { get; }
        public AutomatonState End { get; }
        public TokenType Token { get; }
        public HashSet<TokenType> LookAheadTokens { get; }
    }
}