using Parser.Grammars.LangGrammar;
using Parser.Grammars.tokens;

namespace Parser.Nodes.Primes;

public abstract class LiteralExpressionNode : INode
{
    public TokenType Type { get; } = NLangRules.LiteralExpression;

}