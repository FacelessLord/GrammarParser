namespace Parser.Grammars.Tokens
{
    public class VariableConstantTerminalType : TerminalType
    {
        private readonly string[] _variants;

        public VariableConstantTerminalType(string name, IEnumerable<string> variants) : base(name)
        {
            _variants = variants.OrderBy(v => -v.Length).ToArray();
        }

        public override Match GetMatch(string text)
        {
            var foundText = _variants.Where(text.StartsWith).FirstOrDefault();
            return foundText is not null ? new Match(true, foundText.Length, foundText) : new Match(false, 0, "");
        }
    }
}