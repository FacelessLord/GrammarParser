﻿using System;
using System.Collections.Generic;
using System.Linq;
using Parser.Grammars.tokens;

namespace Parser.Grammars
{
    public class GrammarRule
    {
        public GrammarRule(NonTerminalType source, IReadOnlyList<TokenType> production)
        {
            Source = source;
            Production = production;
        }
        public NonTerminalType Source { get; }
        public IReadOnlyList<TokenType> Production { get; }

        public static implicit operator ParserItem(GrammarRule rule)
        {
            return new ParserItem(rule);
        }

        protected bool Equals(GrammarRule other)
        {
            return Source.Equals(other.Source) &&
                   Production.Count == other.Production.Count &&
                   Production.Select((q, i) => q.Equals(other.Production[i])).All(x => x);
        }
        
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((GrammarRule) obj);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Source, Production);
        }
    }
}