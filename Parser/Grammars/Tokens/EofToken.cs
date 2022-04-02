﻿namespace Parser.Grammars.tokens
{
    public class EofToken : TerminalType
    {

        public EofToken() : base("eof")
        {
        }

        public override Match GetMatch(string text)
        {
            return new Match(false, 0, null);
        }
    }
}