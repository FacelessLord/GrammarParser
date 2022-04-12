using System.Collections.Generic;
using Parser.Grammars.LangGrammar.Terminals;
using Parser.Grammars.Tokens;

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
        public static TerminalType StrictEqual = Operation("===", global::Parser.Grammars.Tokens.Operation.StrictEqual);
        public static TerminalType Equal = Operation("==", global::Parser.Grammars.Tokens.Operation.Equal);
        public static TerminalType Assign = Operation("=", global::Parser.Grammars.Tokens.Operation.Assign);
        public static TerminalType NotStrictEqual = Operation("!==", global::Parser.Grammars.Tokens.Operation.NotStrictEqual);
        public static TerminalType NotEqual = Operation("!=", global::Parser.Grammars.Tokens.Operation.NotEqual);
        public static TerminalType LessOrEqual = Operation("<=", global::Parser.Grammars.Tokens.Operation.LessOrEqual);
        public static TerminalType GreaterOrEqual = Operation(">=", global::Parser.Grammars.Tokens.Operation.GreaterOrEqual);
        public static TerminalType ShiftLeft = Operation("<<", global::Parser.Grammars.Tokens.Operation.ShiftLeft);
        public static TerminalType ShiftRight = Operation(">>", global::Parser.Grammars.Tokens.Operation.ShiftRight);
        public static TerminalType Less = Operation("<", global::Parser.Grammars.Tokens.Operation.Less);
        public static TerminalType Greater = Operation(">", global::Parser.Grammars.Tokens.Operation.Greater);
        public static TerminalType DoublePlus = Operation("++", global::Parser.Grammars.Tokens.Operation.Plus, global::Parser.Grammars.Tokens.Operation.UnaryPlus);
        public static TerminalType Plus = Operation("+", global::Parser.Grammars.Tokens.Operation.Plus, global::Parser.Grammars.Tokens.Operation.UnaryPlus);
        public static TerminalType DoubleMinus = Operation("--", global::Parser.Grammars.Tokens.Operation.Minus, global::Parser.Grammars.Tokens.Operation.UnaryMinus);
        public static TerminalType Minus = Operation("-", global::Parser.Grammars.Tokens.Operation.Minus, global::Parser.Grammars.Tokens.Operation.UnaryMinus);
        public static TerminalType Asterisk = Operation("*", global::Parser.Grammars.Tokens.Operation.Multiply);
        public static TerminalType DoubleSlash = Operation("//", global::Parser.Grammars.Tokens.Operation.DivideInt);
        public static TerminalType Slash = Operation("/", global::Parser.Grammars.Tokens.Operation.Divide);
        public static TerminalType Hat = Operation("^", global::Parser.Grammars.Tokens.Operation.BitwiseXor);
        public static TerminalType DoubleHat = Operation("^^", global::Parser.Grammars.Tokens.Operation.Xor);
        public static TerminalType Pipe = Operation("|", global::Parser.Grammars.Tokens.Operation.BitwiseOr);
        public static TerminalType DoublePipe = Operation("||", global::Parser.Grammars.Tokens.Operation.Or);
        public static TerminalType Ampersand = Operation("&", global::Parser.Grammars.Tokens.Operation.BitwiseAnd);
        public static TerminalType DoubleAmpersand = Operation("&&", global::Parser.Grammars.Tokens.Operation.And);
        public static TerminalType Tilda = Operation("~", global::Parser.Grammars.Tokens.Operation.BitwiseNot);
        public static TerminalType ExclamationMark = Operation("!", global::Parser.Grammars.Tokens.Operation.Not);
        public static TerminalType PrimeOp = Operation("", global::Parser.Grammars.Tokens.Operation.PrimeOp);

        public static TerminalType Var = Constant("var");
        public static TerminalType Const = Constant("const");
        public static TerminalType As = Constant("as");
        public static TerminalType Is = Constant("is");
        public static TerminalType In = Constant("in");
        public static TerminalType If = Constant("if");
        public static TerminalType Else = Constant("else");
        public static TerminalType Foreach = Constant("foreach");
        public static TerminalType For = Constant("for");
        public static TerminalType Break = Constant("break");
        public static TerminalType Continue = Constant("continue");
        public static TerminalType Throw = Constant("throw");
        public static TerminalType Goto = Constant("goto");
        public static TerminalType Return = Constant("return");
        public static TerminalType While = Constant("while");
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
                Var,
                Const,
                As,
                Is,
                If,
                Else,
                Foreach,
                For,
                Break,
                Continue,
                Throw,
                Goto,
                Return,
                While,
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
                In,
                
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