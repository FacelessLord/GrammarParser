using Parser.Grammars.paths;

namespace Parser.Grammars.tokens
{
    public abstract class TokenType
    {
        public TokenType(string name)
        {
            Name = name;
        }
        public string Name { get; }
    }
}