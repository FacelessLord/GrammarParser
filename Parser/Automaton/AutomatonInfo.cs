using System.Collections.Generic;
using Parser.Grammars;

namespace Parser.Automaton
{
    public class AutomatonInfo
    {
        public AutomatonInfo(HashSet<AutomatonState> states, HashSet<AutomatonTransition> transitions, Grammar grammar)
        {
            States = states;
            Transitions = transitions;
            Grammar = grammar;
        }
        public HashSet<AutomatonState> States { get; }
        public HashSet<AutomatonTransition> Transitions { get; }
        public Grammar Grammar {get;}
    }
}