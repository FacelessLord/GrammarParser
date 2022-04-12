using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Parser.Grammars.Tokens;
using Match = Parser.Grammars.Tokens.Match;

namespace Parser.Grammars.LangGrammar.Terminals
{
    public class IntTerminal : TerminalType
    {

        public IntTerminal() : base("int")
        {
        }

        private static Regex Binary =
            new Regex("^(0b[01]+)([^01]|$).*", RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex Octal = new Regex("^(0o[0-7]+)([^0-7]|$).*",
            RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex Decimal =
            new Regex("^(\\d+)([^\\d]|$).*", RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex Hex = new Regex("^(0x[\\dA-F]+)([^\\dA-F]|$).*",
            RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex[] Int = { Hex, Octal, Binary, Decimal };

        public override Match GetMatch(string text)
        {
            foreach (var type in Int)
            {
                if (type.Match(text) is var match && match.Success)
                {
                    var value = match.Groups[1].Value;
                    return new Match(true, value.Length, value);
                }
            }
            return new Match(false, 0, "");
        }
    }
}