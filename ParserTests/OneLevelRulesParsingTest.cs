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
            node.Should().BeOfType<StatementNode>();
            var createVarExpressionNode = (ExpressionTreeLeafNode) ((StatementNode)node).BuildTree();
            var createVarNode = (CreateVarNode) createVarExpressionNode.Leaf;
            createVarNode.Type.Should().Be(NLangRules.CreateVar);
            createVarNode.Names.Count.Should().Be(1);
            createVarNode.Names[0].Should().Be(varName);
            createVarNode.Modifiers.Should().Be(modifiers);
        }

        [Test]
        public void ParsesPlusAndMinusParenatedExpression()
        {
            var node = Parse("x + y + z - (a + y);").Node;
            node.Should().BeOfType<StatementNode>();
            var tree = ExpressionTreeNode.BuildNode(((StatementNode) node).Statement.Nodes);
            tree.Operation.Should().Be(Operation.Minus);
            var left = tree.Args[0];
            left.Operation.Should().Be(Operation.Plus);
            var leftLeft = left.Args[0];
            leftLeft.Operation.Should().Be(Operation.Plus);
            
            var leftLeftLeft = (ExpressionTreeLeafNode)leftLeft.Args[0];
            var x = (TerminalNode) leftLeftLeft.Leaf;
            x.Type.Should().Be(NLangTerminals.Id);
            x.Match.Should().Be("x");

            var leftLeftRight = (ExpressionTreeLeafNode) leftLeft.Args[1];
            var y = (TerminalNode) leftLeftRight.Leaf;
            y.Type.Should().Be(NLangTerminals.Id);
            y.Match.Should().Be("y");

            var leftRight = (ExpressionTreeLeafNode) left.Args[1];
            var z = (TerminalNode) leftRight.Leaf;
            z.Type.Should().Be(NLangTerminals.Id);
            z.Match.Should().Be("z");

            var right = tree.Args[1];
            right.Operation.Should().Be(Operation.Plus);
            
            var rightLeft = (ExpressionTreeLeafNode) right.Args[0];
            var a = (TerminalNode)rightLeft.Leaf;
            a.Type.Should().Be(NLangTerminals.Id);
            a.Match.Should().Be("a");
            
            var rightRight = (ExpressionTreeLeafNode) right.Args[1];
            var y2 = (TerminalNode)rightRight.Leaf;
            y2.Type.Should().Be(NLangTerminals.Id);
            y2.Match.Should().Be("y");
        }
        
        // [TestCase("const a;", new[] { "a" })]
        // [TestCase("const a, b;", new[] { "a", "b" })]
        // [TestCase("const a, b, c, d , e,f,g ,h;", new[] { "a", "b", "c", "d", "e", "f", "g", "h" })]
        // public void ParsesIdList(string input, string[] result)
        // {
        //     var node = Parse(input).Node;
        //     node.Should().BeOfType<StatementNode>();
        //     var tree = ExpressionTreeNode.BuildNode(((StatementNode) node).Statement.Nodes);
        //     var leaf = (ExpressionTreeLeafNode) tree;
        //     var createVarNode = (CreateVarNode) leaf.Leaf;
        //     var ids = createVarNode.Names;
        //     ids.Should().BeEquivalentTo(result);
        // }
    }
}