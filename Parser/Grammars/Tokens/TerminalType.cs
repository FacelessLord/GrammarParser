namespace Parser.Grammars.tokens
{
    public abstract class TerminalType: TokenType
    {
        protected TerminalType(string name) : base(name)
        {
        }

        public abstract Match TryMatch(string text);
    }

    public record Match(bool HasMatch, string? MatchedText);
}