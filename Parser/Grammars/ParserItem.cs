using System;
using System.Collections.Generic;
using Parser.Grammars.tokens;

namespace Parser.Grammars
{
    public class ParserItem
    {
        public GrammarRule Rule { get; }
        public int Position { get; }

        public HashSet<TokenType> LookaheadTerminals = new HashSet<TokenType>();

        public ParserItem(GrammarRule rule, int position = 0)
        {
            Rule = rule;
            Position = position;
        }

        public ParserItem Shift()
        {
            if (Position > Rule.Production.Count)
                throw new IndexOutOfRangeException($"Can't shift in rule {Rule}");
            return new ParserItem(Rule, Position + 1);
        }

        public TokenType? GetCurrentToken()
        {
            return Position > Rule.Production.Count ? null : Rule.Production[Position];
        }

        protected bool Equals(ParserItem other)
        {
            return Rule.Equals(other.Rule) && Position == other.Position;
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((ParserItem) obj);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Rule, Position);
        }
    }
}