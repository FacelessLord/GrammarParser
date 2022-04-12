using System.Collections.Generic;
using System.Linq;

namespace Parser.Grammars.Tokens;

public class OperationTerminalType : ConstantTerminalType
{
    public Operation Operation { get; }
    public Operation UnaryOperation { get; }
        
    public OperationTerminalType(string name, Operation operation, Operation? unaryOperation=null) : base(name)
    {
        Operation = operation;
        UnaryOperation = unaryOperation ?? operation;
    }

    public override Match GetMatch(string text)
    {
        var foundText = text.StartsWith(Name);
        var value = foundText ? Name : "";
        return new Match(foundText, value.Length, value);
    }
}

public enum Operation
{
    PrimeOp,
    Incerement,
    Plus,
    Decrement,
    Minus,
    Multiply,
    Divide,
    DivideInt,
    BitwiseAnd,
    BitwiseOr,
    BitwiseXor,
    And,
    Or,
    Xor,
    BitwiseNot,
    Not,
    UnaryMinus,
    UnaryPlus,
    StrictEqual,
    Equal,
    Assign,
    NotEqual,
    NotStrictEqual,
    LessOrEqual,
    GreaterOrEqual,
    ShiftLeft,
    ShiftRight,
    Less,
    Greater,
    
}