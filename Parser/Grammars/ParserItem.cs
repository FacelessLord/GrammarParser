using System;
using System.Collections.Generic;
using System.Linq;
using Parser.Grammars.Tokens;

namespace Parser.Grammars
{
    public class ParserItem
    {
        public GrammarRule Rule { get; }
        public int Position { get; }

        public HashSet<TerminalType> LookaheadTerminals { get; private init; } = new(){TokenType.Eof};

        public ParserItem(GrammarRule rule, int position = 0)
        {
            Rule = rule;
            Position = position;
        }
        public ParserItem(GrammarRule rule, HashSet<TerminalType> lookaheadTerminals, int position = 0)
        {
            Rule = rule;
            Position = position;
            LookaheadTerminals = lookaheadTerminals;
        }

        public ParserItem Shift()
        {
            if (Position > Rule.Production.Count)
                throw new IndexOutOfRangeException($"Can't shift in rule {Rule}");
            return new ParserItem(Rule, LookaheadTerminals, Position + 1);
        }

        public TokenType? GetCurrentToken()
        {
            return Position >= Rule.Production.Count ? null : Rule.Production[Position];
        }
        public TokenType? GetNextToken()
        {
            return Position + 1 >= Rule.Production.Count ? null : Rule.Production[Position + 1];
        }

        public override string ToString()
        {
            var rule = Rule.ToString().Split(" ");
            return $"{string.Join(' ', rule[..(Position + 2)])}•{string.Join(' ', rule[(Position + 2)..])}";
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