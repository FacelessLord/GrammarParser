using System;
using System.Collections.Generic;
using System.Linq;
using Parser.Grammars.tokens;
using Parser.Nodes;

namespace Parser.Grammars
{
    public class Grammar
    {
        public NonTerminalType Axiom { get; }
        public HashSet<TerminalType> Terminals { get; }
        public HashSet<NonTerminalType> NonTerminals { get; }
        public Dictionary<NonTerminalType, HashSet<GrammarRule>> Rules { get; }

        public Dictionary<TokenType, HashSet<TerminalType>> First { get; }
        public Dictionary<TokenType, HashSet<TerminalType>> Follow { get; }

        public Grammar(NonTerminalType axiom, HashSet<TerminalType> terminals, HashSet<NonTerminalType> nonTerminals,
            Dictionary<NonTerminalType, HashSet<GrammarRule>> rules)
        {
            Terminals = terminals;
            NonTerminals = nonTerminals;
            Rules = rules;
            Axiom = axiom;

            First = BuildFirst(terminals, nonTerminals);
            Follow = BuildFollow(terminals, nonTerminals);
        }

        public Grammar(Grammar source,
            NonTerminalType? axiom = null,
            HashSet<TerminalType>? terminals = null,
            HashSet<NonTerminalType>? nonTerminals = null,
            Dictionary<NonTerminalType,
                HashSet<GrammarRule>>? rules = null) : this(axiom ?? source.Axiom, terminals ?? source.Terminals,
            nonTerminals ?? source.NonTerminals, rules ?? source.Rules)
        {
        }

        public Grammar(NonTerminalType axiom) : this(axiom, new HashSet<TerminalType>(), new HashSet<NonTerminalType>(),
            new Dictionary<NonTerminalType, HashSet<GrammarRule>>())
        {
        }

        private Dictionary<TokenType, HashSet<TerminalType>> BuildFollow(HashSet<TerminalType> terminals,
            HashSet<NonTerminalType> nonTerminals)
        {
            var follow = new Dictionary<TokenType, HashSet<TerminalType>>();
            foreach (var type in terminals.Cast<TokenType>().Concat(nonTerminals))
            {
                follow[type] = new HashSet<TerminalType>();
            }
            follow[Axiom] = new HashSet<TerminalType>() { TokenType.Eof };

            for (var j = 0; j < nonTerminals.Count; j++)
                foreach (GrammarRule rule in nonTerminals.Select(nonTerminal => Rules[nonTerminal])
                    .SelectMany(rules => rules))
                {
                    for (var i = 0; i < rule.Production.Count - 1; i++)
                    {
                        var currSymbol = rule.Production[i];
                        var nextSymbol = rule.Production[i + 1];
                        follow[currSymbol].UnionWith(First[nextSymbol]);
                    }
                    follow[rule.Production[^1]].UnionWith(follow[rule.Source]);
                }

            return follow;
        }

        private Dictionary<TokenType, HashSet<TerminalType>> BuildFirst(HashSet<TerminalType> terminals,
            HashSet<NonTerminalType> nonTerminals)
        {
            var first = new Dictionary<TokenType, HashSet<TerminalType>>();
            terminals.ForEach(t => first[t] = new HashSet<TerminalType>() { t });
            var history = new HashSet<TokenType>();
            BuildFirstForToken(Axiom, first, history);
            for (var i = 0; i < 2; i++)
            {
                foreach (var nonTerminal in nonTerminals)
                {
                    BuildFirstForToken(nonTerminal, first, history);
                }
                history.Clear();
            }
            return first;
        }

        private void BuildFirstForToken(NonTerminalType token, Dictionary<TokenType, HashSet<TerminalType>> first,
            HashSet<TokenType> history)
        {
            if (history.Contains(token))
                return;
            var tokenFirst = new HashSet<TerminalType> { };
            history.Add(token);
            first[token] = tokenFirst;
            Rules[token]
                .Select(r =>
                {
                    if (!first.ContainsKey(r.Production[0]))
                        BuildFirstForToken((NonTerminalType) r.Production[0], first, history);
                    return r.Production[0];
                })
                .Distinct()
                .ForEach(t => first[t].ForEach(e => tokenFirst.Add(e)));
        }

        public void AddRule(GrammarRule rule)
        {
            if (!Rules.ContainsKey(rule.Source))
            {
                Rules[rule.Source] = new HashSet<GrammarRule>();
            }
            Rules[rule.Source].Add(rule);
        }

        public void AddRule(NonTerminalType source, IReadOnlyList<TokenType> production, Func<INode[], INode> collector)
        {
            AddRule(new GrammarRule(source, production, collector));
        }
    }
}