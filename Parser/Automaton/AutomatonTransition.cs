using System;
using System.Collections.Generic;
using System.Linq;
using Parser.Grammars.tokens;

namespace Parser.Automaton
{
    public class AutomatonTransition
    {
        public AutomatonTransition(AutomatonState start, AutomatonState end, TokenType token,
            HashSet<TokenType> lookAheadTokens)
        {
            Start = start;
            End = end;
            Token = token;
            LookAheadTokens = lookAheadTokens;
            Action = new ShiftAction(end);
        }
        public AutomatonState Start { get; }
        public AutomatonState End { get; }
        public TokenType Token { get; }
        public HashSet<TokenType> LookAheadTokens { get; }
        
        public IAutomatonAction Action { get; }

        public override string ToString()
        {
            return $"{Start} +({Token.Name}) => {End} | {string.Join(",", LookAheadTokens.Select(t => t.ToString()))}";
        }

        protected bool Equals(AutomatonTransition other)
        {
            return Start.Equals(other.Start) &&
                   End.Equals(other.End) &&
                   Token.Equals(other.Token) &&
                   LookAheadTokens.Equals(other.LookAheadTokens);
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((AutomatonTransition) obj);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Start, End, Token, LookAheadTokens);
        }
    }
}