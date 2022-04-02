namespace Parser.Grammars.Paths
{
    public static class PathExtensions
    {
        public static ContentPath At(this ISourcePath path, int line)
        {
            return new ContentPath(path, line);
        }
    }
}