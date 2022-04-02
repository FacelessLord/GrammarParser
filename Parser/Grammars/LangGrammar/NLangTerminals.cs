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
        public static TerminalType Arrow = Constant("=>");
        public static TerminalType StrictEqual = Constant("===");
        public static TerminalType Equal = Constant("==");
        public static TerminalType Assign = Constant("=");
        public static TerminalType NotEqual = Constant("!=");
        public static TerminalType LessOrEqual = Constant("<=");
        public static TerminalType GreaterOrEqual = Constant(">=");
        public static TerminalType Less = Constant("<");
        public static TerminalType Greater = Constant(">");
        public static TerminalType Plus = Constant("+");
        public static TerminalType Minus = Constant("-");
        public static TerminalType Asterisk = Constant("*");
        public static TerminalType Slash = Constant("/");
        public static TerminalType Hat = Constant("^");
        public static TerminalType Pipe = Constant("|");
        public static TerminalType Ampersand = Constant("&");
        public static TerminalType Tilda = Constant("~");

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
                Arrow,
                StrictEqual,
                Equal,
                Assign,
                NotEqual,
                LessOrEqual,
                GreaterOrEqual,
                Less,
                Greater,
                Plus,
                Minus,
                Asterisk,
                Slash,
                Hat,
                Pipe,
                Ampersand,
                Tilda,
            };

            foreach (var terminal in Terminals)
            {
                TerminalTypes[terminal.Name] = terminal;
            }
        }
    }
}