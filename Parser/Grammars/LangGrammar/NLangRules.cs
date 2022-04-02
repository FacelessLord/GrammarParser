using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Parser.Nodes;
using Parser.Rules;

using static Parser.Grammars.LangGrammar.NLangTerminals;

namespace Parser.Grammars.LangGrammar
{
    public static class NLangRules
    {
        public static readonly RuleToken IdList = Rule.Named(nameof(IdList))
            .With(Chain.StartWith(Id).CollectBy(nodes => new IdListNode(nodes[0])))
            .With(_ => Chain.StartWith(_).Then(Comma).Then(Id).CollectBy(nodes => new IdListNode(nodes[0], nodes[1])));

        public static readonly RuleToken VariableModifiersList = Rule.Named(nameof(VariableModifiersList))
            .With(Chain.StartWith(Compile).CollectBy(nodes => new VariableModifiersListNode(nodes[0])))
            .With(Chain.StartWith(Const).CollectBy(nodes => new VariableModifiersListNode(nodes[0])))
            .With(Chain.StartWith(Let).CollectBy(nodes => new VariableModifiersListNode(nodes[0])))
            .With(_ => Chain.StartWith(_).Then(Compile)
                .CollectBy(nodes => new VariableModifiersListNode(nodes[0], nodes[1])))
            .With(_ =>Chain.StartWith(_).Then(Const)
                .CollectBy(nodes => new VariableModifiersListNode(nodes[0], nodes[1])))
            .With(_ =>Chain.StartWith(_).Then(Let)
                .CollectBy(nodes => new VariableModifiersListNode(nodes[0], nodes[1])));

        public static readonly RuleToken CreateVar = Rule.Named(nameof(CreateVar))
            .With(Chain.StartWith(VariableModifiersList).Then(IdList).CollectBy(nodes => new CreateVarNode(nodes)));

        static NLangRules()
        {
            Rules = typeof(NLangRules).GetFields(BindingFlags.Static | BindingFlags.Public| BindingFlags.DeclaredOnly)
                .Where(f => f.Name != nameof(Rules))
                .Select(f => f.GetValue(null))
                .Cast<RuleToken>()
                .Peek(r => r.FinishConfiguration())
                .ToList();
        }
        
        public static List<RuleToken> Rules;
    }
}