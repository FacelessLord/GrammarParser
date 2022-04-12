using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Parser;
using Parser.Automaton;
using Parser.Grammars;
using Parser.Grammars.LangGrammar;
using Parser.Grammars.Paths;
using Parser.Grammars.Tokens;
using Parser.Nodes;
using Parser.Nodes.Statements;
using Parser.Nodes.Variables;

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

        [TestCase("{int x;}", "x", "int")]
        [TestCase("{string VerylongUpperCamelCaseName;}", "VerylongUpperCamelCaseName", "string")]
        [TestCase("{bool very_long_lower_camel_case_name;}", "very_long_lower_camel_case_name", "bool")]
        public void ParsesVariableDeclaration(string input, string varName, string typeMatch)
        {
            var expect = Wrap(NLangRules.Statements.Block,
                new StatementListNode(
                    Wrap(NLangRules.Statements.Statement,
                        Wrap(NLangRules.Statements.DeclarationStatement,
                            new LocalVariableDeclarationNode(
                                Wrap(NLangRules.Variables.LocalVariableType, 
                                    new TypeNode(Id(typeMatch))),
                                new LocalVariableDeclaratorListNode(
                                    new LocalVariableDeclaratorNode(Id(varName))))
                        ))));

            var node = Parse(input).Node;
            node.Should().Be(expect);
        }
        
        [TestCase("{var x;}", "x", "var")]
        public void ParsesVariableDeclarationWithTypeInfer(string input, string varName, string typeMatch)
        {
            var expect = Wrap(NLangRules.Statements.Block,
                new StatementListNode(
                    Wrap(NLangRules.Statements.Statement,
                        Wrap(NLangRules.Statements.DeclarationStatement,
                            new LocalVariableDeclarationNode(
                                Wrap(NLangRules.Variables.LocalVariableType, Term(NLangTerminals.Var, typeMatch)),
                                new LocalVariableDeclaratorListNode(
                                    new LocalVariableDeclaratorNode(Id(varName))))
                        ))));

            var node = Parse(input).Node;
            node.Should().Be(expect);
        }
        
        
        public static TerminalNode Id(string name)
        {
            return new TerminalNode(NLangTerminals.Id, name);
        }
        public static TerminalNode Int(string name)
        {
            return new TerminalNode(NLangTerminals.Int, name);
        }
        public static TerminalNode Term(TokenType type, string value)
        {
            return new TerminalNode(type, value);
        }
        public static WrapperNode Wrap(TokenType type, INode node)
        {
            return new WrapperNode(type, node);
        }
    }

    public static class NodeExtensions
    {
    }
}