using System;
using System.Collections.Generic;
using System.Linq;
using Parser.Grammars;

namespace Parser.Automaton
{
    public class AutomatonState
    {
        public AutomatonState(HashSet<ParserItem> items, int id)
        {
            Items = items;
            Id = id;
            _lazyHashCode = new Lazy<int>(() => HashCode.Combine(BuildItemsHashCode(Items)));
        }
        private int Id { get; }
        public HashSet<ParserItem> Items { get; }
        private readonly Lazy<int> _lazyHashCode;

        public override string ToString()
        {
            return Items.Join("; ");
        }

        public override int GetHashCode() => _lazyHashCode.Value;

        private static int BuildItemsHashCode(IEnumerable<ParserItem> items)
        {
            return items
                .Select(i => i.GetHashCode())
                .Aggregate(0, HashCode.Combine);
        }

        public override bool Equals(object? obj)
        {
            return obj is AutomatonState state && GetHashCode() == state.GetHashCode() && Items.SetEquals(state.Items);
        }
    }
}