using Parser.Grammars.Tokens;

namespace Transpiler.Errors;

public class UnsupportedBaseNodeTypeError : IError
{
    public ErrorLevel Level { get; } = ErrorLevel.Fatal;
    public TokenType ActualType { get; }

    public UnsupportedBaseNodeTypeError(TokenType tokenType)
    {
        ActualType = tokenType;
    }
}