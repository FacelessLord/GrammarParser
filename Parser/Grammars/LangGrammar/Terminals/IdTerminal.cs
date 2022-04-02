using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Parser.Grammars.tokens;
using Match = Parser.Grammars.tokens.Match;

namespace Parser.Grammars.LangGrammar.Terminals
{
    public class IdTerminal : TerminalType
    {

        public IdTerminal() : base("id")
        {
        }

        private static Regex Id = new Regex("^([a-zA-z_]\\w*)([^\\w].*)?$",
            RegexOptions.Singleline | RegexOptions.Compiled);

        public override Match GetMatch(string text)
        {
            if (Id.Match(text) is var match && match.Success)
            {
                var value = match.Groups[1].Value;
                return new Match(true, value.Length, value);
            }
            return new Match(false, 0, "");
        }
    }
}