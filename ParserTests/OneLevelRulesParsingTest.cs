using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Parser;
using Parser.Automaton;
using Parser.Grammars;
using Parser.Grammars.LangGrammar;
using Parser.Grammars.Paths;
using Parser.Grammars.tokens;
using Parser.Nodes;

namespace ParserTests
{
    [TestFixture]
    public class OneLevelRulesParsingTest
    {
        public ParserAutomaton Automaton { get; set; }
        public Lexer Lexer { get; set; }

        public NodeWrapper Parse(string input)
        {
            var fileInfo = new FileInfo(new FilePath("test.nlang"), input);

            var tokenStream = Lexer.Parse(fileInfo);
            return Automaton.Parse(tokenStream);
        }
        

        [SetUp]
        public void SetUp()
        {
            Lexer = new Lexer(NLangTerminals.Terminals);
            var builder = new AutomatonBuilder();
            Automaton = builder.Build(NLangGrammar.Grammar);
        }

        [TestCase("let x", "x", VariableModifiers.Let)]
        [TestCase("const x", "x", VariableModifiers.Const)]
        [TestCase("compile x", "x", VariableModifiers.Compile)]
        public void ParsesCreateVarRule(string input, string varName, VariableModifiers modifiers)
        {
            var node = Parse(input).Node;
            node.Should().BeOfType<CreateVarNode>();
            var createVarNode = (CreateVarNode) node;
            createVarNode.Type.Should().Be(NLangRules.CreateVar);
            createVarNode.Names.Count.Should().Be(1);
            createVarNode.Names[0].Should().Be(varName);
            createVarNode.Modifiers.Should().Be(modifiers);
        }
        //
        // [TestCase("a", new[] { "a" })]
        // [TestCase("a, b", new[] { "a", "b" })]
        // [TestCase("a, b, c, d, e, f,g ,h", new[] { "a", "b", "c", "d", "e", "f", "g", "h" })]
        // public void ParsesIdList(string input, string[] result)
        // {
        //     var builder = new AutomatonBuilder();
        //     Automaton = builder.Build(new Grammar(NLangGrammar.Grammar, NLangNonTerminals.IdList), true);
        //     var node = Parse(input).Node;
        //     node.Should().BeOfType<IdListNode>();
        //     var idList = (IdListNode) node;
        //     idList.Type.Should().Be(NLangNonTerminals.IdList);
        //     idList.Ids.Should().BeEquivalentTo(result);
        // }
    }
}