namespace Parser.Grammars.paths
{
    public class FilePath : ISourcePath
    {
        private string Filename { get; }
        private int Line { get; }
        private int Character { get; }

        public FilePath(string filename, int line, int character)
        {
            Filename = filename;
            Line = line;
            Character = character;
        }

        public string ToUserFriendlyPath()
        {
            return $"{Filename}, line {Line}:{Character}";
        }
    }
}