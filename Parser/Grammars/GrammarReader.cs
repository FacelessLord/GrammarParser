using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Parser.Grammars;
using Parser.Grammars.tokens;

namespace Parser.GrammarReader
{
    public class GrammarReader
    {
        private const string TerminalsKeyword = "terminals";
        private const string RulesKeyword = "rules";

        private static readonly Regex WordRegex = new("\\s*(\\w+)\\s*", RegexOptions.Compiled);

        public Grammar Read(string grammarText)
        {
            grammarText = grammarText.Trim();
            var terminalsKeywordIndex = grammarText.IndexOf(TerminalsKeyword, StringComparison.OrdinalIgnoreCase);
            var rulesKeywordIndex = grammarText.IndexOf(RulesKeyword, StringComparison.OrdinalIgnoreCase);

            if (terminalsKeywordIndex == -1 && rulesKeywordIndex == -1)
            {
                throw new ArgumentException("Grammar text contains no keywords, so it's empty");
            }

            terminalsKeywordIndex = terminalsKeywordIndex == -1 ? int.MaxValue : terminalsKeywordIndex;
            rulesKeywordIndex = rulesKeywordIndex == -1 ? int.MaxValue : rulesKeywordIndex;
            var keywordIndex = Math.Min(terminalsKeywordIndex, rulesKeywordIndex);

            return ReadGrammar(grammarText[keywordIndex..]);
        }

        private Grammar ReadGrammar(string grammarText)
        {
            var state = ReaderState.Keyword;
            var storage = new AutomatonStorage();

            while (grammarText.Length > 0 && state != ReaderState.Finish)
                state = state switch
                {
                    ReaderState.Keyword => KeywordStep(storage, ref grammarText),
                    ReaderState.Terminals => TerminalsStep(storage, ref grammarText),
                    ReaderState.TerminalName => TerminalNameStep(storage, ref grammarText),
                    ReaderState.TerminalColon => TerminalColonStep(storage, ref grammarText),
                    ReaderState.TerminalApostrophe => TerminalApostropheStep(storage, ref grammarText),
                    ReaderState.TerminalApostropheEscape => TerminalApostropheEscapeStep(storage, ref grammarText),
                    ReaderState.TerminalApostropheEnd => TerminalApostropheEndStep(storage, ref grammarText),
                    ReaderState.Rules => RulesStep(storage, ref grammarText),
                    ReaderState.RuleName => RulesNameStep(storage, ref grammarText),
                    ReaderState.RuleColon => RulesColonStep(storage, ref grammarText),
                    ReaderState.RuleWords => RulesWordsStep(storage, ref grammarText),
                    _ => throw new ArgumentOutOfRangeException()
                };

            return BuildGrammarFromEntries(storage.Entries);
        }

        private static Grammar BuildGrammarFromEntries(List<GrammarEntry> storageEntries)
        {
            var terminals = new Dictionary<string, TerminalType>();
            var nonTerminals = new Dictionary<string, NonTerminalType>();

            storageEntries.Where(e => e is TerminalLiteral)
                .Cast<TerminalLiteral>()
                .Select(l => new VariableConstantTerminalType(l.Name.ToLower(), new[] { l.Value }))
                .ForEach(t => terminals[t.Name.ToLower()] = t);

            var rules = ResolveRuleExpressions(storageEntries.Where(e => e is RuleExpression)
                    .Cast<RuleExpression>(), terminals, nonTerminals)
                .GroupBy(r => r.Source)
                .ToDictionary(g => g.Key, g => g.ToHashSet());

            return new Grammar(nonTerminals[storageEntries.First(e => e is RuleExpression).Name], terminals.Values.ToHashSet(), nonTerminals.Values.ToHashSet(), rules);
        }

