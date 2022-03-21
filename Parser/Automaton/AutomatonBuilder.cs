using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
            var nablaState = BuildState(nablaRules.Select(r =>
            {
                var item = new ParserItem(r);
                item.LookaheadTerminals.Add(TokenType.Eof);
                return item;
            }), grammar);

            var states = new HashSet<AutomatonState> { nablaState };
            var usedStates = new HashSet<AutomatonState>();
            var transitions = new HashSet<AutomatonTransition>();

            while (usedStates.Count - states.Count > 0)
            {
                var currState = states.First(s => !usedStates.Contains(s));
                var tokenToRule = currState.Items;
                foreach (var itemToShift in tokenToRule)
                {
                    var key = itemToShift.GetCurrentToken();
                    var shiftedItem = itemToShift.Shift();
                    var newState = BuildState(shiftedItem, grammar);
                    var nextToken = itemToShift.GetNextToken();
                    var transition = new AutomatonTransition(currState, newState, key,
                        nextToken != null ? grammar.First[nextToken] : new HashSet<TokenType>());
                    states.Add(newState);
                    transitions.Add(transition);
                }
                usedStates.Add(currState);
            }
            var info = new AutomatonInfo(states, transitions, grammar);
            var table = BuildTable(states, transitions);

            return new ParserAutomaton(table, info, new Stack<(Token, AutomatonState)>());
        }

        private static AutomatonTable BuildTable(HashSet<AutomatonState> states,
            HashSet<AutomatonTransition> transitions)
        {
            var dict = new Dictionary<AutomatonState, Func<TokenType, TokenType, IAutomatonAction>>();
            var transitionsBySource = transitions.GroupBy(t => t.Start).ToImmutableDictionary(g => g.Key);
            foreach (var state in states)
            {
                var stateF = state;

                IAutomatonAction ActionSelector(TokenType curr, TokenType next)
                {
                    var transitions = transitionsBySource[stateF];
                    var transitionsForToken = transitions
                        .Where(t => t.Token == curr && t.LookAheadTokens.Contains(next))
                        .ToList();
                    if (transitionsForToken.Count > 1)
                    {
                        var choices = transitionsForToken.Join(",");
                        throw new Exception(
                            $"Can't decide what transition to take from state {stateF}.\n Choices: [{choices}]");
                    }
                    if (transitionsForToken.Count == 1)
                    {
                        return transitionsForToken.Single().Action;
                    }
                    var reducibleItems = stateF.Items.Where(i =>
                        i.GetCurrentToken() == null && i.LookaheadTerminals.Contains(next)).ToList();
                    if (reducibleItems.Count > 1)
                    {
                        var choices = reducibleItems.Join(";");
                        throw new Exception(
                            $"Can't decide what reduction to apply from state {stateF}.\n Choices: [{choices}]");
                    }

                    return new ReduceAction(reducibleItems.Single().Rule);
                }

                dict[state] = ActionSelector;
            }
            return new AutomatonTable(dict);
        }

        private AutomatonState BuildState(ParserItem item, Grammar grammar)
        {
            var stateItems = new List<ParserItem> { item };
            return BuildStateFromItems(grammar, stateItems);
        }

        private AutomatonState BuildState(IEnumerable<ParserItem> items, Grammar grammar)
        {
            var stateItems = new List<ParserItem>(items);
            return BuildStateFromItems(grammar, stateItems);
        }
        private AutomatonState BuildStateFromItems(Grammar grammar, List<ParserItem> stateItems)
        {
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
            var repeatedItems = stateItems.GroupBy(i => (i.Rule, i.Position));
            var deduplicatedItems = repeatedItems.Select(g =>
                {
                    var item = new ParserItem(g.Key.Rule, g.Key.Position);
                    foreach (var sourceItem in g)
                    {
                        item.LookaheadTerminals.UnionWith(sourceItem.LookaheadTerminals);
                    }

                    return item;
                })
                .ToList();

            return new AutomatonState(deduplicatedItems, _nextId++);
        }
        private static IEnumerable<ParserItem> BuildItemClosure(Grammar grammar, ParserItem item)
        {
            var currToken = item.GetCurrentToken();
            if (currToken is not NonTerminalType nonTerm)
            {
                return Array.Empty<ParserItem>();
            }
            return grammar.Rules[nonTerm].Select<GrammarRule, ParserItem>(r =>
            {
                var newItem = new ParserItem(r);
                if (item.GetNextToken() is { } next)
                    newItem.LookaheadTerminals.UnionWith(grammar.First[next]);
                else
                    newItem.LookaheadTerminals.UnionWith(item.LookaheadTerminals);

                return newItem;
            });
        }
    }
}