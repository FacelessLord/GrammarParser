using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Parser.Grammars;
using Parser.Grammars.tokens;

namespace ParserTests
{
    [TestFixture]
    public class GrammarTest
    {
        private static TerminalType Constant(string name) => new ConstantTerminalType(name);

        private static NonTerminalType NonTerm(string name) => new NonTerminalType(name);

        private Grammar Grammar;

        private TerminalType x = Constant("x");
        private TerminalType y = Constant("y");
        private TerminalType plus = Constant("+");
        private TerminalType lParen = Constant("(");
        private TerminalType rParen = Constant(")");

        private NonTerminalType expr = NonTerm("expr");
        private NonTerminalType term = NonTerm("term");
        private NonTerminalType sum = NonTerm("sum");
        private NonTerminalType parens = NonTerm("parens");

        [SetUp]
        public void SetUp()
        {
            var terminals = new HashSet<TerminalType>()
                { x, y, plus, lParen, rParen };
            var nonTerminals = new HashSet<NonTerminalType>()
                { expr, sum, parens, term };

            var expRule1 = new GrammarRule(expr, new TokenType[] { sum }, nodes => nodes[0]);
            var expRule2 = new GrammarRule(expr, new TokenType[] { parens }, nodes => nodes[0]);
            var expRule3 = new GrammarRule(expr, new TokenType[] { term }, nodes => nodes[0]);
            var sumRule = new GrammarRule(sum, new TokenType[] { expr, plus, expr }, nodes => nodes[0]);
            var parenRule = new GrammarRule(parens, new TokenType[] { lParen, expr, rParen }, nodes => nodes[0]);
            var xRule = new GrammarRule(term, new TokenType[] { x }, nodes => nodes[0]);
            var yRule = new GrammarRule(term, new TokenType[] { y }, nodes => nodes[0]);

            var rules = new HashSet<GrammarRule>() { sumRule, parenRule, xRule, yRule, expRule1, expRule2, expRule3 }
                .GroupBy(r => r.Source)
                .ToDictionary(g => g.Key, g => g.ToHashSet());

            Grammar = new Grammar(expr, terminals, nonTerminals, rules);
        }

        [Test]
        public void BuildsFirstCorrectly()
        {
            Grammar.First[x].Should().OnlyContain(t => t == x);
            Grammar.First[y].Should().OnlyContain(t => t == y);
            Grammar.First[plus].Should().OnlyContain(t => t == plus);
            Grammar.First[lParen].Should().OnlyContain(t => t == lParen);
            Grammar.First[rParen].Should().OnlyContain(t => t == rParen);

            Grammar.First[expr].Should().BeEquivalentTo(new HashSet<TerminalType>() { x, y, lParen });
            Grammar.First[sum].Should().BeEquivalentTo(new HashSet<TerminalType>() { x, y, lParen });
            Grammar.First[parens].Should().BeEquivalentTo(new HashSet<TerminalType>() { lParen });
        }

        [TestCase(10)]
        [TestCase(20)]
        [TestCase(100)]
        [TestCase(1000)]
        public void BuildsFirstForDeepGrammarCorrectly(int depth)
        {
            var terminals = new HashSet<TerminalType>() { x };
            var axiom = new NonTerminalType("axiom");
            var nonTerminals = new List<NonTerminalType>() { axiom };
            for (var i = 0; i < depth; i++)
            {
                nonTerminals.Add(NonTerm("" + i));
            }
            var nonTermToTermRule = new GrammarRule(nonTerminals.Last(), new[] { x }, nodes => nodes[0]);

            var rulesSet = new HashSet<GrammarRule>() { nonTermToTermRule };
            for (var i = 0; i < depth; i++)
            {
                rulesSet.Add(new GrammarRule(nonTerminals[i], new[] { nonTerminals[i + 1] }, nodes => nodes[0]));
            }

            var rules = rulesSet
                .GroupBy(r => r.Source)
                .ToDictionary(g => g.Key, g => g.ToHashSet());

            Grammar = new Grammar(axiom, terminals, nonTerminals.ToHashSet(), rules);

            for (var i = 0; i < depth + 1; i++)
            {
                Grammar.First[nonTerminals[i]].Should().OnlyContain(t => t == x);
            }
        }

        [Test]
        public void BuildsFollowCorrectly()
        {
            Grammar.Follow[x].Should().BeEquivalentTo(new[] { plus, rParen, TokenType.Eof });
            Grammar.Follow[y].Should().BeEquivalentTo(new[] { plus, rParen, TokenType.Eof });
            Grammar.Follow[plus].Should().BeEquivalentTo(new[] { x, y, lParen });
            Grammar.Follow[lParen].Should().BeEquivalentTo(new[] { x, y, lParen });
            Grammar.Follow[rParen].Should().BeEquivalentTo(new[] { plus, rParen, TokenType.Eof });

            Grammar.Follow[expr].Should().BeEquivalentTo(new HashSet<TerminalType>() { plus, rParen, TokenType.Eof });
            Grammar.Follow[sum].Should().BeEquivalentTo(new HashSet<TerminalType>() { plus, rParen, TokenType.Eof });
            Grammar.Follow[parens].Should().BeEquivalentTo(new HashSet<TerminalType>() { plus, rParen, TokenType.Eof });
        }

