using Parser.GrammarReader;

namespace Runner
{
    public static class Runner
    {
        public static void Main(string[] args)
        {
            var grammarText = "terminals " +
                              "x: 'x';" +
                              "star: '*';" +
                              "plus: '+';" +
                              "rules " +
                              "S0: S;" +
                              "S: S plus P | P;" +
                              "P: P star T | T;" +
                              "T: x;";
            var grammarReader = new GrammarReader();
            var grammar = grammarReader.Read(grammarText);
            var x = 1;
        }
    }
}