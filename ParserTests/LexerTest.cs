using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Parser;
using Parser.Exceptions;
using Parser.Grammars.LangGrammar.Terminals;
using Parser.Grammars.Paths;
using Parser.Grammars.Tokens;

namespace ParserTests
{
    public class LexerTest
    {
        private static TerminalType Constant(string name) => new ConstantTerminalType(name);

        public Lexer Lexer { get; set; }
        [SetUp]
        public void SetUp()
        {
            Lexer = new Lexer(new List<TerminalType>
            {
                new StringTerminal(),
                new FloatTerminal(),
                new IntTerminal(),
                new IdTerminal(),

                Constant(":"),
                Constant(";"),
                Constant("("),
                Constant(")"),
                Constant(","),
                Constant("let"),
                Constant("null"),
                Constant("void"),
            });
        }

        [TestCase("\"asf\"", "string", "asf")]
        [TestCase("\"\"", "string", "")]
        [TestCase("0", "int", "0")]
        [TestCase("0123456789", "int", "0123456789")]
        [TestCase("0b010001011", "int", "0b010001011")]
        [TestCase("0o01234567", "int", "0o01234567")]
        [TestCase("0x9ABCDF", "int", "0x9ABCDF")]
        [TestCase("01234.56789", "float", "01234.56789")]
        [TestCase("0b0100.01011", "float", "0b0100.01011")]
        [TestCase("0o01.234567", "float", "0o01.234567")]
        [TestCase("0x9AB.CDF", "float", "0x9AB.CDF")]
        public void ParsesSingleToken(string inputText, string expectedType, string expectedMatch)
        {
            var fileInfo = new FileInfo(null, inputText);
            var token = Lexer.Parse(fileInfo).Single();
            var node = (TerminalNode) token.Node;
            node.Type.Name.Should().Be(expectedType);
            node.Match.Should().Be(expectedMatch);
        }

        [TestCase("\"asf")]
        [TestCase("\"")]
        [TestCase("-")]
        [TestCase("0123456789A")]
        [TestCase("0b0100010112")]
        [TestCase("0o012345678")]
        [TestCase("0x9ABCDFG")]
        [TestCase("01234..56789A")]
        [TestCase("0b0100..010112")]
        [TestCase("0o01..2345678")]
        [TestCase("0x9AB..CDFG")]
        public void NotParsesSingleBrokenToken(string inputText)
        {
            var fileInfo = new FileInfo(null, inputText);
            Func<NodeWrapper> parse = (() => Lexer.Parse(fileInfo).Single());
            parse.Should().Throw<Exception>();
        }
    }
}