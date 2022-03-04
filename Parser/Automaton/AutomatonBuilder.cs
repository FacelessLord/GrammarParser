using System;
using System.Collections.Generic;
using System.Linq;
using Parser.Grammars;
using Parser.Grammars.tokens;

namespace Parser.Automaton
{
    public class AutomatonBuilder
    {
        private int _nextId;
        
        public ParserAutomaton Build(Grammar grammar)
        {
            var nablaRules = grammar.Rules[grammar.Axiom];
            var nablaState = BuildState(nablaRules.Select<GrammarRule, ParserItem>(r =>
            {
                var item = new ParserItem(r);
                item.LookaheadTerminals.Add(TokenType.)
                return item;
            }), grammar);
            
            var states = new HashSet<AutomatonState>{nablaState};
            var usedStates = new HashSet<AutomatonState> {};

            while (usedStates.Count - states.Count > 0)
            {
                var currState = states.First(s => !usedStates.Contains(s));
                if (currState != null)
                {
                    var tokenToRule = currState.Items.GroupBy(i => i.GetCurrentToken());
                }
                usedStates.Add(currState);
            }
        }

        public AutomatonState BuildState(IEnumerable<ParserItem> items, Grammar grammar)
        {
            var stateItems = new List<ParserItem>(items);
            var pendingRules = new List<ParserItem>(stateItems);
            var usedItems = new HashSet<ParserItem>();
            while (pendingRules.Count > 0)
            {
                var rule = pendingRules.First();
                usedItems.Add(rule);
                BuildItemClosure(grammar, rule)
                    .Where(i => !usedItems.Contains(i))
                    .ForEach(i =>
                    {
                        stateItems.Add(i);
                        pendingRules.Add(i);
                    });
            }

            return new AutomatonState(stateItems, _nextId++);
        }
        public IEnumerable<ParserItem> BuildItemClosure(Grammar grammar, ParserItem item)
        {
            var currToken = item.GetCurrentToken();
            if (currToken is not NonTerminalType nonTerm)
            {
                return Array.Empty<ParserItem>();
            }
            return grammar.Rules[nonTerm].Select<GrammarRule, ParserItem>(r => r);
        }
    }
}