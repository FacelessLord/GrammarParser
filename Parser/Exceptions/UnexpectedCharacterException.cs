using System;

namespace Parser.Exceptions
{
    public class UnexpectedCharacterException : Exception
    {
        public UnexpectedCharacterException(string content) : base(
            $"Unexpected character around \"...{content[..Math.Min(10, content.Length)]}...\"")
        {
        }
    }
}