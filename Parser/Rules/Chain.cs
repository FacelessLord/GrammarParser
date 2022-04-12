using System;
using System.Collections.Generic;
using Parser.Grammars;
using Parser.Grammars.Tokens;
using Parser.Nodes;

namespace Parser.Rules
{
    public static class Chain
    {
        public static ChainConfig StartWith(TokenType rule)
        {
            return new(rule);
        }
    }

    public class GrammarRuleConfig
    {
        public GrammarRuleConfig(List<TokenType> elements, Func<INode[], INode> collector)
        {
            Elements = elements;
            Collector = collector;
        }
        public List<TokenType> Elements { get; }
        public Func<INode[], INode> Collector { get; }
    }

    public class ChainConfig
    {
        private List<TokenType> Elements { get; }

        public ChainConfig(TokenType rule)
        {
            Elements = new List<TokenType> { rule };
        }

        public ChainConfig Then(TokenType rule)
        {
            Elements.Add(rule);
            return this;
        }

        public GrammarRuleConfig CollectBy(Func<INode[], INode> collector)
        {
            return new GrammarRuleConfig( Elements, collector);
        }
    }
}