using System;
using System.Collections.Generic;
using Parser.Grammars.tokens;
using Parser.Nodes;
using Parser.Utils;

namespace Parser.Rules
{
    public class RuleChain
    {
        public List<TokenType> Elements { get; }
        public Func<INode[], INode> Collector { get; }
        /**
         * gets all but first node
         * associated with betas in left-recursive rule scheme
         */
        public Func<INode[], INode> LeftRecursionCollector { get; }

        public RuleChain(List<TokenType> elements, Func<INode[], INode> collector = null, Func<INode[], INode> leftRecursionCollector = null)
        {
            Elements = elements;
            Collector = collector;
            LeftRecursionCollector = leftRecursionCollector;
        }
    }
}