        private static IEnumerable<GrammarRule> ResolveRuleExpressions(IEnumerable<RuleExpression> expressions,
            Dictionary<string, TerminalType> terminals, Dictionary<string, NonTerminalType> nonTerminals)
        {
            var expressionList = expressions.ToHashSet();
            var rules = new HashSet<GrammarRule>();

            var preResolvedRefs = expressionList
                .Where(r => r.Value.All(terminals.ContainsKey))
                .ToList();

            while (expressionList.Count > 0)
            {
                if (preResolvedRefs.Count == 0)
                {
                    expressionList.ForEach(i =>
                        Console.WriteLine(
                            $"[Warn] Rule {i.Name} -> {string.Join(" ", i.Value)} is not productive. It will be omitted"));
                    break;
                }
                preResolvedRefs.Select(r => (r, ResolveRule(r, terminals, nonTerminals)))
                    .ForEach(p =>
                    {
                        var (expr, result) = p;
                        rules.Add(result);
                        expressionList.Remove(expr);
                    });
                preResolvedRefs = expressionList
                    .Where(r => r.Value.All(v => terminals.ContainsKey(v) || nonTerminals.ContainsKey(v)))
                    .ToList();
            }

            return rules;
        }

        private static GrammarRule ResolveRule(RuleExpression rule,
            IReadOnlyDictionary<string, TerminalType> terminals,
            IDictionary<string, NonTerminalType> nonTerminals)
        {
            var (sourceName, value) = rule;
            if (!nonTerminals.ContainsKey(sourceName))
                nonTerminals[sourceName] = new NonTerminalType(sourceName);
            var source = nonTerminals[sourceName];
            var production = value.Select(t =>
            {
                TokenType? token = terminals.TryGetValue(t, out var term) ? term :
                    nonTerminals.TryGetValue(t, out var nonTerm) ? nonTerm : null;
                if (token is null)
                    throw new KeyNotFoundException($"Token \"{t}\" has no productions");
                return token;
            }).ToArray();
            return new GrammarRule(source, production);
        }

        private static ReaderState KeywordStep(AutomatonStorage storage, ref string grammarText)
        {
            if (grammarText.StartsWith(RulesKeyword, StringComparison.OrdinalIgnoreCase))
            {
                ReadSequence(RulesKeyword, ref grammarText);
                return ReaderState.Rules;
            }

            ReadSequence(TerminalsKeyword, ref grammarText);
            return ReaderState.Terminals;
        }
        private static ReaderState TerminalsStep(AutomatonStorage storage, ref string grammarText)
        {
            if (grammarText.StartsWith(RulesKeyword, StringComparison.OrdinalIgnoreCase))
            {
                ReadSequence(RulesKeyword, ref grammarText);
                return ReaderState.Rules;
            }

            storage.SetName(ReadWord(ref grammarText));
            return ReaderState.TerminalName;
        }
        private static ReaderState TerminalNameStep(AutomatonStorage storage, ref string grammarText)
        {
            ReadSequence(":", ref grammarText);
            return ReaderState.TerminalColon;
        }
        private static ReaderState TerminalColonStep(AutomatonStorage storage, ref string grammarText)
        {
            ReadSequence("'", ref grammarText);
            return ReaderState.TerminalApostrophe;
        }
        private static ReaderState TerminalApostropheStep(AutomatonStorage storage, ref string grammarText)
        {
            if (grammarText.StartsWith("\\"))
            {
                ReadSequence("\\", ref grammarText);
                return ReaderState.TerminalApostropheEscape;
            }
            if (grammarText.StartsWith("'"))
            {
                ReadSequence("'", ref grammarText);
                return ReaderState.TerminalApostropheEnd;
            }
            storage.AddCharacter(grammarText[0]);
            grammarText = grammarText[1..].Trim();
            return ReaderState.TerminalApostrophe;
        }

        private static ReaderState TerminalApostropheEscapeStep(AutomatonStorage storage, ref string grammarText)
        {
            storage.AddCharacter(grammarText[0]);
            grammarText = grammarText[1..].Trim();
            return ReaderState.TerminalApostrophe;
        }

        private static ReaderState TerminalApostropheEndStep(AutomatonStorage storage, ref string grammarText)
        {
            if (grammarText.StartsWith("|"))
            {
                ReadSequence("|", ref grammarText);
                storage.StoreTerminalLiteral();
                ReadSequence("'", ref grammarText);
                return ReaderState.TerminalApostrophe;
            }
            ReadSequence(";", ref grammarText);
            storage.StoreTerminalLiteral();
            return ReaderState.Terminals;
        }

