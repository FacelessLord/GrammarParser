using System.Collections.Generic;
using System.Linq;

namespace Parser.Grammars.tokens
{
    public class OperationTerminalType : ConstantTerminalType
    {
        public Operation Operation { get; }
        
        public OperationTerminalType(string name, Operation operation) : base(name)
        {
            Operation = operation;
        }

        public override Match GetMatch(string text)
        {
            var foundText = text.StartsWith(Name);
            var value = foundText ? Name : "";
            return new Match(foundText, value.Length, value);
        }
    }
}

public enum Operation
{
    PrimeOp,
    Plus,
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
    Less,
    Greater,
    
}