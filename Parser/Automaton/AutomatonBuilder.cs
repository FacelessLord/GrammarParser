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
            var nablaState = BuildState(nablaRules.Select(r => new ParserItem(r)), grammar);

            var states = new HashSet<AutomatonState> { nablaState };
            var usedStates = new HashSet<AutomatonState>();
            var transitions = new HashSet<AutomatonTransition>();

            while (states.Count > 0)
            {
                var currState = states.First();
                states.Remove(currState);
                usedStates.Add(currState);
                //all possible input states for currState when reducing possible
                var possibleInputStates = currState.Items.GetCurrentSymbols();

                foreach (var inputState in possibleInputStates)
                {
                    var key = inputState.Item1;
                    var lookahead = inputState.Item2;

                    var rulesToShift = currState.Items
                        .Where(i => i.GetCurrentToken() == key)
                        .Select(r => r.Shift());
                    var newState = BuildState(rulesToShift, grammar);
                    
                    if(!usedStates.Contains(newState) && !states.Contains(newState))
                    {
                        var transition = new AutomatonTransition(currState, newState, key, lookahead);
                        states.Add(newState);
                        transitions.Add(transition);
                    }
                    else
                    {
                        if(usedStates.TryGetValue(newState, out var actualState) || states.TryGetValue(newState, out actualState))
                        {
                            var transition = new AutomatonTransition(currState, actualState, key, lookahead);
                            
                            transitions.Add(transition);
                        }
                    }
                }
            }
            var info = new AutomatonInfo(usedStates, transitions, grammar);
            var table = BuildTable(usedStates, transitions);

            var stack = new Stack<(NodeWrapper, AutomatonState)>();
            stack.Push((null, nablaState));
            return new ParserAutomaton(table, info, stack);
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
                    var reducibleItems = stateF.Items.Where(i =>
                        i.GetCurrentToken() == null && i.LookaheadTerminals.Contains(next)).ToList();
                    if (reducibleItems.Count > 1)
                    {
                        var choices = reducibleItems.Join(";");
                        throw new Exception(
                            $"Can't decide what reduction to apply from state {stateF}.\n Choices: [{choices}]");
                    }
                    if(reducibleItems.Count == 1)
                    {
                        return new ReduceAction(reducibleItems.Single().Rule);
                    }

                    var reducibleItemsWithoutLookahead = stateF.Items.Where(i =>
                        i.GetCurrentToken() == null).ToList();
                    if (reducibleItemsWithoutLookahead.Count > 1)
                    {
                        var choices = reducibleItems.Join(";");
                        throw new Exception(
                            $"Can't decide what reduction to apply from state {stateF}.\n Choices: [{choices}]");
                    }
                    if(reducibleItemsWithoutLookahead.Count == 1)
                    {
                        return new ReduceAction(reducibleItemsWithoutLookahead.Single().Rule);
                    }

                    var transitions = transitionsBySource.ContainsKey(stateF)
                        ? transitionsBySource[stateF].ToList()
                        : new List<AutomatonTransition>();
                    var transitionsForToken = transitions
                        .Where(t => t.Token == curr)
                        .ToList();
                    if (transitionsForToken.Count > 1)
                    {
                        var choices = transitionsForToken.Join(", ");
                        throw new Exception(
                            $"Can't decide what transition to take from state {stateF}.\n Choices: [{choices}]");
                    }
                    if (transitionsForToken.Count == 1)
                    {
                        return transitionsForToken.Single().Action;
                    }
                    throw new Exception(
                        $"Can't find any transition or reducings to apply from state {stateF} by {curr}|-{next}");
                }

                dict[state] = ActionSelector;
            }
            return new AutomatonTable(dict);
        }

        private AutomatonState BuildState(IEnumerable<ParserItem> items, Grammar grammar)
        {
            var stateItems = new HashSet<ParserItem>(items);
            var usedSymbols = new HashSet<NonTerminalType>();
            var currentSymbols = stateItems.GetCurrentNonTerminalSymbols();
            while (currentSymbols.Count > 0)
            {
                var curr = currentSymbols.Take(1).Single();
                currentSymbols.Remove(curr);
                usedSymbols.Add(curr);

                var newItems = grammar.Rules[curr]
                    .Select(r => new ParserItem(r, grammar.Follow[curr]))
                    .ToHashSet();
                stateItems.UnionWith(newItems);

                var newSymbols = newItems.GetCurrentNonTerminalSymbols();
                newSymbols.ExceptWith(usedSymbols);
                currentSymbols.UnionWith(newSymbols);
            }
            return new AutomatonState(stateItems, _nextId++);
        }
    }

    public static class ParserItemExtensions
    {
        public static HashSet<NonTerminalType> GetCurrentNonTerminalSymbols(this HashSet<ParserItem> items)
        {
            return items
                .Select(i =>  i.GetCurrentToken())
                .Where(t => t is NonTerminalType)
                .Select(t => (NonTerminalType) t!)
                .ToHashSet();
        }
        public static HashSet<(TokenType, HashSet<TerminalType>)> GetCurrentSymbols(this HashSet<ParserItem> items)
        {
            return items
                .Select(i => (i.GetCurrentToken(), i.LookaheadTerminals))
                .Where(t => t.Item1 is not null)
                .Select(t => (t.Item1!, t.Item2))
                .ToHashSet();
        }
    }
}