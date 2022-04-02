using System;

namespace Parser.Grammars.Paths
{
    public class ContentSpan
    {
        public ContentPath FilePathStart { get; }
        public ContentPath FilePathEnd { get; }

        public ContentSpan(ContentPath filePath)
        {
            FilePathStart = filePath;
            FilePathEnd = filePath;
        }

        public ContentSpan(ContentPath filePathStart, ContentPath filePathEnd)
        {
            FilePathStart = filePathStart;
            FilePathEnd = filePathEnd;
        }

        public string ToUserFriendlyPath()
        {
            return $"From {FilePathStart.ToUserFriendlyPath()} to {FilePathEnd.ToUserFriendlyPath()}";
        }

        public static ContentSpan Aggregate(ContentSpan a, ContentSpan b)
        {
            return new ContentSpan(ContentPath.Min(a.FilePathStart, b.FilePathStart),
                ContentPath.Max(a.FilePathEnd, b.FilePathEnd));
        }
    }
}