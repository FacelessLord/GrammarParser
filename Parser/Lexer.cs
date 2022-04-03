using System.Collections.Generic;
using System.Linq;
using Parser.Exceptions;
using Parser.Grammars.Paths;
using Parser.Grammars.tokens;

namespace Parser
{
    public class Lexer
    {
        private readonly List<TerminalType> _tokenTypes;
        public Lexer(List<TerminalType> tokenTypes)
        {
            _tokenTypes = tokenTypes;
        }

        private IEnumerable<NodeWrapper> ParseInternal(Parser.Grammars.Paths.FileInfo info)
        {
            var source = info.Source;
            var content = info.Content;
            int line = 0;
            var match = new Match(true, 0, "");
            f:
            while (content.Length > 0 && match.HasMatch)
            {
                foreach (var tokenType in _tokenTypes)
                {
                    match = tokenType.GetMatch(content);
                    if (match.HasMatch)
                    {
                        content = content[match.MatchedLength..].TrimStart();
                        var tokenPath = source.At(line);
                        var node = new TerminalNode(tokenType, match.MatchedText);
                        yield return new NodeWrapper(node, new ContentSpan(tokenPath));
                        goto f;
                    }
                }
                if (match.HasMatch)
                    continue;

                match = TokenType.NewLine.GetMatch(content);
                if (!match.HasMatch)
                    break;

                line++;
                content = content[match.MatchedLength..].TrimStart();
            }

            if (!match.HasMatch)
                throw new UnexpectedCharacterException(content);
        }

        public Stack<NodeWrapper> Parse(Parser.Grammars.Paths.FileInfo info)
        {
            var tokenStream = new Stack<NodeWrapper>();
            var superEofToken = new NodeWrapper(new TerminalNode(TokenType.SuperEof, ""),
                new ContentSpan(new ContentPath(info.Source, 0)));
            var eofToken = new NodeWrapper(new TerminalNode(TokenType.Eof, ""),
                new ContentSpan(new ContentPath(info.Source, 0)));
            tokenStream.Push(superEofToken);
            tokenStream.Push(eofToken);
            ParseInternal(info).Reverse().ForEach(tokenStream.Push);
            return tokenStream;
        }
    }
}