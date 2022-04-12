namespace Parser.Grammars.Tokens
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
    public class SuperEofToken : TerminalType
    {

        public SuperEofToken() : base("superEof")
        {
        }

        public override Match GetMatch(string text)
        {
            return new Match(false, 0, null);
        }
    }
}