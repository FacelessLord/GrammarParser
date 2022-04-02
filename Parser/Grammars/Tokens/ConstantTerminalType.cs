using System.Collections.Generic;
using System.Linq;

namespace Parser.Grammars.tokens
{
    public class ConstantTerminalType : TerminalType
    {
        public ConstantTerminalType(string name) : base(name)
        {
        }

        public override Match GetMatch(string text)
        {
            var foundText = text.StartsWith(Name);
            var value = foundText ? Name : "";
            return new Match(foundText, value.Length, value);
        }
    }
}