        [TestCase(10)]
        [TestCase(20)]
        [TestCase(100)]
        [TestCase(1000)]
        public void BuildsFollowForTwoBranchedDeepGrammarCorrectly(int depth)
        {
            var terminals = new HashSet<TerminalType>() { x, y };
            var axiom = new NonTerminalType("axiom");
            var nonTerminals = new List<NonTerminalType>() { axiom };
            for (var i = 1; i < depth; i++)
            {
                nonTerminals.Add(NonTerm("+" + i));
            }
            for (var i = 1; i < depth; i++)
            {
                nonTerminals.Add(NonTerm("-" + i));
            }

            var rulesSet = new HashSet<GrammarRule>();
            for (var i = 1; i < depth - 1; i++)
            {
                rulesSet.Add(new GrammarRule(nonTerminals[i], new[] { nonTerminals[i + 1] }, nodes => nodes[0]));
            }
            rulesSet.Add(new GrammarRule(nonTerminals[depth - 1], new[] { x }, nodes => nodes[0]));
            for (var i = depth; i < 2 * depth - 2; i++)
            {
                rulesSet.Add(new GrammarRule(nonTerminals[i], new[] { nonTerminals[i + 1] }, nodes => nodes[0]));
            }
            rulesSet.Add(new GrammarRule(nonTerminals[2 * depth - 2], new[] { y }, nodes => nodes[0]));
            rulesSet.Add(new GrammarRule(axiom, new[] { nonTerminals[1], nonTerminals[depth] }, nodes => nodes[0]));

            var rules = rulesSet
                .GroupBy(r => r.Source)
                .ToDictionary(g => g.Key, g => g.ToHashSet());

            Grammar = new Grammar(axiom, terminals, nonTerminals.ToHashSet(), rules);

            Grammar.Follow[axiom].Should().BeEquivalentTo(new[] { TokenType.Eof });

            var followX = new[] { y };
            var followY = new[] { TokenType.Eof };

            for (var i = 1; i < depth; i++)
            {
                Grammar.Follow[nonTerminals[i]].Should().BeEquivalentTo(followX);
            }
            for (var i = depth; i < 2 * depth - 1; i++)
            {
                Grammar.Follow[nonTerminals[i]].Should().BeEquivalentTo(followY);
            }
        }

        [TestCase(10, 3)]
        [TestCase(20, 4)]
        [TestCase(100, 10)]
        [TestCase(1000, 20)]
        public void BuildsFollowForNBranchedDeepGrammarCorrectly(int depth, int branches)
        {
            var terminalsList = new List<TerminalType>();
            for (var j = 0; j < branches; j++)
            {
                terminalsList.Add(Constant("#" + j));
            }

            var axiom = new NonTerminalType("axiom");
            var nonTerminals = new List<NonTerminalType>() { };
            for (var j = 0; j < branches; j++)
            for (var i = 0; i < depth; i++)
            {
                nonTerminals.Add(NonTerm(j + "|" + i));
            }

            var rulesSet = new HashSet<GrammarRule>();
            for (var j = 0; j < branches; j++)
            {
                for (var i = j * depth + 0; i < (j + 1) * depth - 1; i++)
                {
                    rulesSet.Add(new GrammarRule(nonTerminals[i], new[] { nonTerminals[i + 1] }, nodes => nodes[0]));
                }
                rulesSet.Add(new GrammarRule(nonTerminals[(j + 1) * depth - 1], new[] { terminalsList[j] },
                    nodes => nodes[0]));
            }
            var axiomProduction = new TokenType[branches];
            for (int j = 0; j < branches; j++)
            {
                axiomProduction[j] = nonTerminals[j * depth];
            }
            rulesSet.Add(new GrammarRule(axiom, axiomProduction, nodes => nodes[0]));

            var rules = rulesSet
                .GroupBy(r => r.Source)
                .ToDictionary(g => g.Key, g => g.ToHashSet());

            Grammar = new Grammar(axiom, terminalsList.ToHashSet(), nonTerminals.Append(axiom).ToHashSet(), rules);

            Grammar.Follow[axiom].Should().BeEquivalentTo(new[] { TokenType.Eof });

            var lastBranchFollow = new[] { TokenType.Eof };

            for (var j = 0; j < branches - 1; j++)
            {
                var branchFollow = new[] { terminalsList[j + 1] };
                for (var i = 0; i < depth; i++)
                {
                    Grammar.Follow[nonTerminals[i + j * depth]].Should().BeEquivalentTo(branchFollow);
                }
            }
            for (var i = depth * (branches - 1); i < depth * branches; i++)
            {
                Grammar.Follow[nonTerminals[i]].Should().BeEquivalentTo(lastBranchFollow);
            }
        }
    }
}