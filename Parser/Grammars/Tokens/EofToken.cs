namespace Parser.Grammars.tokens
{
    public class EofToken : TerminalType
    {

        public EofToken() : base("eof")
        {
        }

        public override Match TryMatch(string text)
        {
            return new Match(false, null);
        }
    }
}