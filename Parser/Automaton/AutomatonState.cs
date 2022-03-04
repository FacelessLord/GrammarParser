using System;
using System.Collections.Generic;
using System.Linq;
using Parser.Grammars;

namespace Parser.Automaton
{
    public class AutomatonState
    {
        public AutomatonState(List<ParserItem> items, int id)
        {
            Items = items;
            Id = id;
            _lazyHashCode = new Lazy<int>(() => HashCode.Combine(BuildItemsHashCode(Items), Id));
        }
        private int Id { get; }
        public List<ParserItem> Items { get; }
        private readonly Lazy<int> _lazyHashCode;

        private bool Equals(AutomatonState other)
        {
            return Items.Count == other.Items.Count &&
                   Items.Where(i => i.Position == 0).Select((q, i) => q.Equals(other.Items[i])).All(x => x) &&
                   Id == other.Id;
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((AutomatonState) obj);
        }
        public override int GetHashCode() => _lazyHashCode.Value;

        private static int BuildItemsHashCode(IEnumerable<ParserItem> items)
        {
            return items.Where(i => i.Position == 0).Select(i => i.GetHashCode()).Aggregate(HashCode.Combine);
        }
    }
}