        private static ReaderState RulesStep(AutomatonStorage storage, ref string grammarText)
        {
            if (grammarText.StartsWith(TerminalsKeyword))
            {
                ReadSequence(TerminalsKeyword, ref grammarText);
                return ReaderState.Terminals;
            }

            storage.SetName(ReadWord(ref grammarText));
            return ReaderState.RuleName;
        }
        private static ReaderState RulesNameStep(AutomatonStorage storage, ref string grammarText)
        {
            ReadSequence(":", ref grammarText);
            return ReaderState.RuleColon;
        }
        private static ReaderState RulesColonStep(AutomatonStorage storage, ref string grammarText)
        {
            storage.AddReference(ReadWord(ref grammarText));
            return ReaderState.RuleWords;
        }
        private static ReaderState RulesWordsStep(AutomatonStorage storage, ref string grammarText)
        {
            if (grammarText.StartsWith(";"))
            {
                ReadSequence(";", ref grammarText);
                storage.StoreRule();
                return ReaderState.Rules;
            }
            if (grammarText.StartsWith("|"))
            {
                ReadSequence("|", ref grammarText);
                storage.StoreRule();
                storage.ResetReferences();
                return ReaderState.RuleWords;
            }
            storage.AddReference(ReadWord(ref grammarText));
            return ReaderState.RuleWords;
        }

        //=================== UTILITIES ======================
        private static void ReadSequence(string sequence, ref string text)
        {
            if (!text.StartsWith(sequence))
                throw new ArgumentException(
                    $"Invalid grammar. Expected \"{sequence}\" near \"{text[..6]}\" but found \"{text[..sequence.Length]}\"");
            text = text[sequence.Length..].Trim();
        }
        private static string ReadWord(ref string text)
        {
            var wordMatch = WordRegex.Match(text);
            if (string.IsNullOrEmpty(wordMatch.Groups[0].Value))
                throw new ArgumentException("Invalid grammar. Can't read word");

            text = text[wordMatch.Value.Length..].Trim();
            return wordMatch.Groups[1].Value;
        }

        private record GrammarEntry(string Name);

        private record TerminalLiteral(string Name, string Value) : GrammarEntry(Name);

        private record TerminalExpression(string Name, List<string> Value) : GrammarEntry(Name);

        private record RuleExpression(string Name, List<string> Value) : GrammarEntry(Name);

        private class AutomatonStorage
        {
            private readonly List<string> _references = new();
            private readonly StringBuilder _terminalBuilder = new();
            public readonly List<GrammarEntry> Entries = new();

            private string _name = "";

            public void Clear()
            {
                _name = "null";
                _references.Clear();
                _terminalBuilder.Clear();
            }
            //todo void ClearEntries()

            public void AddReference(string reference)
            {
                _references.Add(reference);
            }

            public void ResetReferences()
            {
                _references.Clear();
            }

            public void ResetTerminalBuilder()
            {
                _terminalBuilder.Clear();
            }

            public void AddCharacter(char character)
            {
                _terminalBuilder.Append(character);
            }

            public void SetName(string name)
            {
                _name = name;
            }

            public void StoreRule()
            {
                Entries.Add(new RuleExpression(_name, _references.ToList()));
                ResetReferences();
            }
            public void StoreTerminalExpression()
            {
                Entries.Add(new TerminalExpression(_name, _references.ToList()));
                ResetReferences();
            }
            public void StoreTerminalLiteral()
            {
                Entries.Add(new TerminalLiteral(_name, _terminalBuilder.ToString()));
                ResetTerminalBuilder();
            }
        }

        private enum ReaderState
        {
            Keyword,
            Terminals,
            TerminalName,
            TerminalColon,
            TerminalApostrophe,
            TerminalApostropheEscape,
            TerminalApostropheEnd,
            Rules,
            RuleName,
            RuleColon,
            RuleWords,
            Finish
        }
    }
}