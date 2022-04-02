using System;

namespace Parser.Grammars.Paths
{
    public class ContentPath : ISourcePath
    {
        private ISourcePath FilePath { get; }
        private int Line { get; }

        public ContentPath(ISourcePath filePath, int line)
        {
            FilePath = filePath;
            Line = line;
        }

        public string ToUserFriendlyPath()
        {
            return $"{FilePath.ToUserFriendlyPath()}, line {Line}";
        }

        public static ContentPath Max(ContentPath a, ContentPath b)
        {
            if (a.FilePath != b.FilePath)
                throw new InvalidOperationException();
            if (a.Line > b.Line)
                return a;
            return b;
        }

        public static ContentPath Min(ContentPath a, ContentPath b)
        {
            if (a.FilePath != b.FilePath)
                throw new InvalidOperationException();
            if (a.Line < b.Line)
                return a;
            return b;
        }
    }
}