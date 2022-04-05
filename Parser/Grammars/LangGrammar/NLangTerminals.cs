using System.Collections.Generic;
using Parser.Grammars.LangGrammar.Terminals;
using Parser.Grammars.tokens;

namespace Parser.Grammars.LangGrammar
{
    public static class NLangTerminals
    {
        public static Dictionary<string, TerminalType> TerminalTypes = new();
        public static List<TerminalType> Terminals;

        private static TerminalType Constant(string name) => new ConstantTerminalType(name);
        private static TerminalType Operation(string name, Operation operation, Operation? unaryOperation=null) => new OperationTerminalType(name, operation, unaryOperation);

        public static TerminalType String = new StringTerminal();
        public static TerminalType Float = new FloatTerminal();
        public static TerminalType Int = new IntTerminal();
        public static TerminalType Id = new IdTerminal();
        public static TerminalType Colon = Constant(":");
        public static TerminalType Semicolon = Constant(";");
        public static TerminalType LParen = Constant("(");
        public static TerminalType RParen = Constant(")");
        public static TerminalType LBrace = Constant("{");
        public static TerminalType RBrace = Constant("}");
        public static TerminalType LBracket = Constant("[");
        public static TerminalType RBracket = Constant("]");
        public static TerminalType Comma = Constant(",");
        public static TerminalType Dot = Constant(".");
        public static TerminalType DoubleQuestionMark = Constant("??");
        public static TerminalType QuestionMark = Constant("?");
        public static TerminalType Arrow = Constant("=>");
        public static TerminalType StrictEqual = Operation("===", global::Operation.StrictEqual);
        public static TerminalType Equal = Operation("==", global::Operation.Equal);
        public static TerminalType Assign = Operation("=", global::Operation.Assign);
        public static TerminalType NotStrictEqual = Operation("!==", global::Operation.NotStrictEqual);
        public static TerminalType NotEqual = Operation("!=", global::Operation.NotEqual);
        public static TerminalType LessOrEqual = Operation("<=", global::Operation.LessOrEqual);
        public static TerminalType GreaterOrEqual = Operation(">=", global::Operation.GreaterOrEqual);
        public static TerminalType ShiftLeft = Operation("<<", global::Operation.ShiftLeft);
        public static TerminalType ShiftRight = Operation(">>", global::Operation.ShiftRight);
        public static TerminalType Less = Operation("<", global::Operation.Less);
        public static TerminalType Greater = Operation(">", global::Operation.Greater);
        public static TerminalType DoublePlus = Operation("++", global::Operation.Plus, global::Operation.UnaryPlus);
        public static TerminalType Plus = Operation("+", global::Operation.Plus, global::Operation.UnaryPlus);
        public static TerminalType DoubleMinus = Operation("--", global::Operation.Minus, global::Operation.UnaryMinus);
        public static TerminalType Minus = Operation("-", global::Operation.Minus, global::Operation.UnaryMinus);
        public static TerminalType Asterisk = Operation("*", global::Operation.Multiply);
        public static TerminalType DoubleSlash = Operation("//", global::Operation.DivideInt);
        public static TerminalType Slash = Operation("/", global::Operation.Divide);
        public static TerminalType Hat = Operation("^", global::Operation.BitwiseXor);
        public static TerminalType DoubleHat = Operation("^^", global::Operation.Xor);
        public static TerminalType Pipe = Operation("|", global::Operation.BitwiseOr);
        public static TerminalType DoublePipe = Operation("||", global::Operation.Or);
        public static TerminalType Ampersand = Operation("&", global::Operation.BitwiseAnd);
        public static TerminalType DoubleAmpersand = Operation("&&", global::Operation.And);
        public static TerminalType Tilda = Operation("~", global::Operation.BitwiseNot);
        public static TerminalType ExclamationMark = Operation("!", global::Operation.Not);
        public static TerminalType PrimeOp = Operation("", global::Operation.PrimeOp);

        public static TerminalType Let = Constant("let");
        public static TerminalType Const = Constant("const");
        public static TerminalType Compile = Constant("compile");
        public static TerminalType As = Constant("as");
        public static TerminalType Is = Constant("is");
        public static TerminalType If = Constant("if");
        public static TerminalType Else = Constant("else");
        public static TerminalType For = Constant("for");
        public static TerminalType Function = Constant("function");
        public static TerminalType Class = Constant("class");
        public static TerminalType Record = Constant("record");
        public static TerminalType Type = Constant("type");
        public static TerminalType New = Constant("new");

        public static TerminalType This = Constant("this");
        public static TerminalType Base = Constant("base");
        public static TerminalType True = Constant("true");
        public static TerminalType False = Constant("false");
        public static TerminalType Null = Constant("null");
        public static TerminalType Void = Constant("void");
        public static TerminalType Undefined = Constant("undefined");
        public static TerminalType Any = Constant("any");

        static NLangTerminals()
        {
            Terminals = new List<TerminalType>()
            {
                Let,
                Const,
                Compile,
                As,
                Is,
                If,
                Else,
                For,
                Function,
                Class,
                Record,
                Type,
                New,

                Base,
                This,
                True,
                False,
                Null,
                Void,
                Undefined,
                Any,
                
                Id,
                String,
                Float,
                Int,
                
                Colon,
                Semicolon,
                LParen,
                RParen,
                LBrace,
                RBrace,
                LBracket,
                RBracket,
                Comma,
                Dot,
                DoubleQuestionMark,
                QuestionMark,
                Arrow,
                StrictEqual,
                Equal,
                Assign,
                NotStrictEqual,
                NotEqual,
                LessOrEqual,
                GreaterOrEqual,
                ShiftLeft,
                ShiftRight,
                Less,
                Greater,
                DoublePlus,
                Plus,
                DoubleMinus,
                Minus,
                Asterisk,
                DoubleSlash,
                Slash,
                DoubleHat,
                Hat,
                DoublePipe,
                Pipe,
                DoubleAmpersand,
                Ampersand,
                Tilda,
                ExclamationMark
            };

            foreach (var terminal in Terminals)
            {
                TerminalTypes[terminal.Name] = terminal;
            }
        }
    }
}