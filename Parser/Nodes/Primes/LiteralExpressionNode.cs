using Parser.Grammars.LangGrammar;
using Parser.Grammars.Tokens;

namespace Parser.Nodes.Primes;

public abstract class LiteralExpressionNode : INode
{
    public TokenType Type { get; } = NLangRules.Expressions.LiteralExpression;

}