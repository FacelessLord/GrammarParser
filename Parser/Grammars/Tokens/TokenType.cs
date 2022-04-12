namespace Parser.Grammars.Tokens
{
    public abstract class TokenType
    {
        public static readonly TerminalType Eof = new EofToken();
        public static readonly TerminalType SuperEof = new SuperEofToken();
        public static readonly TerminalType NewLine = new NewLineToken();
        public TokenType(string name)
        {
            Name = name;
        }
        public string Name { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}