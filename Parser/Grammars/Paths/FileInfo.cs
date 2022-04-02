namespace Parser.Grammars.Paths
{
    public class FileInfo
    {
        public ISourcePath Source { get; }
        public string Content { get; }

        public FileInfo(ISourcePath source, string content)
        {
            Source = source;
            Content = content;
        }
    }
}