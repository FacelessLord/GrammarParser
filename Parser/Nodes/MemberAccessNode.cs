using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes;

public class MemberAccessNode : INode
{
    public TokenType Type { get; } = NLangRules.MemberAccess;
    public ExpressionNode Source { get; }
    public List<string> Path { get; }

    public MemberAccessNode(INode prev, INode next)
    {
        if (prev is MemberAccessNode prevMember)
        {
            Source = prevMember.Source;
            Path = prevMember.Path;
            Path.Add(((TerminalNode) next).Match);
        }
        else
        {
            Source = (ExpressionNode) prev;
            Path = new List<string> { ((TerminalNode) next).Match };
        }
    }
}