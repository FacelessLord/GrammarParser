using System.Collections.Generic;
using System.Linq;

namespace Parser.Grammars.tokens
{
    public class ConstantTerminalType : TerminalType
    {
        private readonly string[] _variants;

        public ConstantTerminalType(string name, IEnumerable<string> variants) : base(name)
        {
            _variants = variants.OrderBy(v => -v.Length).ToArray();
        }

        public override Match TryMatch(string text)
        {
            var foundText = _variants.Where(text.StartsWith).FirstOrDefault();
            return new Match(foundText is not null, foundText);
        }
    }
}