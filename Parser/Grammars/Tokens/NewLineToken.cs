namespace Parser.Grammars.tokens
{
    public class NewLineToken : TerminalType
    {

        public NewLineToken() : base("newLine")
        {
        }

        public override Match GetMatch(string text)
        {
            if (text.StartsWith("\n"))
                return new Match(true, 1, "\n");
            return new Match(false, 0, null);
        }
    }
}