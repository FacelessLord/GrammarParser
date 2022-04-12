namespace Parser.Grammars.Tokens
{
    public abstract class TerminalType: TokenType
    {
        protected TerminalType(string name) : base(name)
        {
        }

        public abstract Match GetMatch(string text);
    }

    public record Match(bool HasMatch, int MatchedLength, string MatchedText);
}