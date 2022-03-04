using System.Collections.Generic;
using Parser.Grammars.paths;

namespace Parser.Grammars.tokens
{
    public class Token
    {
        public Token(TokenType type, ISourcePath tokenStartPath, ISourcePath tokenEndPath, List<Token> internals)
        {
            Internals = internals;
            Type = type;
            TokenStartPath = tokenStartPath;
            TokenEndPath = tokenEndPath;
        }
        public TokenType Type { get; }
        public ISourcePath TokenStartPath { get; }
        public ISourcePath TokenEndPath { get; }
        public List<Token> Internals { get; }
    }
}