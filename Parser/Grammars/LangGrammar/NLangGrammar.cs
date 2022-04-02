using System.Collections.Generic;
using System.Linq;
using Parser.Grammars.tokens;
using Parser.Rules;

namespace Parser.Grammars.LangGrammar
{
    public static class NLangGrammar
    {
        public static Grammar Grammar;

        static NLangGrammar()
        {
            var rulesList = NLangRules.Rules
                .SelectMany(r => r.Rules)
                .ToList();
            var rules = rulesList
                .GroupBy(t => t.Source)
                .ToDictionary(g => g.Key, g => g.ToHashSet());
            var terminals = rulesList
                .SelectMany(r => r.Production)
                .Where(t => t is TerminalType)
                .Cast<TerminalType>()
                .ToHashSet();
            var nonTerminals = rulesList
                .SelectMany(r => r.Production.Append(r.Source))
                .Where(t => t is NonTerminalType)
                .Cast<NonTerminalType>()
                .ToHashSet();

            Grammar = new Grammar(NLangRules.CreateVar, terminals,
                nonTerminals, rules);
        }
    }
}