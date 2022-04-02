using Parser.Grammars.Paths;

namespace Parser.Grammars.tokens
{
    public abstract class TokenType
    {
        public static readonly TerminalType Eof = new EofToken();
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