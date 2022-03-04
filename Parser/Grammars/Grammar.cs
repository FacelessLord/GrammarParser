using System.Collections.Generic;
using System.Linq;
using Parser.Grammars.tokens;

namespace Parser.Grammars
{
    public class Grammar
    {
        public NonTerminalType Axiom { get; }
        public HashSet<TerminalType> Terminals { get; }
        public HashSet<NonTerminalType> NonTerminals { get; }
        public Dictionary<NonTerminalType, HashSet<GrammarRule>> Rules { get; }

        public Dictionary<TokenType, HashSet<TokenType>> First { get; }

        public Grammar(NonTerminalType axiom, HashSet<TerminalType> terminals, HashSet<NonTerminalType> nonTerminals,
            Dictionary<NonTerminalType, HashSet<GrammarRule>> rules)
        {
            Terminals = terminals;
            NonTerminals = nonTerminals;
            Rules = rules;
            Axiom = axiom;

            First = BuildFirst(nonTerminals);
        }

        public Grammar(NonTerminalType axiom) : this(axiom, new HashSet<TerminalType>(), new HashSet<NonTerminalType>(),
            new Dictionary<NonTerminalType, HashSet<GrammarRule>>())
        {
        }

        private Dictionary<TokenType, HashSet<TokenType>> BuildFirst(HashSet<NonTerminalType> nonTerminals)
        {
            var first = new Dictionary<TokenType, HashSet<TokenType>>();
            BuildFirstForToken(Axiom, first);
            return first;
        }

        private void BuildFirstForToken(TokenType token, Dictionary<TokenType, HashSet<TokenType>> first)
        {
            if (first.ContainsKey(token))
                return;
            var tokenFirst = new HashSet<TokenType> { token };
            first[token] = tokenFirst;
            if (token is NonTerminalType nonTerm)
            {
                Rules[nonTerm]
                    .Select(r =>
                    {
                         BuildFirstForToken(r.Production[0], first);
                         return r.Production[0];
                    })
                    .ForEach(t => first[t]
                        .ForEach(e => tokenFirst.Add(e)));
            }
        }

        public void AddRule(GrammarRule rule)
        {
            if (!Rules.ContainsKey(rule.Source))
            {
                Rules[rule.Source] = new HashSet<GrammarRule>();
            }
            Rules[rule.Source].Add(rule);
        }

        public void AddRule(NonTerminalType source, IReadOnlyList<TokenType> production)
        {
            AddRule(new GrammarRule(source, production));
        }
    }
}