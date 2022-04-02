using System;
using System.Collections.Generic;
using Parser.Grammars;
using Parser.Grammars.tokens;

namespace Parser.Rules
{
    public static class Rule
    {
        public static RuleToken Named(string name)
        {
            return new RuleToken(name);
        }
    }

    public class RuleToken : NonTerminalType
    {
        public RuleToken(string name) : base(name)
        {
            Rules = new List<GrammarRule>();
            LazyRules = new List<Func<RuleToken, GrammarRuleConfig>>();
        }
        
        public List<GrammarRule> Rules { get; }
        public List<Func<RuleToken, GrammarRuleConfig>> LazyRules { get; }
        
        public RuleToken With(GrammarRuleConfig config)
        {
            var rule = new GrammarRule(this, config.Elements, config.Collector);
            Rules.Add(rule);
            
            return this;
        }
        public RuleToken With(Func<RuleToken, GrammarRuleConfig> lazyConfig)
        {
            LazyRules.Add(lazyConfig);
            return this;
        }

        public void FinishConfiguration()
        {
            foreach (var lazyRule in LazyRules)
            {
                var config = lazyRule(this);
                With(config);
            }
        }
    }
    
}