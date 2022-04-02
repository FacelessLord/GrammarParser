using System;
using System.Collections.Generic;
using System.Linq;
using Parser.Grammars.tokens;
using Parser.Nodes;

namespace Parser.Grammars
{
    public class GrammarRule
    {
        public GrammarRule(NonTerminalType source, IReadOnlyList<TokenType> production, Func<INode[], INode> collector)
        {
            Source = source;
            Production = production;
            Collector = collector;
        }
        public NonTerminalType Source { get; }
        public IReadOnlyList<TokenType> Production { get; }
        public Func<INode[], INode> Collector { get; }

        public override string ToString()
        {
            return $"{Source.Name} => {string.Join(' ', Production.Select(t => t.Name))}";
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