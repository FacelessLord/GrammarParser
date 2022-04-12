using Transpiler.Errors;

namespace Transpiler;

public class TranspilationResult
{
    public List<IError> Errors { get; }
    public string? TranspiledCode { get; set; } = null;

    public TranspilationResult(List<IError> errors, string? transpiledCode = null)
    {
        Errors = errors;
        TranspiledCode = transpiledCode;
    }
}