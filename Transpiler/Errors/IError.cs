namespace Transpiler.Errors;

public interface IError
{
    ErrorLevel Level { get; }
}

public enum ErrorLevel
{
    Info, 
    Warning, // compiler will succeed, but things need to be fixed later
    Error, // will try to find other errors even though exception will be thrown in the end
    Fatal // can't continue checks
}