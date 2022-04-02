namespace Parser.Grammars.Paths
{
    public class FilePath : ISourcePath
    {
        private string Filename { get; }

        public FilePath(string filename)
        {
            Filename = filename;
        }

        public string ToUserFriendlyPath()
        {
            return $"{Filename}";
        }
    }
}