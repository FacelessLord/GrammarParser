using System;
using System.Collections.Generic;
using System.Text;
using Parser.Grammars.tokens;

namespace Parser.Grammars.LangGrammar.Terminals
{
    public class StringTerminal : TerminalType
    {
        public StringTerminal() : base("string")
        {
        }

        public override Match GetMatch(string text)
        {
            if (text.StartsWith("\""))
            {
                var readString = new StringBuilder();
                var shouldSkipChar = false;
                var currentPosition = 1;
                for(; currentPosition < text.Length && (text[currentPosition] != '"' || shouldSkipChar); currentPosition++)
                {
                    if (shouldSkipChar)
                    {
                        readString.Append(text[currentPosition]);
                        shouldSkipChar = false;
                        continue;
                    }
                    shouldSkipChar = text[currentPosition] == '\\';
                    if(!shouldSkipChar)
                        readString.Append(text[currentPosition]);
                }
                if (text[currentPosition] == '"')
                    currentPosition++;
                return new Match(true, currentPosition, readString.ToString());
            }
            return new Match(false, 0, "");
        }
    }
}